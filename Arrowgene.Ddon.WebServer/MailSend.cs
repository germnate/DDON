using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Logging;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Arrowgene.Ddon.WebServer
{
    public class MailSend
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(DdonWebServer));
        private readonly MailSetting _mailSetting;

        public MailSend(MailSetting mailSetting)
        {
            _mailSetting = mailSetting ?? throw new ArgumentNullException(nameof(mailSetting));
        }
        public async Task SendAsync(string mailModel, Account account, CancellationToken cancellationToken = default)
        {
            Dictionary<string, string> placeholders = new();

            if (string.IsNullOrWhiteSpace(mailModel))
            {
                Logger.Error($"MailSend - Mail model not provided");
                return;
            }

            if (account == null)
            {
                Logger.Error($"MailSend - Invalid account provided");
                return;
            }

            var templateFile = Path.Combine(_mailSetting.TemplatePath, $"{mailModel}.html");

            if (!File.Exists(templateFile))
            {
                Logger.Error($"MailSend - Template not found: {templateFile}");
                return;
            }

            var body = await File.ReadAllTextAsync(templateFile, cancellationToken);

            var baseUrl = _mailSetting.DomainUrl?.TrimEnd('/') ?? throw new InvalidOperationException("ServerUrl is not configured");

            if(mailModel == "new_account" || mailModel == "mail_verify")
            {
                if (string.IsNullOrWhiteSpace(account.MailToken))
                {
                    Logger.Error("MailSend - A mail_token is required for this mail model");
                    return;
                }

                placeholders = new Dictionary<string, string>
                {
                    ["{{UserName}}"] = WebUtility.HtmlEncode(account.Name),
                    ["{{DomainUrl}}"] = baseUrl,
                    ["{{VerificationLink}}"] = $"http://{baseUrl}:52099/web/verify.html?token={Uri.EscapeDataString(account.MailToken)}",
                    ["{{Year}}"] = DateTime.UtcNow.Year.ToString()
                };
            }
            else if(mailModel == "password_reset")
            {
                if (string.IsNullOrWhiteSpace(account.PasswordToken))
                {
                    Logger.Error("MailSend - A password_token is required for this mail model");
                    return;
                }
                placeholders = new Dictionary<string, string>
                {
                    ["{{UserName}}"] = WebUtility.HtmlEncode(account.Name),
                    ["{{DomainUrl}}"] = baseUrl,
                    ["{{PasswordResetLink}}"] = $"http://{baseUrl}:52099/web/reset_password.html?token={Uri.EscapeDataString(account.PasswordToken)}",
                    ["{{Year}}"] = DateTime.UtcNow.Year.ToString()
                };
            }
            else
            {
                Logger.Error($"MailSend - Unsupported mail model: {mailModel}");
                return;
            }

            foreach (var item in placeholders)
            {
                body = body.Replace(item.Key, item.Value);
            }

            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(_mailSetting.FromAddress));
            message.To.Add(MailboxAddress.Parse(account.Mail));
            message.Subject = SubjectFor(mailModel);

            message.Body = new TextPart("html")
            {
                Text = body
            };

            using var smtpClient = new SmtpClient();

            await smtpClient.ConnectAsync(
                _mailSetting.SmtpServer,
                _mailSetting.SmtpPort,
                SecureSocketOptions.StartTls,
                cancellationToken
            );

            if (!string.IsNullOrWhiteSpace(_mailSetting.SmtpUser))
            {
                await smtpClient.AuthenticateAsync(
                    _mailSetting.SmtpUser,
                    _mailSetting.SmtpPassword,
                    cancellationToken
                );
            }

            await smtpClient.SendAsync(message, cancellationToken);
            await smtpClient.DisconnectAsync(true, cancellationToken);
        }

        private static string SubjectFor(string mailModel)
        {
            return mailModel switch
            {
                "new_account" => "Welcome to Dragon's Dogma Online",
                "mail_verify" => "Verify your updated e-mail",
                "reset_password" => "Password reset",
                _ => "Dragon's Dogma Online"
            };
        }
    }
}

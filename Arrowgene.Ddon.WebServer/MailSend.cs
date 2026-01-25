using Arrowgene.Ddon.Database.Model;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.WebServer
{
    public class MailSend
    {

        private readonly MailSetting _mailSetting;

        public MailSend(MailSetting mailSetting)
        {
            _mailSetting = mailSetting
                ?? throw new ArgumentNullException(nameof(mailSetting));
        }
        public async Task SendAsync(string mailModel, Account account, CancellationToken cancellationToken = default)
        {
            Dictionary<string, string> placeholders = new();

            if (string.IsNullOrWhiteSpace(mailModel))
                throw new ArgumentException("mailModel is required");

            if (account == null)
                throw new ArgumentNullException(nameof(account));

            var templateFile = Path.Combine(_mailSetting.TemplatePath, $"{mailModel}.html");

            if (!File.Exists(templateFile))
            {
                throw new FileNotFoundException($"Template not found: {templateFile}");
            }

            var body = await File.ReadAllTextAsync(
                templateFile,
                cancellationToken
            );

            var baseUrl = _mailSetting.DomainUrl?.TrimEnd('/') ?? throw new InvalidOperationException("ServerUrl is not configured");

            if(mailModel == "new_account" || mailModel == "mail_verify")
            {
                if (string.IsNullOrWhiteSpace(account.MailToken))
                {
                    throw new InvalidOperationException("Account MailToken is required for this mail model");
                }

                placeholders = new Dictionary<string, string>
                {
                    ["{{UserName}}"] = WebUtility.HtmlEncode(account.Name),
                    ["{{ServerUrl}}"] = baseUrl,
                    ["{{VerificationLink}}"] = $"{baseUrl}:52099/web/verify.html?token={Uri.EscapeDataString(account.MailToken)}",
                    ["{{Year}}"] = DateTime.UtcNow.Year.ToString()
                };
            }
            else if(mailModel == "password_reset")
            {
                if (string.IsNullOrWhiteSpace(account.PasswordToken))
                {
                    throw new InvalidOperationException("Account PasswordToken is required for this mail model");
                }
                placeholders = new Dictionary<string, string>
                {
                    ["{{UserName}}"] = WebUtility.HtmlEncode(account.Name),
                    ["{{ServerUrl}}"] = baseUrl,
                    ["{{PasswordResetLink}}"] = $"{baseUrl}:52099/web/reset_password.html?token={Uri.EscapeDataString(account.PasswordToken)}",
                    ["{{Year}}"] = DateTime.UtcNow.Year.ToString()
                };
            }
            else
            {
                throw new InvalidOperationException($"Unsupported mail model: {mailModel}");
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

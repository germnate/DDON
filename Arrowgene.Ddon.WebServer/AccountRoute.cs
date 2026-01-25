using System;
using System.Security.AccessControl;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Shared.Crypto;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using Arrowgene.WebServer;
using Arrowgene.WebServer.Route;
using Microsoft.AspNetCore.Server.HttpSys;

namespace Arrowgene.Ddon.WebServer
{
    public class AccountRoute : WebRoute
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(AccountRoute));


        public override string Route => "/api/account";

        private readonly IDatabase _database;
        private readonly MailSend _mail;
        private class AccountRequest
        {
            public string Action { get; set; }
            public string Account { get; set; }
            public string Email { get; set; }
            public string EmailToken { get; set; }
            public string Password { get; set; }
            public string PasswordToken { get; set; }
        }

        private class AccountResponse
        {
            public string Error { get; set; }
            public string Message { get; set; }
            public string Token { get; set; }
        }

        private class AccountVerification
        {
            public bool Error { get; set; }
            public string Message { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }

            public AccountVerification(string username, string password, string email)
            {
                Username = username;
                Password = password;
                Email = email;
                
                // Very simple data checks on the parameters.

                if (Username.Trim().Length == 0)
                {
                    Error = true;
                    Message = "Account ID cannot be empty";
                    return;
                }

                // Disallow any whitespace.

                if (Regex.IsMatch(Username, @"\s"))
                {
                    Error = true;
                    Message = "Account ID cannot contain spaces";
                    return;
                }

                if (Password.Trim().Length == 0)
                {
                    Error = true;
                    Message = "Password cannot be empty";
                    return;
                }

                if (Regex.IsMatch(Password, @"\s"))
                {
                    Error = true;
                    Message = "Password cannot contain spaces";
                    return;
                }
                
                if (Email == null || Email.Trim().Length == 0)
                {
                    Error = true;
                    Message = "E-mail cannot be empty";
                    return;
                }

                if (Regex.IsMatch(Email, @"\s"))
                {
                    Error = true;
                    Message = "E-mail cannot contain spaces";
                    return;
                }

                if (!Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    Error = true;
                    Message = "Invalid e-mail";
                    return;
                }
            }
        }

        public AccountRoute(IDatabase database, MailSetting mailSetting)
        {
            _database = database;
            _mail = new MailSend(mailSetting);
        }

        public override async Task<WebResponse> Post(WebRequest request)
        {
            AccountRequest req = await request.ReadJsonAsync<AccountRequest>();
            if (req == null)
            {
                return await WebResponse.InternalServerError();
            }

            AccountResponse res = new AccountResponse();

            switch (req.Action)
            {
                case "login":

                    string token = CreateLoginToken(req.Account, req.Password);
                    if (token == null)
                    {
                        res.Error = "Account or password wrong";
                        break;
                    }

                    if (token == "mail")
                    {
                        res.Error = "Email not verified yet.";
                        break;
                    }

                    res.Message = "Login Success";
                    res.Token = token;
                    break;

                case "create":
                    AccountVerification accountCheck = new(req.Account, req.Password, req.Email);

                    if (accountCheck.Error)
                    {
                        res.Error = accountCheck.Message;
                        break;
                    }

                    Account account = CreateAccount(req.Account, req.Email, req.Password);
                    if (account == null)
                    {
                        res.Error = "Account or e-mail already in use";
                        break;
                    }

                    res.Message = "Account created";
                    await _mail.SendAsync("new_account", account);
                    break;

                case "recover":
                    account = CreatePasswordToken(req.Email);

                    if (account == null)
                    {
                        res.Error = "Account not found";
                        break;
                    }

                    if (!account.MailVerified)
                    {
                        res.Message = "E-mail not verified yet";
                        break;
                    }

                    res.Message = "Password token generated";
                    await _mail.SendAsync("password_reset", account);
                    break;

                case "reset":
                    account = ResetPassword(req.Password, req.PasswordToken);

                    if (account == null)
                    {
                        res.Error = "Invalid account or token";
                        break;
                    }

                    res.Message = "Password changed";
                    break;

                case "verify":
                    bool verification = VerifyEmail(req.EmailToken);
                    if (!verification)
                    {
                        res.Error = "Email not found";
                        break;
                    }
                    
                    res.Message = "Email verified";
                    break;

                case "resend":
                    account = ResendEmailVerification(req.Email);
                    if (account == null)
                    {
                        res.Error = "Email not found";
                        break;
                    }

                    res.Message = "Verification token resent";
                    await _mail.SendAsync("mail_verify", account);

                    break;

            }

            WebResponse response = new WebResponse();
            response.StatusCode = 200;
            await response.WriteJsonAsync(res);
            return response;
        }

        private Account CreateAccount(string name, string mail, string password)
        {
            Account account = _database.SelectAccountByName(name);
            if (account != null)
            {
                Logger.Error($"{name} - CreateAccount: account already taken");
                return null;
            }

            Account email = _database.SelectAccountByEmail(mail);
            if (email != null)
            {
                Logger.Error($"{mail} - CreateAccount: email already taken");
                return null;
            }

            string hash = PasswordHash.CreateHash(password);
            account = _database.CreateAccount(name, mail, hash);
            return account;
        }

        private string CreateLoginToken(string name, string password)
        {
            Account account = _database.SelectAccountByName(name);
            if (account == null)
            {
                Logger.Error($"{name} - CreateToken: account does not exist");
                return null;
            }

            if (!PasswordHash.Verify(password, account.Hash))
            {
                Logger.Error($"{name} - CreateToken: wrong password provided");
                return null;
            }

            if (!account.MailVerified)
            {
                Logger.Error($"{name} - CreateToken: email not verified yet");
                return "mail";
            }

            account.LoginToken = GameToken.GenerateToken();
            account.LoginTokenCreated = DateTime.UtcNow;
            _database.UpdateAccount(account);
            return account.LoginToken;
        }

        private Account ResetPassword(string newPassword, string passwordToken)
        {
            Account account = _database.SelectAccountByPasswordToken(passwordToken);
            if (account == null)
            {
                Logger.Error("ResetPassword: account does not exist");
                return null;
            }

            if (account.PasswordToken != passwordToken)
            {
                Logger.Error($"ResetPassword: invalid token");
                account.PasswordToken = null;
                _database.UpdateAccount(account);
                return null;
            }

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                Logger.Error($"ResetPassword: invalid password");
                account.PasswordToken = null;
                _database.UpdateAccount(account);
                return null;
            }

            account.PasswordToken = null;
            account.Hash = PasswordHash.CreateHash(newPassword);
            _database.UpdateAccount(account);
            return account;
        }

        private Account CreatePasswordToken(string mail)
        {
            Account account = _database.SelectAccountByEmail(mail);
            if (account == null)
            {
                Logger.Error($"{mail} - CreatePasswordToken: account does not exist");
                return null;
            }

            account.PasswordToken = GameToken.GenerateToken();
            _database.UpdateAccount(account);
            return account;
        }

        private bool VerifyEmail(string emailToken)
        {
            
            Account account = _database.SelectAccountByMailToken(emailToken);
            if (account == null)
            {
                Logger.Error("VerifyEmail: account does not exist");
                return false;
            }

            if (account.MailToken != emailToken)
            {
                Logger.Error($"{account.NormalName} - VerifyEmail: invalid email token");
                return false;
            }

            account.MailToken = null;
            account.MailVerified = true;
            account.MailVerifiedAt = DateTime.UtcNow;
            _database.UpdateAccount(account);
            return true;
        }

        private Account ResendEmailVerification(string mail)
        {
            Account account = _database.SelectAccountByEmail(mail);
            if (account == null)
            {
                Logger.Error("ResendEmailVerification: account does not exist");
                return null;
            }

            account.MailToken = GameToken.GenerateToken();
            _database.UpdateAccount(account);
            return account;
        }
    }
}

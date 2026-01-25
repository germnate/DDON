using System.IO;
using System.Runtime.Serialization;
using Arrowgene.Ddon.Shared;

namespace Arrowgene.Ddon.WebServer
{
    [DataContract]
    public class MailSetting
    {
        [DataMember(Order = 1)]
        public string DomainUrl { get; set; }
        [DataMember(Order = 2)]
        public string SmtpServer { get; set; }

        [DataMember(Order = 3)]
        public int SmtpPort { get; set; }

        [DataMember(Order = 4)]
        public string SmtpUser { get; set; }

        [DataMember(Order = 5)]
        public string SmtpPassword { get; set; }

        [DataMember(Order = 6)]
        public string FromAddress { get; set; }

        [DataMember(Order = 7)]
        public string TemplatePath { get; set; }

        public MailSetting()
        {
            SetDefaultValues();
        }

        public MailSetting(MailSetting setting)
        {
            DomainUrl = setting.DomainUrl;
            SmtpServer = setting.SmtpServer;
            SmtpPort = setting.SmtpPort;
            SmtpUser = setting.SmtpUser;
            SmtpPassword = setting.SmtpPassword;
            FromAddress = setting.FromAddress;
            TemplatePath = setting.TemplatePath;
        }

        [OnDeserializing]
        void OnDeserializing(StreamingContext context)
        {
            SetDefaultValues();
        }

        [OnDeserialized]
        void OnDeserialized(StreamingContext context)
        {
            DomainUrl ??= "https://www.dd.on";
            SmtpServer ??= "smtp.dd.on";
            FromAddress ??= "no-reply@dd.on";
            TemplatePath ??= Path.Combine(Util.ExecutingDirectory(), "Files/mail_templates");
        }

        private void SetDefaultValues()
        {
            DomainUrl = "https://www.dd.on";
            SmtpServer = "smtp.dd.on";
            SmtpPort = 587;
            SmtpUser = "no-reply@dd.on";
            SmtpPassword = "p@55VV0rD";
            FromAddress = "no-reply@dd.on";
            TemplatePath = Path.Combine(Util.ExecutingDirectory(), "Files/mail_templates");
        }
    }
}

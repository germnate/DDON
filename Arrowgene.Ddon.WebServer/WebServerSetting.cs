using System.IO;
using System.Net;
using System.Runtime.Serialization;
using Arrowgene.Ddon.Shared;
using Arrowgene.WebServer;

namespace Arrowgene.Ddon.WebServer
{
    [DataContract]
    public class WebServerSetting
    {
        public WebServerSetting()
        {
            PublicWebEndPoint = new WebEndPoint();
            PublicWebEndPoint.Port = 52099;
            PublicWebEndPoint.IpAddress = IPAddress.Loopback;

            WebSetting = new WebSetting();
            WebSetting.ServerHeader = "";
            WebSetting.WebFolder = Path.Combine(Util.ExecutingDirectory(), "Files/www");

            WebSetting.WebEndpoints.Clear();
            WebEndPoint httpEndpoint = new WebEndPoint();
            httpEndpoint.Port = 52099;
            httpEndpoint.IpAddress = IPAddress.Any;
            WebSetting.WebEndpoints.Add(httpEndpoint);

            MailSetting = new MailSetting();
        }

        public WebServerSetting(WebServerSetting webServerSetting)
        {
            PublicWebEndPoint = new WebEndPoint(webServerSetting.PublicWebEndPoint);
            WebSetting = new WebSetting(webServerSetting.WebSetting);
            MailSetting = new MailSetting(webServerSetting.MailSetting);
        }

        [OnDeserialized]
        void OnDeserialized(StreamingContext context)
        {
            PublicWebEndPoint ??= new WebEndPoint();
            WebSetting ??= new WebSetting();
            MailSetting ??= new MailSetting();
        }

        [DataMember(Order = 1)]
        public WebEndPoint PublicWebEndPoint { get; set; }

        [DataMember(Order = 2)]
        public WebSetting WebSetting { get; set; }

        [DataMember(Order = 3)]
        public MailSetting MailSetting { get; set; }

    }
}

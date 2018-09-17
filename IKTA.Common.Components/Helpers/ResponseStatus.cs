using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;


namespace IKTA.Common.Components.Helpers
{
    public class ResponseStatus
    {
        public static XmlDocument GetResponseStatus(int statusKode)
        {
            var statusMsg = GetStatusMessage(statusKode);
            return GetResponseStatus(statusKode, statusMsg);
        }

        public static XmlDocument GetResponseStatus(int statusKode, string message)
        {
            XmlDocument xmlDocument = new XmlDocument();
            XDocument xDocument;
            XNamespace nameSpace = "http://IKTA.Common.Schemas.ResponseStatus";

            xDocument = new XDocument(
                 new XElement(nameSpace + "ResponseStatus",
                     new XAttribute(XNamespace.Xmlns + "ns0", nameSpace),
                     new XElement("StatusKode", statusKode),
                     new XElement("Beskrivelse", message)
                     ));

            xmlDocument.Load(xDocument.CreateReader());
            return xmlDocument;
        }

        private static string GetStatusMessage(int statusCode)
        {
            var msg = string.Empty;
            var ssoString = "Http" + statusCode.ToString();
            try
            {
                msg = IKTA.Common.Components.Helpers.SSO.GetSSOString("IKTA.Common", ssoString);
            }
            catch
            {
                msg = "Ukjent feil. Kontakt IKT-Agder.";
            }
            return msg;
        }
    }
}

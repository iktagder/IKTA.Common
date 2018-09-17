using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace IKTA.Common.Components.WCFBehaviors
{
    public class SanitizedFaultHandler : IErrorHandler
    {
        public bool HandleError(Exception error)
        {
            return true;
        }

        public void ProvideFault(Exception error, MessageVersion version, ref Message Fault)
        {
            // Analyze error message and extract reason for failing

            string statusCode = "500";
            string errorMessage = "Intern feil. Kontact IKT-Agder";
            if (error.Message.Contains("Reason: The XML Validator failed to validate."))
            {
                statusCode = "400";
                errorMessage = error.Message.Substring(error.Message.LastIndexOf("Detaljer: ") + 9);
            }
            if (error.Message.Contains("Reason: Access to service denied."))
            {
                statusCode = "403";
                errorMessage = "Akseess til service avvist.";
            }
            if (error.Message.Contains("GetMappedValue") && error.Message.Contains("Vigilo"))
            {
                statusCode = "400";
                errorMessage = "Produkt eller firma ikke definert i mapping.";
            }

            var xmlResponseStatus = $"<ns0:ResponseStatus xmlns:ns0='http://IKTA.Common.Schemas.ResponseStatus'><StatusKode>{statusCode}</StatusKode><Beskrivelse>{errorMessage}</Beskrivelse></ns0:ResponseStatus>";

            //XDocument errorMsg = XDocument.Parse(error.Message);
            HttpResponseMessageProperty prop = new HttpResponseMessageProperty();
            prop.Headers[HttpResponseHeader.ContentType] = "application/json; charset=utf-8";

            // get the HTTP status code
            prop.StatusCode = (HttpStatusCode)Int32.Parse(statusCode);

            //// this doesn't need to be included in the message
            XDocument errDoc = XDocument.Parse(xmlResponseStatus);

            Fault = Message.CreateMessage(version, null, new JsonErrorBodyWriter(errDoc));
            Fault.Properties.Add(HttpResponseMessageProperty.Name, prop);
            Fault.Properties.Add(WebBodyFormatMessageProperty.Name, new WebBodyFormatMessageProperty(WebContentFormat.Json));
        }
    }

    class JsonErrorBodyWriter : BodyWriter
    {
        Encoding utf8Encoding = new UTF8Encoding(false);
        private XDocument exMsg;

        public JsonErrorBodyWriter(XDocument exMsg): base(true)
        {
            this.exMsg = exMsg;
        }

        protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
        {
            writer.WriteStartElement("root");
            writer.WriteAttributeString("type", "object");

            XElement root = exMsg.Root;

            foreach (XElement el in root.Descendants())
            {
                writer.WriteStartElement(el.Name.ToString());
                writer.WriteAttributeString("type", "string");
                writer.WriteString(el.Value.ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IKTA.Common.Components.Helpers;
using log4net.helpers;

namespace IKTA.Common.UnitTests
{
    [TestClass]
    public class SecurityTests
    {
        [TestMethod]
        public void IsValidService()
        {
            var isValid = IKTA.Common.Components.Helpers.Security.IsValidService("2B84E4DC-8818-447D-BC5D-42926D1CD53A", "Rcv_Invoices_OneWay_v1_0");
        }

        [TestMethod]
        public void ExtractErrorMessageTest()
        {
            var e = "There was a failure executing the receive pipeline: \"IKTA.Common.Pipelines.JsonToXmlPipeline, IKTA.Common.Pipelines, Version = 1.0.0.0, Culture = neutral, PublicKeyToken = 28f58901cad08dbb\" Source: \"XML validator\" Receive Port: \"Rcv_Invoices_v1_0\" URI: \"/IKTAgderApi/Service1.svc\" Reason: The XML Validator failed to validate." +
                     "Details: The element 'Fakturering' in namespace 'http://IKTA.Common.Schemas.v1_0.Fakturaer' has invalid child element 'Something'. List of possible elements expected: 'FraSystem'..  ";
            var s = ExtractErrorMessage(e);

            var e2  = "There was a failure executing the receive pipeline: \"IKTA.Common.Pipelines.JsonToXmlPipeline, IKTA.Common.Pipelines, Version = 1.0.0.0, Culture = neutral, PublicKeyToken = 28f58901cad08dbb\" Source: \"API Access Validator\" Receive Port: \"Rcv_Invoices_v1_0\" URI: \" / IKTAgderApi / Service1.svc\" Reason: Access to service denied. ";
            var s2 = ExtractErrorMessage(e2);
            var ss = "000";
        }

        public string ExtractErrorMessage(string errMsg)
        {
            string extractedErrMsg = String.Empty;

            if (errMsg.Contains("Reason: The XML Validator failed to validate."))
            {
                return errMsg.Substring(errMsg.LastIndexOf("Details: ") + 9);
            }

            if (errMsg.Contains("Reason: Access to service denied."))
            {
                return "Access to service denied.";
            }
            return extractedErrMsg;
        }
    }
}

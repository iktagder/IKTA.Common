using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKTA.Common.UnitTests
{
    [TestClass]
    public class ResponseStatusTests
    {
        [TestMethod]
        public void Get_ResposeStatusFromKnownHttpCode()
        {
            var xmlDocument = IKTA.Common.Components.Helpers.ResponseStatus.GetResponseStatus(400);
            var str = xmlDocument.InnerXml;
            Assert.IsTrue(str.Contains("Feil ved anmodning"));
        }
        [TestMethod]
        public void Get_ResposeStatusFromUnknownHttpCode()
        {
            var xmlDocument = IKTA.Common.Components.Helpers.ResponseStatus.GetResponseStatus(197);
            var str = xmlDocument.InnerXml;
            Assert.IsTrue(str.Contains("Ukjent feil"));
        }
        [TestMethod]
        public void OverrideStatusMsgWithKnownHttpCode()
        {
            var xmlDocument = IKTA.Common.Components.Helpers.ResponseStatus.GetResponseStatus(400,"override");
            var str = xmlDocument.InnerXml;
            Assert.IsTrue(str.Contains("override"));
        }
        [TestMethod]
        public void OverrideStatusMsgWithUnknownHttpCode()
        {
            var xmlDocument = IKTA.Common.Components.Helpers.ResponseStatus.GetResponseStatus(197,"override");
            var str = xmlDocument.InnerXml;
            Assert.IsTrue(str.Contains("override"));
        }
    }
}

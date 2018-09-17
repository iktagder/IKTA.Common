using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using IKTA.Common.Components.Helpers;

namespace IKTA.Common.UnitTests
{
    [TestClass]
    public class SSOTests
    {
        [TestMethod]
        public void We_should_be_able_to_get_an_integer_from_SSO()
        {
            var value = SSO.GetSSOInt32("IKTA.Common", "CommandTimeout");
            Assert.AreEqual(30, value);
        }

        [TestMethod]
        public void We_should_be_able_to_get_a_string_from_SSO()
        {
            var value = SSO.GetSSOString("IKTA.Common", "SsoAppUserGroup");
            Assert.AreEqual("BizTalk Application Users", value);
        }

        [TestMethod]
        public void Requesting_missing_item_from_SSO_should_throw_an_exception()
        {
            var exceptionThrown = false;
            try
            {
                SSO.GetSSOInt32("IKTA.Common", "missingValue");
            }
            catch (Exception e)
            {
                exceptionThrown = true;
            }
            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        public void Requesting_item_from_nonexistant_application_from_SSO_should_throw_an_exception()
        {
            var exceptionThrown = false;
            try
            {
                SSO.GetSSOInt32("NonExisting", "missingValue");
            }
            catch (Exception e)
            {
                exceptionThrown = true;
            }
            Assert.IsTrue(exceptionThrown);
        }
    }
}

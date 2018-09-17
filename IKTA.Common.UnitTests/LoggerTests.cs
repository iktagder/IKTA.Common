using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IKTA.Common.Components.Helpers;
using log4net.helpers;

namespace IKTA.Common.UnitTests
{
    [TestClass]
    public class LoggerTests
    {
        [TestMethod]
        public void We_should_be_able_to_log_an_error_message()
        {
            try
            {
                Logger.LogError("","Dette går ikke");
            }
            catch (Exception e2)
            {
                var dang = "";
            }
        }
    }
}

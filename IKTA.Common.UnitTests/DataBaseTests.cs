using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IKTA.Common.Components.Helpers;


namespace IKTA.Common.UnitTests
{
    [TestClass]
    public class DataBaseTests
    {
        [TestMethod]
        public void Missing_FromSystem_should_throw_an_exception()
        {
            var exceptionThrown = false;
            try
            {
                DataBase.GetMappedValue("", "fromField", "fromValue", "toSystem", "toField");
            }
            catch (Exception e)
            {
                exceptionThrown = true;
            }
            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        public void We_should_ge_an_exception_if_mapped_value_was_not_found()
        {
            var exceptionThrown = false;
            try
            {
                DataBase.GetMappedValue("fromSystem", "fromField", "fromValue", "toSystem", "toField");
            }
            catch (Exception e)
            {
                exceptionThrown = true;
            }
            Assert.IsTrue(exceptionThrown);
        }


        [TestMethod]
        public void We_should_get_a_mapped_value()
        {
            var mappedValue = DataBase.GetMappedValue("IKT-Agder", "Faktura.EierKode", "Grimstad kommune", "XLedger", "SO01b.EntityCode");
            Assert.AreEqual("13126", mappedValue);
        }

        [TestMethod]
        public void We_should_get_a_mapped_value_with_default()
        {
            var mappedValue = DataBase.GetMappedValueWithDefault("IKT-Agder", "Faktura.EierKode", "Grimstad kommune", "XLedger", "SO01b.EntityCode","defaultValue");
            Assert.AreEqual("13126", mappedValue);
        }

        [TestMethod]
        public void We_should_get_a_default_value_if_not_found_when_asking_for_default()
        {
            var mappedValue = DataBase.GetMappedValueWithDefault("IKT-Agder", "Faktura.EierKode", "Ukjent kommune", "XLedger", "SO01b.EntityCode", "defaultValue");
            Assert.AreEqual("defaultValue", mappedValue);
        }
    }
}

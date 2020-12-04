using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BucketGame.UnitTest
{
    [TestClass]
    public class ContainerTypes
    {
        [TestMethod]
        public void IsPrime_InputIs1_ReturnFalse()
        {
            bool result = false;

            Assert.IsFalse(result, "1 should not be prime");
        }
    }
}

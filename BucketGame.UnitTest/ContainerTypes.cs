using System;
using BucketGame.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BucketGame.UnitTest
{
    [TestClass]
    public class ContainerTypes
    {
        #region Constructors
        [TestMethod]
        public void CreateNewBucket()
        {
            Bucket bucket0 = new Bucket();

            Assert.IsInstanceOfType(bucket0, typeof(Bucket), "bucket0 is not of type Bucket");
        }
        #endregion
    }
}

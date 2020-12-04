using System;
using System.Diagnostics;
using BucketGame.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static BucketGame.Constants.UnitTesting;
using static BucketGame.Constants.RainBarrel;
using static BucketGame.Constants.OilBarrel;

namespace BucketGame.UnitTest
{
    [TestClass]
    public class ContainerTypes
    {
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            Debug.WriteLine("Started unit testing using MSTest");
        }

        #region Constructors
        [DataTestMethod]
        [TestCategory(CategoryTypes.Constructor)]
        // Bucket
        [DataRow(12, 13, typeof(Bucket))]
        [DataRow(15, 15, typeof(Bucket))]
        [DataRow(13, 12, typeof(Bucket))]
        [DataRow(12, null, typeof(Bucket))]
        [DataRow(15, null, typeof(Bucket))]

        // RainBarrel
        [DataRow(50, RainBarrelSmall, typeof(RainBarrel))]
        [DataRow(130, RainBarrelMedium, typeof(RainBarrel))]
        [DataRow(200, RainBarrelLarge, typeof(RainBarrel))]
        [DataRow(200, null, typeof(RainBarrel))]
        [DataRow(100, null, typeof(RainBarrel))]

        // OilBarrel
        [DataRow(50, OilBarrelCap, typeof(OilBarrel))]
        [DataRow(200, OilBarrelCap, typeof(OilBarrel))]
        [DataRow(80, null, typeof(OilBarrel))]
        [DataRow(1000, null, typeof(OilBarrel))]
        [DataRow(10, null, typeof(OilBarrel))]
        public void CreateNewBucket(int? content, int? capacity, Type containerType)
        {
            // Create undefined container variable
            Container container;

            // If content is not null
            if (!content.HasValue)
            {
                container = Activator.CreateInstance(containerType) as Container;
            }

            // If capacity is not null
            else if (!capacity.HasValue)
            {
                container = Activator.CreateInstance(containerType, content.Value) as Container;
            }

            // Else use content and capacity
            else
            {
                container = Activator.CreateInstance(containerType, content.Value, capacity.Value) as Container;
            }

            // Check if bucket is a type of ... and is a type from Container
            Assert.IsInstanceOfType(container, typeof(Container), $"{container?.GetType().Name} is not a type from Container");

            // If input content is lower then bucketCap
            if (content <= container?.Capacity)
            {
                Assert.AreEqual(content, container.Content, $"{container.GetType().Name} is not inputted content");
            }

            // Else if this was not the case, the container should have overflowed so less then actual content
            else
            {
                Assert.AreNotEqual(content, container?.Content, "bucket is equal to inputted content");
            }

            // If capacity was inputted it should be the same as what container currently has
            if (capacity.HasValue)
            {
                Assert.AreEqual(capacity, container?.Capacity);
            }
        }
        #endregion

        #region Properties
        [DataTestMethod]
        [TestCategory(CategoryTypes.Properties)]
        [DataRow(typeof(Bucket))]
        [DataRow(typeof(RainBarrel))]
        [DataRow(typeof(OilBarrel))]
        public void CheckContainerProperties(Type containerType)
        {
            // Create a new container with the type of containerType
            Container container = Activator.CreateInstance(containerType) as Container;

            // Check if content and capacity exist on container
            Assert.IsNotNull(container?.Content);
            Assert.IsNotNull(container.Capacity);

            // Check if Content/Capacity is higher then zero and Content is not higher then Capacity
            Assert.IsTrue(container.Content >= 0);
            Assert.IsTrue(container.Capacity >= 0);
            Assert.IsTrue(container.Content <= container.Capacity);
        }
        #endregion
    }
}

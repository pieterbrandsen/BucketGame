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
            Debug.WriteLine("Testing all public members/properties");
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
                Assert.AreEqual(capacity, container?.Capacity, "Input capacity is not equal to containers Capacity");
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
            Assert.IsNotNull(container?.Content, "Missing Content on container");
            Assert.IsNotNull(container.Capacity, "Missing Capacity on container");

            // Check if Content/Capacity is higher then zero and Content is not higher then Capacity
            Assert.IsTrue(container.Content >= 0, "Content is lower then zero");
            Assert.IsTrue(container.Capacity >= 0, "Capacity is not higher then zero");
            Assert.IsTrue(container.Content <= container.Capacity, "Content is higher then Capacity");
        }
        #endregion

        #region Methods
        [DataTestMethod]
        [TestCategory(CategoryTypes.Methods)]
        [DataRow(typeof(Bucket), typeof(Bucket))]
        [DataRow(typeof(Bucket), typeof(RainBarrel))]
        [DataRow(typeof(Bucket), typeof(OilBarrel))]
        [DataRow(typeof(RainBarrel), typeof(Bucket))]
        [DataRow(typeof(RainBarrel), typeof(RainBarrel))]
        [DataRow(typeof(RainBarrel), typeof(OilBarrel))]
        [DataRow(typeof(OilBarrel), typeof(Bucket))]
        [DataRow(typeof(OilBarrel), typeof(RainBarrel))]
        [DataRow(typeof(OilBarrel), typeof(OilBarrel))]
        public void CheckFillMethodUsingContainer(Type containerType, Type containerType2)
        {
            // Create 2 new containers with their type as a container
            Container targetContainer = Activator.CreateInstance(containerType) as Container;
            Container container = Activator.CreateInstance(containerType2) as Container;

            // Fill targetContainer using the other container
            targetContainer?.Fill(container);
        }

        [DataTestMethod]
        [TestCategory(CategoryTypes.Methods)]
        [DataRow(3, typeof(Bucket))]
        [DataRow(5, typeof(Bucket))]
        [DataRow(10, typeof(Bucket))]
        [DataRow(40, typeof(RainBarrel))]
        [DataRow(80, typeof(RainBarrel))]
        [DataRow(120, typeof(RainBarrel))]
        [DataRow(50, typeof(OilBarrel))]
        [DataRow(100, typeof(OilBarrel))]
        [DataRow(150, typeof(OilBarrel))]
        public void CheckFillMethodUsingAmount(int amount, Type containerType)
        {
            // Create a new container with the type of containerType
            Container container = Activator.CreateInstance(containerType) as Container;

            // Check if container is null so it can return that test failed
            if (container == null)
            {
                Assert.Fail("Container was null, please try again");
            }

            // Set content to Cap / 2 as default content
            container.Content = container.Capacity / 2;

            // Assign current content to current Content
            int? preCap = container?.Content;
            // Fill container using amount
            container.Fill(amount);

            // If expected content is lower or equal to Capacity
            if (preCap + amount <= container.Capacity)
            {
                // Check if preCap + amount is the same as current Content
                Assert.AreEqual(preCap + amount, container.Content);
            }
            else
            {
                // Content should be the same as Capacity because of overflow
                Assert.AreEqual(container.Content, container.Capacity);
            }
        }

        [DataTestMethod]
        [TestCategory(CategoryTypes.Methods)]
        [DataRow(3, typeof(Bucket))]
        [DataRow(5, typeof(Bucket))]
        [DataRow(10, typeof(Bucket))]
        [DataRow(40, typeof(RainBarrel))]
        [DataRow(80, typeof(RainBarrel))]
        [DataRow(120, typeof(RainBarrel))]
        [DataRow(50, typeof(OilBarrel))]
        [DataRow(100, typeof(OilBarrel))]
        [DataRow(150, typeof(OilBarrel))]
        public void CheckAddContentMethodUsingAmount(int amount, Type containerType)
        {
            // Create a new container with the type of containerType
            Container container = Activator.CreateInstance(containerType) as Container;

            // Check if container is null so it can return that test failed
            if (container == null)
            {
                Assert.Fail("Container was null, please try again");
            }

            // Set content to Cap / 2 as default content
            container.Content = container.Capacity / 2;

            // Assign current content to current Content
            int? preCap = container?.Content;
            // AddContent to container using amount
            container.AddContent(amount);

            // If expected content is lower or equal to Capacity
            if (preCap + amount <= container.Capacity)
            {
                // Check if preCap + amount is the same as current Content
                Assert.AreEqual(preCap + amount, container.Content);
            }
            else
            {
                // Content should be the same as Capacity because of overflow
                Assert.AreEqual(container.Content, container.Capacity);
            }
        }

        #endregion
    }
}

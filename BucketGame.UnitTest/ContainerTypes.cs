using System;
using System.Diagnostics;
using BucketGame.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static BucketGame.Constants.ContainerTypes;
using static BucketGame.Constants.OilBarrel;
using static BucketGame.Constants.RainBarrel;
using static BucketGame.Constants.UnitTesting;
using static BucketGame.Models.ContainerEvents;

namespace BucketGame.UnitTest
{
    [TestClass]
    public class ContainerTypes : Container
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
        [DataRow(null, null, typeof(Bucket))]
        [DataRow(12, 8, typeof(Bucket))]
        [DataRow(15, 15, typeof(Bucket))]
        [DataRow(13, 12, typeof(Bucket))]
        [DataRow(12, 200, typeof(Bucket))]
        [DataRow(15, null, typeof(Bucket))]

        // RainBarrel
        [DataRow(null, null, typeof(RainBarrel))]
        [DataRow(50, RainBarrelSmall, typeof(RainBarrel))]
        [DataRow(130, RainBarrelMedium, typeof(RainBarrel))]
        [DataRow(200, RainBarrelLarge, typeof(RainBarrel))]
        [DataRow(200, 200, typeof(RainBarrel))]
        [DataRow(100, null, typeof(RainBarrel))]

        // OilBarrel
        [DataRow(null, null, typeof(OilBarrel))]
        [DataRow(50, OilBarrelCap, typeof(OilBarrel))]
        [DataRow(200, OilBarrelCap, typeof(OilBarrel))]
        [DataRow(80, 500, typeof(OilBarrel))]
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
                Assert.AreNotEqual(content, container?.Content, $"{container?.GetType().Name} is equal to inputted content");
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
        [DataRow(typeof(Bucket), 5, typeof(Bucket), 12)]
        [DataRow(typeof(Bucket), 10, typeof(RainBarrel), RainBarrelSmall)]
        [DataRow(typeof(Bucket), 50, typeof(OilBarrel), OilBarrelCap)]
        [DataRow(typeof(RainBarrel), 100, typeof(Bucket), 10)]
        [DataRow(typeof(RainBarrel), 60, typeof(RainBarrel), RainBarrelMedium)]
        [DataRow(typeof(RainBarrel), 50, typeof(OilBarrel), OilBarrelCap)]
        [DataRow(typeof(OilBarrel), 2, typeof(Bucket), 9)]
        [DataRow(typeof(OilBarrel), 9, typeof(RainBarrel), RainBarrelLarge)]
        [DataRow(typeof(OilBarrel), 9, typeof(OilBarrel), OilBarrelCap)]
        public void CheckFillMethodUsingContainer(Type containerType, int containerAmount, Type containerType2, int container2Amount)
        {
            // Create 2 new containers with their type as a container
            Container targetContainer = Activator.CreateInstance(containerType) as Container;
            Container container = Activator.CreateInstance(containerType2) as Container;

            // If container used to fill with is not null, set Content to inputted content
            if (targetContainer != null && container != null)
            {
                targetContainer.Content = containerAmount;
                container.Content = container2Amount;
            }

            // Fill targetContainer using the other container
            targetContainer?.Fill(container);
        }

        [DataTestMethod]
        [TestCategory(CategoryTypes.Methods)]
        [DataRow(3, typeof(Bucket))]
        [DataRow(5, typeof(Bucket))]
        [DataRow(10, typeof(Bucket))]
        [DataRow(-5, typeof(Bucket))]
        [DataRow(40, typeof(RainBarrel))]
        [DataRow(80, typeof(RainBarrel))]
        [DataRow(120, typeof(RainBarrel))]
        [DataRow(-5, typeof(RainBarrel))]
        [DataRow(50, typeof(OilBarrel))]
        [DataRow(100, typeof(OilBarrel))]
        [DataRow(150, typeof(OilBarrel))]
        [DataRow(-5, typeof(OilBarrel))]
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
            int preContent = container.Content;
            // Fill container using amount
            container.Fill(amount);

            // If amount is negative, check if content is still equal to capacity
            if (amount <= 0)
            {
                Assert.AreEqual(preContent, container.Content, "Container has increased/decreased content when filling with negative amount");
                return;
            }

            // If expected content is lower or equal to Capacity
            if (preContent + amount <= container.Capacity)
            {
                // Check if preCap + amount is the same as current Content
                Assert.AreEqual(preContent + amount, container.Content, "preContent + amount is expected to be Content but was not equal to Content");
            }
            else
            {
                // Content should be the same as Capacity because of overflow
                Assert.AreEqual(container.Content, container.Capacity, "Content is expected to be Capacity but was not equal to Capacity");
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
            int preContent = container.Content;
            // AddContent to container using amount
            container.AddContent(amount);

            // If expected content is lower or equal to Capacity
            if (preContent + amount <= container.Capacity)
            {
                // Check if preCap + amount is the same as current Content
                Assert.AreEqual(preContent + amount, container.Content, "preContent + amount is expected to be Content but was not equal to Content");
            }
            else
            {
                // Content should be the same as Capacity because of overflow
                Assert.AreEqual(container.Capacity, container.Content, "Content is expected to be Capacity but was not equal to Capacity");
            }
        }

        [DataTestMethod]
        [TestCategory(CategoryTypes.Methods)]
        [DataRow(3, typeof(Bucket))]
        [DataRow(-1, typeof(Bucket))]
        [DataRow(10, typeof(Bucket))]
        [DataRow(40, typeof(RainBarrel))]
        [DataRow(-1, typeof(RainBarrel))]
        [DataRow(120, typeof(RainBarrel))]
        [DataRow(50, typeof(OilBarrel))]
        [DataRow(-1, typeof(OilBarrel))]
        [DataRow(150, typeof(OilBarrel))]
        public void CheckRemoveContentMethodUsingAmount(int amount, Type containerType)
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
            int preContent = container.Content;
            // RemoveContent from container using amount
            container.RemoveContent(amount);

            // If amount is negative, check if content is still equal to capacity
            if (amount <= 0)
            {
                Assert.AreEqual(preContent, container.Content, "Container has increased/decreased content when removing with negative amount");
                return;
            }

            // If expected content is higher or equal to 0
            if (preContent - amount >= 0)
            {
                // Check if preCap + amount is the same as current Content
                Assert.AreEqual(preContent - amount, container.Content, "PreContent - amount is expected to be Content but was not equal to Content");
            }
            else
            {
                // Content should be the same as 0 because the bucket was already empty before removing the rest
                Assert.AreEqual(0, container.Content, "Content is expected 0 but was not zero");
            }
        }
        #endregion

        #region Events
        [DataTestMethod]
        [TestCategory(CategoryTypes.Events)]
        [DataRow(ConTypes.Bucket)]
        [DataRow(ConTypes.RainBarrel)]
        [DataRow(ConTypes.OilBarrel)]
        public void CheckFullEvent(ConTypes containerType)
        {
            // Create new event args using args
            ContainerEventArgs e = new ContainerEventArgs { ContainerType = containerType };

            // Check if event executed successfully
            Assert.AreEqual(EventReturnString, OnFull(e));
        }

        [DataTestMethod]
        [TestCategory(CategoryTypes.Events)]
        [DataRow(true, typeof(Bucket), ConTypes.Bucket)]
        [DataRow(false, typeof(Bucket), ConTypes.Bucket)]
        [DataRow(true, typeof(RainBarrel), ConTypes.RainBarrel)]
        [DataRow(false, typeof(RainBarrel), ConTypes.RainBarrel)]
        [DataRow(true, typeof(OilBarrel), ConTypes.OilBarrel)]
        [DataRow(false, typeof(OilBarrel), ConTypes.OilBarrel)]
        public void CheckCapacityOverflowingEvent(bool debugMessageSend, Type containerType, ConTypes containerTypeEnum)
        {
            // Create a new container with the type of containerType
            Container container = Activator.CreateInstance(containerType) as Container;

            // Check if container is null so it can return that test failed
            if (container == null)
            {
                Assert.Fail("Container was null, please try again");
            }

            // Create new event args using args
            CapacityOverflowingEventArgs e = new CapacityOverflowingEventArgs { DebugMessageSend = debugMessageSend, ContainerType = containerTypeEnum };

            // Check if all possible event args are not null
            Assert.IsNotNull(e.DebugMessageSend);
            Assert.IsNotNull(e.ContainerType);

            // Check if event executed successfully
            Assert.AreEqual(EventReturnString, OnCapacityOverflowing(e));
        }

        [DataTestMethod]
        [TestCategory(CategoryTypes.Events)]
        [DataRow(ConTypes.Bucket, 10)]
        [DataRow(ConTypes.Bucket, 100)]
        [DataRow(ConTypes.RainBarrel, 10)]
        [DataRow(ConTypes.RainBarrel, 100)]
        [DataRow(ConTypes.OilBarrel, 10)]
        [DataRow(ConTypes.OilBarrel, 100)]
        public void CheckCapacityOverflowedEvent(ConTypes containerType, int lostAmount)
        {
            // Create new event args using args
            CapacityOverflowedEventArgs e = new CapacityOverflowedEventArgs { ContainerType = containerType, LostAmount = lostAmount };

            // Check if all possible event args are not null
            Assert.IsNotNull(e.LostAmount);

            // Check if event executed successfully
            Assert.AreEqual(EventReturnString, OnCapacityOverflowed(e));
        }

        #endregion
    }
}

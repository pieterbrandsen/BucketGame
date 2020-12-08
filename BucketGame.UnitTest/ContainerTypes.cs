using System;
using System.Diagnostics;
using System.Threading.Tasks;
using BucketGame.Helpers;
using BucketGame.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static BucketGame.Constants.ContainerTypes;
using static BucketGame.Constants.OilBarrel;
using static BucketGame.Constants.RainBarrel;
using static BucketGame.Constants.UnitTesting;
using static BucketGame.Models.ContainerEvents;

namespace BucketGame.UnitTest
{
    public static class ContainerTypesHelper
    {
        public static bool CheckCapacityAndContentWithExpected(int capacity, int content, int preContent, int amount, string method = "fill")
        {
            switch (method)
            {
                case "fill":
                    // If expected content is lower or equal to Capacity
                    if (preContent + amount <= capacity && amount > 0)
                    {
                        // Check if preCap + amount is the same as current Content
                        return preContent + amount == content;
                    }

                    // If amount is negative, nothing should have happened so preContent == content
                    if (amount <= 0)
                    {
                        return content == preContent;
                    }

                    // Content should be the same as Capacity because of overflow
                    return content == capacity;
                case "empty":

                    // If expected content is lower or equal to Capacity
                    if (preContent - amount > 0 && amount > 0)
                    {
                        // Check if preCap + amount is the same as current Content
                        return preContent - amount == content;
                    }

                    // If amount is negative, nothing should have happened so preContent == content
                    if (amount <= 0)
                    {
                        return content == preContent;
                    }

                    // Content should be the same as Capacity because of overflow
                    return content == 0;
                default:
                    break;
            }

            // If expected content is lower or equal to Capacity
            if (amount <= capacity && amount > 0)
            {
                // Check if preCap + amount is the same as current Content
                return preContent == content;
            }

            // If amount is negative, nothing should have happened so preContent == content
            if (amount <= 0)
            {
                return content == preContent;
            }

            // Content should be the same as Capacity because of overflow
            return content == capacity;
        }
    }

    [TestClass]
    public class ContainerTypes : Container
    {
        #region Constructors
        [DataTestMethod]
        [TestCategory(CategoryTypes.Constructor)]
        [DynamicData(nameof(DataGenerator.GetRandom_Number_Number_Types), typeof(DataGenerator), DynamicDataSourceType.Method)]
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

            int preContent = container.Content;

            // Check if Content and Capacity of container is as expected
            Assert.IsTrue(ContainerTypesHelper.CheckCapacityAndContentWithExpected(container.Capacity, container.Content, preContent, content ?? 0, "none"));
        }
        #endregion

        #region Properties
        [DataTestMethod]
        [TestCategory(CategoryTypes.Properties)]
        [DynamicData(nameof(DataGenerator.GetRandomTypes), typeof(DataGenerator), DynamicDataSourceType.Method)]
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
        [DynamicData(nameof(DataGenerator.GetRandom_Types_Number_Types_Number), typeof(DataGenerator), DynamicDataSourceType.Method)]
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

            Assert.IsNotNull(container);
            Assert.IsNotNull(targetContainer);
        }

        [DataTestMethod]
        [TestCategory(CategoryTypes.Methods)]
        [DynamicData(nameof(DataGenerator.GetRandom_Number_Types), typeof(DataGenerator), DynamicDataSourceType.Method)]
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

            // Check if Content and Capacity of container is as expected
            Assert.IsTrue(ContainerTypesHelper.CheckCapacityAndContentWithExpected(container.Capacity, container.Content, preContent, amount));
        }

        [DataTestMethod]
        [TestCategory(CategoryTypes.Methods)]
        [DynamicData(nameof(DataGenerator.GetRandom_Number_Types), typeof(DataGenerator), DynamicDataSourceType.Method)]
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

            // Check if Content and Capacity of container is as expected
            Assert.IsTrue(ContainerTypesHelper.CheckCapacityAndContentWithExpected(container.Capacity, container.Content, preContent, amount));
        }

        [DataTestMethod]
        [TestCategory(CategoryTypes.Methods)]
        [DynamicData(nameof(DataGenerator.GetRandom_Number_Types), typeof(DataGenerator), DynamicDataSourceType.Method)]
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

        [DataTestMethod]
        [TestCategory(CategoryTypes.Methods)]
        [DynamicData(nameof(DataGenerator.GetRandomTypes), typeof(DataGenerator), DynamicDataSourceType.Method)]
        public void CheckEmptyMethod(Type containerType)
        {
            // Create a new container with the type of containerType
            Container container = Activator.CreateInstance(containerType) as Container;

            // Check if container is null so it can return that test failed
            if (container == null)
            {
                Assert.Fail("Container was null, please try again");
            }

            // Empty container
            container.Empty();

            // Check after draining if Content == 0
            Assert.AreEqual(0, container.Content);
        }

        [DataTestMethod]
        [TestCategory(CategoryTypes.Methods)]
        [DynamicData(nameof(DataGenerator.GetRandom_Number_Types), typeof(DataGenerator), DynamicDataSourceType.Method)]
        public void CheckEmptyMethodUsingAmount(int amount, Type containerType)
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

            // Empty container
            int preContent = container.Content;
            container.Empty(amount);

            // Check after draining if Content == 0
            Assert.IsTrue(ContainerTypesHelper.CheckCapacityAndContentWithExpected(container.Capacity, container.Content, preContent, amount, "empty"));
        }
        #endregion

        #region Events
        [DataTestMethod]
        [TestCategory(CategoryTypes.Events)]
        [DynamicData(nameof(DataGenerator.GetRandom_ConTypes), typeof(DataGenerator), DynamicDataSourceType.Method)]
        public void CheckFullEvent(ConTypes containerType)
        {
            // Create new event args using args
            ContainerEventArgs e = new ContainerEventArgs { ContainerType = containerType };

            // Check if event executed successfully
            Assert.AreEqual(EventReturnString, OnFull(e));
        }

        [DataTestMethod]
        [TestCategory(CategoryTypes.Events)]
        [DynamicData(nameof(DataGenerator.GetRandom_Type_ConTypes), typeof(DataGenerator), DynamicDataSourceType.Method)]
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
        [DynamicData(nameof(DataGenerator.GetRandom_ConTypes_Number), typeof(DataGenerator), DynamicDataSourceType.Method)]
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

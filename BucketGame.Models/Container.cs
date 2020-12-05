using System;
using System.Diagnostics;
using static BucketGame.Constants.Bucket;
using static BucketGame.Constants.ContainerTypes;
using static BucketGame.Constants.OilBarrel;
using static BucketGame.Constants.RainBarrel;
using static BucketGame.Models.ContainerEvents;

namespace BucketGame.Models
{
    public abstract class Container
    {
        #region Constructors
        protected Container()
        {
            // Initialize Bucket
            AddEvents(this);
            Capacity = BucketDefaultCap;
            Content = 0;
        }

        protected Container(int content)
        {
            // Initialize Bucket
            AddEvents(this);
            Capacity = BucketDefaultCap;
            AddContent(content);
        }

        protected Container(int content, int capacity)
        {
            // Initialize Container
            AddEvents(this);
            Capacity = capacity;
            AddContent(content);
        }

        #endregion

        #region Properties
        public int Content { get => content; set => OverwriteContent(value); }
        private int content { get; set; }
        public int Capacity { get => capacity; set => OverwriteCapacity(value); }
        private int capacity { get; set; }
        #endregion

        #region Methods
        #region Content related
        public void AddContent(int newContent)
        {
            // If inputted content is lower then zero, return
            if (newContent < 0)
            {
                return;
            }

            // Bucket has not yet overflowed
            bool bucketIsOverflowing = false;
            int overflowedAmount = 0;
            while (newContent > 0)
            {
                content++; newContent--;

                // Check if Content is at Capacity, call OnFull event
                // Else if Content is higher then Capacity, call OnCapacityOverflowing event
                if (Content == Capacity)
                {
                    OnFull(new ContainerEventArgs { ContainerType = Enum.Parse<ConTypes>(GetType().Name) });
                }
                else if (Content > Capacity)
                {
                    OnCapacityOverflowing(new CapacityOverflowingEventArgs { DebugMessageSend = bucketIsOverflowing, ContainerType = Enum.Parse<ConTypes>(GetType().Name) });
                    overflowedAmount += 1; bucketIsOverflowing = true;
                }
            }

            if (bucketIsOverflowing)
            {
                // Finish overflowing
                OnCapacityOverflowed(new CapacityOverflowedEventArgs { LostAmount = overflowedAmount, ContainerType = Enum.Parse<ConTypes>(GetType().Name) });
            }
            else if (Content > Capacity * 0.9 && Content < Capacity)
            {
                Debug.WriteLine($"A {Enum.Parse<ConTypes>(GetType().Name)} is almost at capacity because its {Content * 100 / Capacity * 100 / 100}% full, there is {Capacity - Content} capacity free");
            }
        }

        public void AddContent(Container container)
        {
            // Loop until the other bucket is empty or this bucket is full
            while (container.Content > 0 && Content < Capacity)
            {
                AddContent(1);
                container.RemoveContent(1);
            }

            if (container.Content > 0 && Content >= Capacity)
            {
                Debug.WriteLine("Stopped filling bucket because it would have overflowed otherwise");
            }
            else if (Content > Capacity * 0.9 && Content < Capacity)
            {
                Debug.WriteLine($"A {Enum.Parse<ConTypes>(GetType().Name)} is almost at capacity because its {Content / Capacity * 100}% full, there is {Capacity - Content} capacity free");
            }
        }

        private void OverwriteContent(int newContent)
        {
            // Reset content back to zero and add new value afterwards
            content = 0;
            AddContent(newContent);
        }

        public void RemoveContent(int removeContent)
        {
            // If inputted content is lower then zero
            if (removeContent < 0)
            {
                // Please don't use negative values in AddContent
                return;
            }

            // While inputted content and content is higher then zero, remove content
            while (removeContent > 0 && content > 0)
            {
                // While newContent has values to add to content, continue
                content--;
                removeContent--;
            }
        }

        public void Fill(int amount)
        {
            // Add amount to bucket
            AddContent(amount);
        }

        public void Fill(Container container)
        {
            // If this is not a bucket but the other container is a bucket
            if (!(container is Bucket))
            {
                Debug.WriteLine($"Cant fill a {GetType().Name} using not a bucket");
                return;
            }

            // Fill this bucket using another bucket
            AddContent(container);
        }
        #endregion
        #region Capacity
        private void OverwriteCapacity(int newCapacity)
        {
            // Check if Container is wrong size
            // If this is the case, throw a ArgumentOutOfRangeException
            bool isBucketWrongSize = this is Bucket && (newCapacity < BucketMinCap || newCapacity > BucketMaxCap);
            bool isRainBarrelWrongSize = this is RainBarrel && newCapacity != RainBarrelSmall && newCapacity != RainBarrelMedium && newCapacity != RainBarrelLarge;
            bool isOilBarrelWrongSize = this is OilBarrel && newCapacity != OilBarrelCap;
            if (isBucketWrongSize)
            {
                newCapacity = BucketDefaultCap;
            }
            else if (isRainBarrelWrongSize)
            {
                newCapacity = RainBarrelMedium;
            }
            else if (isOilBarrelWrongSize)
            {
                newCapacity = OilBarrelCap;
            }

            capacity = newCapacity;
        }
        #endregion
        #endregion

        #region ContainerEvents

        public event EventHandler<ContainerEventArgs> Full;

        protected string OnFull(ContainerEventArgs e)
        {
            Full?.Invoke(this, e);
            return EventReturnString;
        }

        public event EventHandler<CapacityOverflowingEventArgs> CapacityOverflowing;

        protected string OnCapacityOverflowing(CapacityOverflowingEventArgs e)
        {
            CapacityOverflowing?.Invoke(this, e);
            return EventReturnString;
        }

        public event EventHandler<CapacityOverflowedEventArgs> CapacityOverflowed;

        protected string OnCapacityOverflowed(CapacityOverflowedEventArgs e)
        {
            CapacityOverflowed?.Invoke(this, e);
            return EventReturnString;
        }

        #endregion
    }

    public class ContainerEvents
    {
        public static void AddEvents(Container container)
        {
            container.Full += Full;
            container.CapacityOverflowing += CapacityOverflowing;
            container.CapacityOverflowed += CapacityOverflowed;
        }

        public class ContainerEventArgs : EventArgs
        {
            public ConTypes ContainerType { get; set; }
        }

        public class CapacityOverflowingEventArgs : ContainerEventArgs
        {
            public bool DebugMessageSend { get; set; }
        }

        public class CapacityOverflowedEventArgs : ContainerEventArgs
        {
            public int LostAmount { get; set; }
        }

        public static void Full(object sender, ContainerEventArgs e)
        {
            Debug.WriteLine($"A {e.ContainerType} is full, please empty that bucket before it overflows");
        }

        public static void CapacityOverflowing(object sender, CapacityOverflowingEventArgs e)
        {
            Container container = sender as Container;
            container?.RemoveContent(1);

            // Notify that bucket is overflowing
            if (!e.DebugMessageSend)
            {
                Debug.WriteLine($"A {e.ContainerType} is overflowing");
            }
        }

        public static void CapacityOverflowed(object sender, CapacityOverflowedEventArgs e)
        {
            Debug.WriteLine($"A {e.ContainerType} has overflowed and lost {e.LostAmount} content");
        }
    }
}

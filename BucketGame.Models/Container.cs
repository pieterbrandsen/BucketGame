using System;
using System.Diagnostics;
using BucketGame.Constants;
using static BucketGame.Constants.Bucket;
using static BucketGame.Constants.OilBarrel;
using static BucketGame.Constants.RainBarrel;
using static BucketGame.Models.ContainerEvents;

namespace BucketGame.Models
{
    public abstract class Container
    {
        #region Constructors
        /*
        public Container()
        {
            // Empty constructor
        }

        public Container(int content)
        {
            // Initialize Bucket
            Capacity = BucketDefaultCap;
            AddContent(content);
        }

        public Container(int content, int capacity)
        {
            Capacity = capacity;
            AddContent(content);
        }
        */

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
            // If inputted content is lower then zero
            if (newContent < 0)
            {
                // Please don't use negative values in AddContent
                throw new ArgumentOutOfRangeException();
            }

            // Bucket has not yet overflowed
            bool bucketIsOverflowing = false;
            int overflowedAmount = 0;
            while (newContent > 0)
            {
                // While newContent has values to add to content, continue
                content++;
                newContent--;

                // If this bucket is full
                if (Content == Capacity)
                {
                    OnFull(new ContainerEventArgs() { ContainerType = Enum.Parse<ContainerTypes>(GetType().Name) });
                }

                // If Content is higher then Capacity
                else if (Content > Capacity)
                {
                    // Start capacity overflowing
                    overflowedAmount += 1;
                    OnCapacityOverflowing(new CapacityOverflowingEventArgs() { DebugMessageSend = bucketIsOverflowing, ContainerType = Enum.Parse<ContainerTypes>(GetType().Name) });
                    bucketIsOverflowing = true;
                }
            }

            if (bucketIsOverflowing)
            {
                // Finish overflowing
                OnCapacityOverflowed(new CapacityOverflowedEventArgs() { LostAmount = overflowedAmount, ContainerType = Enum.Parse<ContainerTypes>(GetType().Name) });
            }
        }

        public void AddContent(Container bucket)
        {
            // Loop until the other bucket is empty or this bucket is full
            while (bucket.Content > 0 && Content < Capacity)
            {
                AddContent(1);
                bucket.RemoveContent(1);
            }

            if (bucket.Content > 0 && Content >= Capacity)
            {
                OnFull(new ContainerEventArgs() { ContainerType = Enum.Parse<ContainerTypes>(GetType().Name) });
                Debug.WriteLine("Stopped filling bucket because it would have overflowed otherwise");
            }
        }

        public void RemoveContent(int removeContent) => content -= removeContent;
        private void OverwriteContent(int newContent)
        {
            // Reset content back to zero and add new value afterwards
            content = 0;
            AddContent(newContent);
        }

        public void Fill(int amount)
        {
            // Add amount to bucket
            AddContent(amount);
        }

        public void Fill(Container container)
        {
            // If this is not a bucket but the other container is a bucket
            if (!(this is Bucket) && container is Bucket)
            {
                Debug.WriteLine("Cant fill a bucket with another type of container then a bucket");
                return;
            }

            // Fill this bucket using another bucket
            AddContent(container.Content);
            // Afterwards remove old content
            container.RemoveContent(container.Content);
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
            if (isBucketWrongSize || isRainBarrelWrongSize || isOilBarrelWrongSize)
            {
                // Capacity is not right size, please try again.
                throw new ArgumentOutOfRangeException();
            }

            capacity = newCapacity;
        }
        #endregion
        #endregion

        #region ContainerEvents

        public event EventHandler<ContainerEventArgs> Full;

        protected void OnFull(ContainerEventArgs e)
        {
            Full?.Invoke(this, e);
        }

        public event EventHandler<CapacityOverflowingEventArgs> CapacityOverflowing;

        protected void OnCapacityOverflowing(CapacityOverflowingEventArgs e)
        {
            CapacityOverflowing?.Invoke(this, e);
        }

        public event EventHandler<CapacityOverflowedEventArgs> CapacityOverflowed;

        protected void OnCapacityOverflowed(CapacityOverflowedEventArgs e)
        {
            CapacityOverflowed?.Invoke(this, e);
        }

        #endregion
    }

    public class ContainerEvents
    {
        public class ContainerEventArgs : EventArgs
        {
            public ContainerTypes ContainerType { get; set; }
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
            try
            {
                Container container = sender as Container;
                container?.RemoveContent(1);
                // Notify that bucket is overflowing
                if (!e.DebugMessageSend)
                {
                    Debug.WriteLine($"A {e.ContainerType} is overflowing");
                }
            }
            catch (NullReferenceException exception)
            {
                Debug.WriteLine(exception);
                throw;
            }
        }

        public static void CapacityOverflowed(object sender, CapacityOverflowedEventArgs e)
        {
            Debug.WriteLine($"A {e.ContainerType} overflowed and lost {e.LostAmount} content");
        }
    }
}

namespace BucketGame.Models
{
    using System;
    using System.Diagnostics;
    using static BucketGame.Constants.Bucket;
    using static BucketGame.Constants.RainBarrel;
    using static BucketGame.Constants.OilBarrel;
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
                // If bucket has not yet started overflowing and content is higher then Capacity
                if (!bucketIsOverflowing && Content > Capacity)
                {
                    // Start capacity overflowing
                    overflowedAmount += OnCapacityOverflowing(EventArgs.Empty);
                    bucketIsOverflowing = true;
                }
            }

            if (bucketIsOverflowing)
            {
                // Finish overflowing
                OnCapacityOverflowed(EventArgs.Empty, overflowedAmount);
            }
        }

        public void AddContent(Container bucket)
        {
            while (bucket.Content > 0 && Content < Capacity)
            {
                AddContent(1);
                bucket.RemoveContent(1);
            }

            Debug.WriteLineIf(bucket.Content > 0 && Content >= Capacity, "Stopped filling bucket because it would have overflowed otherwise");
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
            // Fill this bucket using another bucket
            AddContent(container.Content);
            // Afterwards remove old content
            container.RemoveContent(container.Content);
        }
        #endregion
        #region Capacity
        private void OverwriteCapacity(int capacity)
        {
            // If capacity is higher then minCap and lower then maxCap
            if (capacity >= BucketMinCap && capacity <= BucketMaxCap)
            {
                Capacity = capacity;
            }
            else
            {
                // Capacity inputted was higher or lower then allowed
                throw new ArgumentOutOfRangeException();
            }
        }
        #endregion
        #endregion

        #region Events

        public event EventHandler Full;

        protected void OnFull(EventArgs e)
        {
            // Create new event handler
            EventHandler handler = Full;
            if (handler != null)
            {
                Debug.WriteLine($"A bucket is full, please empty that bucket before it overflows");
                handler(this, e);
            }
        }

        public event EventHandler CapacityOverflowing;

        protected int OnCapacityOverflowing(EventArgs e)
        {
            // Create new event handler
            EventHandler handler = CapacityOverflowing;

            if (handler != null)
            {
                RemoveContent(1);
                Debug.WriteLine($"A bucket is overflowing");
                handler(this, e);
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public event EventHandler CapacityOverflowed;

        protected void OnCapacityOverflowed(EventArgs e, int lostContent)
        {
            // Create new event handler
            EventHandler handler = CapacityOverflowed;

            if (handler != null)
            {
                Debug.WriteLine($"A bucket overflowed and lost {lostContent} from a bucket");
                handler(this, e);
            }
        }

        #endregion
    }
}

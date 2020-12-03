namespace BucketGame.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using static BucketGame.Constants.Bucket;
    using static BucketGame.Constants.OilBarrel;
    using static BucketGame.Constants.RainBarrel;
    public class Bucket : Container
    {
        public Bucket()
        {
            // Empty constructor
        }

        public Bucket(int content)
        {
            // Initialize Bucket
            Capacity = BucketDefaultCap;
            AddContent(content);
        }

        public Bucket(int content, int capacity)
        {
            Capacity = capacity;
            AddContent(content);
        }
    }

    public class RainBarrel : Container
    {
        public RainBarrel()
        {
            // Empty constructor
        }

        public RainBarrel(int content)
        {
            // Initialize Bucket
            Capacity = RainBarrelMedium;
            AddContent(content);
        }

        public RainBarrel(int content, int capacity)
        {
            Capacity = capacity;
            AddContent(content);
        }
    }

    public class OilBarrel : Container
    {
        public OilBarrel()
        {
            // Empty constructor
        }

        public OilBarrel(int content)
        {
            // Initialize Bucket
            Capacity = OilBarrelCap;
            AddContent(content);
        }

        public OilBarrel(int content, int capacity)
        {
            Capacity = capacity;
            AddContent(content);
        }
    }
}

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
            // Initialize Bucket
            ContainerEvents.AddEvents(this);
            Capacity = BucketDefaultCap;
            Content = 0;
        }

        public Bucket(int content)
        {
            // Initialize Bucket
            ContainerEvents.AddEvents(this);
            Capacity = BucketDefaultCap;
            AddContent(content);
        }

        public Bucket(int content, int capacity)
        {
            // Initialize Bucket
            ContainerEvents.AddEvents(this);
            Capacity = capacity;
            AddContent(content);
        }
    }

    public class RainBarrel : Container
    {
        public RainBarrel()
        {
            // Initialize RainBarrel
            ContainerEvents.AddEvents(this);
            Capacity = RainBarrelMedium;
            Content = 0;
        }

        public RainBarrel(int content)
        {
            // Initialize Bucket
            ContainerEvents.AddEvents(this);
            Capacity = RainBarrelMedium;
            AddContent(content);
        }

        public RainBarrel(int content, int capacity)
        {
            // Initialize Bucket
            ContainerEvents.AddEvents(this);
            Capacity = capacity;
            AddContent(content);
        }
    }

    public class OilBarrel : Container
    {
        public OilBarrel()
        {
            // Initialize OilBarrel
            ContainerEvents.AddEvents(this);
            Capacity = OilBarrelCap;
            Content = 0;
        }

        public OilBarrel(int content)
        {
            // Initialize Bucket
            ContainerEvents.AddEvents(this);
            Capacity = OilBarrelCap;
            AddContent(content);
        }

        public OilBarrel(int content, int capacity)
        {
            // Initialize Bucket
            ContainerEvents.AddEvents(this);
            Capacity = capacity;
            AddContent(content);
        }
    }
}

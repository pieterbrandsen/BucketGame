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
        }

        public Bucket(int content) : base(content)
        {
        }

        public Bucket(int content, int capacity) : base(content, capacity)
        {
        }
    }

    public class RainBarrel : Container
    {
        public RainBarrel()
        {
        }

        public RainBarrel(int content) : base(content)
        {
        }

        public RainBarrel(int content, int capacity) : base(content, capacity)
        {
        }
    }

    public class OilBarrel : Container
    {
        public OilBarrel()
        {
        }

        public OilBarrel(int content) : base(content)
        {
        }

        public OilBarrel(int content, int capacity) : base(content, capacity)
        {
        }
    }
}

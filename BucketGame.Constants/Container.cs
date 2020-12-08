using System;
using System.Collections.Generic;

namespace BucketGame.Constants
{
    public static class Bucket
    {
        public const int BucketMinCap = 10;
        public const int BucketDefaultCap = 12;
        public const int BucketMaxCap = 15;
    }

    public static class RainBarrel
    {
        public const int RainBarrelSmall = 80;
        public const int RainBarrelMedium = 120;
        public const int RainBarrelLarge = 160;
    }

    public static class OilBarrel
    {
        public const int OilBarrelCap = 159;
    }

    public static class ContainerTypes
    {
        public enum ConTypes
        {
            Bucket,
            RainBarrel,
            OilBarrel
        }

        public static string EventReturnString = "Executed Event";
    }

    public static class UnitTesting
    {
        public static class CategoryTypes
        {
            public const string Constructor = "Constructor";
            public const string Properties = "Properties";
            public const string Methods = "Methods";
            public const string Events = "Events";
        }

        public static class Data
        {
            public const int DataCount = 10*1000;
            public const int MinNumber = -100;
            public const int MaxNumber = 500;
        }
    }
}
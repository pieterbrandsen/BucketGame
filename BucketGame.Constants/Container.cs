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

    public enum ContainerTypes
    {
        Bucket,
        RainBarrel,
        OilBarrel
    }
}
namespace BucketGame.Core
{
    using System;
    using Models;
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            OilBarrel container = new OilBarrel(12);
            Bucket container2 = new Bucket(11, 12);
            container2.Full += ContainerEvents.Full;
            container2.CapacityOverflowing += ContainerEvents.CapacityOverflowing;
            container2.CapacityOverflowed += ContainerEvents.CapacityOverflowed;
            container2.Fill(container);

            Console.ReadLine();
        }
    }
}

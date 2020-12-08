using System;
using System.Collections.Generic;
using BucketGame.Models;
using static BucketGame.Constants.UnitTesting.Data;
using static BucketGame.Constants.ContainerTypes;

namespace BucketGame.Helpers
{
    public static class DataGenerator
    {
        private static readonly Random rnd;
        private static List<object[]> list;
        private static readonly Type[] types = { typeof(Bucket), typeof(RainBarrel), typeof(OilBarrel) };
        private static readonly ConTypes[] conTypes = { ConTypes.Bucket, ConTypes.RainBarrel, ConTypes.OilBarrel };
        static DataGenerator()
        {
            rnd = new Random();
        }

        private static void RunLoop(params object[] parameters)
        {
            list = new List<object[]>();
            for (int i = DataCount; i > 0; i--)
            {
                list.Add(parameters);
            }
        }

        public static IEnumerable<object[]> GetRandom_Types_Number_Types_Number()
        {
            // Fill list
            RunLoop(types[rnd.Next(types.Length)], rnd.Next(MinNumber, MaxNumber), types[rnd.Next(types.Length)], rnd.Next(MinNumber, MaxNumber));

            // Return list
            return list;
        }

        public static IEnumerable<object[]> GetRandomTypes()
        {
            // Fill list
            RunLoop(types[rnd.Next(types.Length)]);

            // Return list
            return list;
        }

        public static IEnumerable<object[]> GetRandom_ConTypes()
        {
            // Fill list
            RunLoop(conTypes[rnd.Next(conTypes.Length)]);

            // Return list
            return list;
        }

        public static IEnumerable<object[]> GetRandom_ConTypes_Number()
        {
            // Fill list
            RunLoop(conTypes[rnd.Next(conTypes.Length)], rnd.Next(MinNumber, MaxNumber));

            // Return list
            return list;
        }
        public static IEnumerable<object[]> GetRandom_Type_ConTypes()
        {
            // Fill list
            RunLoop(false, types[rnd.Next(types.Length)], conTypes[rnd.Next(conTypes.Length)]);
            RunLoop(true, types[rnd.Next(types.Length)], conTypes[rnd.Next(conTypes.Length)]);

            // Return list
            return list;
        }

        public static IEnumerable<object[]> GetRandom_Number_Types()
        {
            // Fill list
            RunLoop(rnd.Next(MinNumber, MaxNumber), types[rnd.Next(types.Length)]);

            // Return list
            return list;
        }

        public static IEnumerable<object[]> GetRandom_Number_Number_Types()
        {
            // Fill list
            RunLoop(rnd.Next(MinNumber, MaxNumber), rnd.Next(MinNumber, MaxNumber), types[rnd.Next(types.Length)]);
            RunLoop(rnd.Next(MinNumber, MaxNumber), null, types[rnd.Next(types.Length)]);
            RunLoop(null, null, types[rnd.Next(types.Length)]);

            // Return list
            return list;
        }
    }
}

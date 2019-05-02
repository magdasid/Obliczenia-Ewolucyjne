using System;

namespace TSP
{
    public static class Extensions
    {
        public static bool Contains(this int[] array, int value)
        {
            return array.IndexOf(value) != -1;
        }

        public static int IndexOf(this int[] array, int value)
        {
            int index = -1;
            for (int i = 0; i < array.Length; i++)
                if (array[i] == value) return i;
            return index;
        }

        public static int[] Shuffle(this int[] array)
        {
            var random = RandomGenerator.GetInstance();
            int[] newArray = new int[array.Length];
            Array.Copy(array, newArray, array.Length);

            for (int i = array.Length; i > 1; i--)
            {
                int j = random.Next(i);
                int tmp = newArray[j];
                newArray[j] = newArray[i - 1];
                newArray[i - 1] = tmp;
            }
            return newArray;
        }

        public static double Min(this double[] array)
        {
            double bestResult = array[0];
            for (int i = 1; i < array.Length; i++)
                if (array[i] < bestResult)
                    bestResult = array[i];

            return bestResult;
        }
    }
}

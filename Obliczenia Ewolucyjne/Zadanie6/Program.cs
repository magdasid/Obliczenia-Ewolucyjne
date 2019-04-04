using System;

namespace Zadanie6
{
    public class Individual
    {
        public int[] genotype;
        public int[] tourList;
        //public double tourLength;
        
        public Individual(int[] g)
        {
            genotype = g;
            tourList = TourList(g, 9);
        }

        public int[] TourList(int[] genotype, int numberOfCities)
        {
            int[] freeList = new int[numberOfCities];
            int[] tourList = new int[numberOfCities];

            for (int i = 0; i < numberOfCities; i++)
            {
                freeList[i] = i + 1;
            }

            for (int i = 0; i < genotype.Length; i++)
            {
                int element = genotype[i] - 1;

                Console.WriteLine("el" + element);
                tourList[i] = FindCity(freeList, element);

                Console.WriteLine("tour:" + tourList[i]);
            }
            
            return tourList;
        }

        public int FindCity(int[] freeList, int element)
        {
            int value = 0;
            int indexOfFreeElements = 0;

            for (int i = 0; i < freeList.Length; i++)
            {
                if(freeList[i] != -1)
                {
                    if(element == indexOfFreeElements)
                    {
                        value = freeList[i];
                        freeList[i] = -1;
                        break;
                    } else
                    {
                        indexOfFreeElements++;
                    }
                }
            }
            return value;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            int[] tablic = new int[9] { 1, 1, 2, 1, 4, 1, 3, 1, 1 };
            Individual ind1 = new Individual(tablic);

            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}

using System;
using TSP.Interfaces;

namespace TSP.Crossovers
{
    public class PMX : ICrossover, ICrossoverUsingPathRepresentation
    {
        private static Random Random = new Random();

        public Individual Cross(Individual[] parents, Cities cities)
        {
            int size = parents[0].tourList.Length;
            int splitPoint1 = Random.Next(1, size);
            int splitPoint2 = Random.Next(splitPoint1 + 1, size);

            Individual child = null;

            int[] childTourList = new int[size];
            int[] tab = new int[size];

            for (int i = 0; i < childTourList.Length; i++)
            {
                if (i < splitPoint1)
                {
                    tab[i] = -1;
                    childTourList[i] = -1;
                }
                else if (i >= splitPoint1 && i < splitPoint2)
                {
                    tab[i] = parents[0].tourList[i];
                    childTourList[i] = parents[0].tourList[i];
                }
            }

            for (int i = 0; i < childTourList.Length; i++)
            {
                if (i < splitPoint1 || i >= splitPoint2)
                {
                    if (Array.IndexOf(tab, parents[1].tourList[i]) == -1)
                    {
                        childTourList[i] = parents[1].tourList[i];
                        tab[i] = parents[1].tourList[i];
                    }
                    else
                    {
                        int element = parents[1].tourList[i];
                        int elementIndex = Array.IndexOf(parents[0].tourList, element);
                        int val = parents[1].tourList[elementIndex];
                        while (Array.IndexOf(childTourList, val) != -1)
                        {
                            element = val;
                            elementIndex = Array.IndexOf(parents[0].tourList, element);
                            val = parents[1].tourList[elementIndex];
                        }
                        childTourList[i] = val;
                    }
                }
            }
            
            child = new Individual(cities, childTourList);

            return child;
        }

        public override string ToString()
        {
            return "Partially Mapped Crossover - krzyżowanie z częściowym odwzorowaniem";
        }
        
    }
}

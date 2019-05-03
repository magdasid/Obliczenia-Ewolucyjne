using System;
using TSP.Interfaces;

namespace TSP.Crossovers
{
    public class PMX : ICrossover, ICrossoverUsingPathRepresentation
    {
        private static Random Random = RandomGenerator.GetInstance();

        public Individual Cross(Individual[] parents, Cities cities)
        {
            int size = parents[0].tourList.Length;
            int splitPoint1 = Random.Next(1, size);
            int splitPoint2 = Random.Next(splitPoint1 + 1, size);

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
                    if (tab.IndexOf(parents[1].tourList[i]) == -1)
                    {
                        childTourList[i] = parents[1].tourList[i];
                        tab[i] = parents[1].tourList[i];
                    }
                    else
                    {
                        int element = parents[1].tourList[i];
                        int elementIndex = parents[0].tourList.IndexOf(element);
                        int val = parents[1].tourList[elementIndex];
                        while (childTourList.IndexOf(val) != -1)
                        {
                            element = val;
                            elementIndex = parents[0].tourList.IndexOf(element);
                            val = parents[1].tourList[elementIndex];
                        }
                        childTourList[i] = val;
                    }
                }
            }

            return new Individual(cities, childTourList);
        }

        public override string ToString() => "Partially Mapped Crossover - krzyżowanie z częściowym odwzorowaniem";     
    }
}

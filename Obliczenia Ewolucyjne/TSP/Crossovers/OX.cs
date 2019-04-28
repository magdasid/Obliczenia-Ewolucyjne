using System;
using TSP.Interfaces;

namespace TSP.Crossovers
{
    public class OX : ICrossover, ICrossoverUsingPathRepresentation
    {
        private static Random Random = new Random();

        public Individual Cross(Individual[] parents, Cities cities)
        {
            int size = parents[0].tourList.Length; 
            int splitPoint1 = Random.Next(1, size);
            int splitPoint2 = Random.Next(splitPoint1, size); 

            Individual child = null;

            int[] childTourList = new int[size];
            int[] tab = new int[size];

            // wyjmujemy środek z 1 rodzica pomiędzy wylosowanymi punktami
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

            int index = splitPoint2;
            // uzupełniamy ogon 
            for (int i = splitPoint2; i < childTourList.Length; i++)
            {
                int element = parents[1].tourList[index];

                while (Array.IndexOf(tab, element) != -1)
                {
                    if (index == size - 1)
                    {
                        index = 0;
                    }
                    else
                    {
                        index++;
                    }
                    element = parents[1].tourList[index];

                }
                childTourList[i] = element;
                tab[i] = element;
            }

            //uzupełniam początek listy
            for (int i = 0; i < splitPoint1; i++)
            {
                int element = parents[1].tourList[index];

                while (Array.IndexOf(tab, element) != -1)
                {
                    if (index == size - 1)
                    {
                        index = 0;
                    }
                    else
                    {
                        index++;
                    }
                    element = parents[1].tourList[index];

                }
                childTourList[i] = element;
                tab[i] = element;
            }
            
            child = new Individual(cities, childTourList);

            return child;
        }

        public override string ToString()
        {
            return "Order Crossover - Krzyżowanie z porządkowaniem";
        }
    }
}

using System;
using TSP.Interfaces;

namespace TSP.Crossovers
{
    public class CX : ICrossover, ICrossoverUsingPathRepresentation
    {
        public Individual Cross(Individual[] parents, Cities cities)
        {
            int[] parent = parents[0].tourList;
            int[] parent2 = parents[1].tourList;

            int size = parents[0].tourList.Length;
            Individual child = null;

            int[] childTourList = new int[size];
            int index = 0;
            int element = parent[index];

            Console.WriteLine("element" + element);

            // wprowadzamy to co możemy z 1 rodzica, dopóki nie zaczną się powtarzać miasta
            while (Array.IndexOf(childTourList, element) == -1)
            {
                childTourList[index] = element;
                index = Array.IndexOf(parent, parent2[index]);

                element = parent[index];
            }

            // dopisujemy resztę z rodzica nr 2
            for (int i = 0; i < childTourList.Length; i++)
            {
                if (childTourList[i] == 0)
                {
                    childTourList[i] = parent2[i];
                }
            }

            for (int i = 0; i < childTourList.Length; i++)
            {
                Console.WriteLine("childTour: " + childTourList[i]);
            }

            child = new Individual(cities, childTourList);

            return child;
        }

        public override string ToString()
        {
            return "Cyclic Crossover - Krzyżowanie cykliczne";
        }
    }
}

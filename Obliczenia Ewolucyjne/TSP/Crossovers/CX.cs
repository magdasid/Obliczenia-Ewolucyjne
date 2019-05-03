using System;
using TSP.Interfaces;

namespace TSP.Crossovers
{
    public class CX : ICrossover, ICrossoverUsingPathRepresentation
    {
        public Individual Cross(Individual[] parents, Cities cities)
        {
            int size = parents[0].tourList.Length;
            int[] parent1 = parents[0].tourList;
            int[] parent2 = parents[1].tourList;
            int[] childTourList = new int[size];
            bool[] used = new bool[size + 1];

            int[] parent1Mapping = new int[size + 1];
            for (int i = 0; i < parent1.Length; i++)
                parent1Mapping[parent1[i]] = i;
     
            int index = 0;
            int element = parent1[index];
            // wprowadzamy to co możemy z 1 rodzica, dopóki nie zaczną się powtarzać miasta
            while (!used[element])
            {
                childTourList[index] = element;
                used[element] = true;
                index = parent1Mapping[parent2[index]];
                element = parent1[index];
            }
            // dopisujemy resztę z rodzica nr 2
            for (int i = 0; i < childTourList.Length; i++)
                if (childTourList[i] == 0)
                    childTourList[i] = parent2[i];

            return new Individual(cities, childTourList);
        }

        public override string ToString() => "Cyclic Crossover - Krzyżowanie cykliczne";
    }
}

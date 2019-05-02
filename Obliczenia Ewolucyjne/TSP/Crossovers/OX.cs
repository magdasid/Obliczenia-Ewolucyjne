using System;
using TSP.Interfaces;

namespace TSP.Crossovers
{
    public class OX : ICrossover, ICrossoverUsingPathRepresentation
    {
        private static Random Random = RandomGenerator.GetInstance();

        public Individual Cross(Individual[] parents, Cities cities)
        {
            int size = parents[0].tourList.Length; 
            int splitPoint1 = Random.Next(1, size);
            int splitPoint2 = Random.Next(splitPoint1, size);

            int[] childTourList = new int[size];
            bool[] used = new bool[size + 1];

            // wyjmujemy środek z 1 rodzica pomiędzy wylosowanymi punktami
            for (int i = 0; i < childTourList.Length; i++)
            {
                if (i < splitPoint1)
                {
                    childTourList[i] = -1;
                }
                else if (i >= splitPoint1 && i < splitPoint2)
                {
                    used[parents[0].tourList[i]] = true;
                    childTourList[i] = parents[0].tourList[i];
                }
            }

            int index = splitPoint2;
            // uzupełniamy ogon 
            for (int i = splitPoint2; i < childTourList.Length; i++)
            {
                int element = parents[1].tourList[index];
                while (used[element])
                {
                    index = index == size - 1 ? 0 : index + 1;
                    element = parents[1].tourList[index];
                }
                childTourList[i] = element;
                used[element] = true;
            }

            //uzupełniam początek listy
            for (int i = 0; i < splitPoint1; i++)
            {
                int element = parents[1].tourList[index];
                while (used[element])
                {
                    index = index == size - 1 ? 0 : index + 1;
                    element = parents[1].tourList[index];
                }
                childTourList[i] = element;
                used[element] = true;
            }
            
            return new Individual(cities, childTourList);
        }

        public override string ToString() => "Order Crossover - Krzyżowanie z porządkowaniem";
    }
}

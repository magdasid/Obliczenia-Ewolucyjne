using System;
using TSP.Interfaces;

namespace TSP.ParentSelectors
{
    public class RouletteWheelSelection : IParentSelection
    {
        private static Random Random = RandomGenerator.GetInstance();

        public Individual FindParent(Individual[] population, Cities cities)
        {
            Individual parent = null;
            double sum = 0;

            for (int i = 0; i < population.Length; i++)
                sum += 1.0d / population[i].tourLength;

            double randomElement = Random.NextDouble() * sum;
            double partialSum = 0;
            int j = 0;

            while (partialSum < randomElement)
            {
                partialSum += 1.0d / population[j].tourLength;
                j++;
            }
            j--;
            parent = new Individual(cities, population[j].tourList);

            return parent;
        }

        public override string ToString() => "Selekcja metodą ruletki wartościowej";
    }
}

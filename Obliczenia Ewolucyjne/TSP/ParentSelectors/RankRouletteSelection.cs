using System;
using TSP.Interfaces;

namespace TSP.ParentSelectors
{
    public class RankRouletteSelection : IParentSelection
    {
        private static Random Random = RandomGenerator.GetInstance();

        public Individual FindParent(Individual[] population, Cities cities)
        {
            Array.Sort(population, (x, y) => {
                if (x.tourLength < y.tourLength)
                {
                    return -1;
                }
                else if (x.tourLength > y.tourLength)
                {
                    return 1;
                }
                return 0;
            });

            double partialSum = 0;
            int sum = (population.Length * (population.Length + 1)) / 2;
            double randomElement = Random.NextDouble() * sum;
            int j = 0;
            while (partialSum < randomElement)
            {
                partialSum += population.Length - j;
                j++;
            }

            return new Individual(population[j - 1].ordinalList, cities); ;
        }

        public override string ToString()
        {
            return "Selekcja metodą ruletki rankingowej";
        }
    }
}

using System;
using TSP.Interfaces;

namespace TSP.ParentSelectors
{

    public class RouletteWheel : IParentSelection
    {
        private static Random Random = new Random();

        public Individual FindParent(Individual[] population, Cities cities)
        {
            Individual parent = null;
            double sum = 0;
            double[] tourProbability = new double[population.Length];

            for (int i = 0; i < population.Length; i++)
                sum += (1.0d / population[i].tourLength);

            for (int i = 0; i < tourProbability.Length; i++)
                tourProbability[i] = (1.0d/population[i].tourLength) / sum;

            double randomElement = Random.NextDouble();
            double partialSum = 0;
            int j = 0;

            while (partialSum < randomElement)
            {
                partialSum += tourProbability[j];
                j++;
            }
            j--;
            parent = new Individual(cities, population[j].tourList);

            return parent;
        }

        public override string ToString()
        {
            return "Selekcja metodą ruletki wartościowej";
        }
    }
}

using System;
using TSP.Interfaces;

namespace TSP
{
    public class Generation
    {
        private static Random Random = new Random();

        private Individual[] population;
        private Cities cities;
        private IParentSelection parentSelection;
        private ICrossover crossoverOperator;
        private IMutation methodOfMutation;

        public Generation(Individual[] currentPopulation, Cities cities, IParentSelection parentSelection, ICrossover crossoverOperator, IMutation methodOfMutation)
        {
            this.population = currentPopulation;
            this.cities = cities;
            this.parentSelection = parentSelection;
            this.crossoverOperator = crossoverOperator;
            this.methodOfMutation = methodOfMutation;
        }

        public Individual[] ProcessEpoche()
        {
            Individual[] newPopulation = new Individual[population.Length];

            for (int i = 0; i < population.Length; i++)
            {
                Individual[] parents = FindParents();
                Individual child = crossoverOperator.Cross(parents, cities);
                Individual mutatedChild = methodOfMutation.Mutate(cities, child);

                newPopulation[i] = mutatedChild;
            }

            population = newPopulation;
            return population;
        }

        public double GetShortestPathFromGeneration()
        {
            double bestResult = population[0].tourLength;

            for (int i = 1; i < population.Length; i++)
            {
                if (population[i].tourLength < bestResult)
                {
                    bestResult = population[i].tourLength;
                }
            }

            return bestResult;
        }

        private Individual[] FindParents()
        {
            return new Individual[] {
                parentSelection.FindParent(population, cities),
                parentSelection.FindParent(population, cities)
            };
        }

    }
}

using System;
using TSP.Interfaces;

namespace TSP
{
    public class TSPAlgorithm
    {
        private static Random Random = new Random();

        private IParentSelection parentSelection;
        public ICrossover crossoverOperator;
        private IMutation methodOfMutation;
        public int numberOfGenerations;
        private int populationSize;
        private Cities cities;
        private double[] bestResults;

        public TSPAlgorithm(int numberOfGenerations, int populationSize, Cities cities,
            IParentSelection parentSelection, IMutation methodOfMutation, ICrossover crossoverOperator)
        {
            this.numberOfGenerations = numberOfGenerations;
            this.populationSize = populationSize;
            this.cities = cities;
            this.parentSelection = parentSelection;
            this.crossoverOperator = crossoverOperator;
            this.methodOfMutation = methodOfMutation;
            this.bestResults = new double[numberOfGenerations];
        }

        public string GetInformations()
        {
            return $"Operator krzyżowania: {crossoverOperator.ToString()}" + Environment.NewLine +
                   $"Wybór rodzica metodą: {parentSelection.ToString()}" + Environment.NewLine +
                   $"Metoda mutacji: {methodOfMutation.ToString()}" + Environment.NewLine +
                   $"Wielkość populacji: {populationSize}" +Environment.NewLine +
                   $"Liczba pokoleń: {numberOfGenerations}" + Environment.NewLine +
                   $"Zestaw danych: {cities.setName}" + Environment.NewLine +
                   $"Liczba miast: {cities.cities.Length}" + Environment.NewLine;
        }

        public void Process()
        {
            Individual[] currentPopulation;
            if (crossoverOperator is ICrossoverUsingPathRepresentation)
                currentPopulation = GeneratePopulationInPathRepresentation();
            else if (crossoverOperator is ICrossoverUsingOridinalRepresentation)
                currentPopulation = GeneratePopulationInOrdinalRepresentation();
            else
                throw new Exception("Unknown crossover operator.");

            for (int i = 0; i < numberOfGenerations; i++)
            {
                Generation generation = new Generation(currentPopulation, cities, parentSelection, crossoverOperator, methodOfMutation);
                currentPopulation = generation.ProcessEpoche();
                bestResults[i] = generation.GetShortestPathFromGeneration();

                Console.WriteLine($"Epoka: {(i + 1)}, Najlepszy wynik: {bestResults[i]}");
            }
        }
        
        /// <summary>
        /// Zwraca najlepszy wynik w danym wywołaniu
        /// </summary>
        /// <returns></returns>
        public double GetBestResult()
        {
            return bestResults.Min();
        }

        public double[] GetBestResults()
        {
            return bestResults;
        }

        /// <summary>
        /// generowanie populacji startowej w reprezentacji porządkowej
        /// </summary>
        private Individual[] GeneratePopulationInOrdinalRepresentation()
        {
            Individual[] population = new Individual[populationSize];
            int numberOfCities = cities.cities.Length;

            for (int i = 0; i < populationSize; i++)
            {
                int[] genotype = new int[numberOfCities];
                for (int j = 0; j < numberOfCities; j++)
                {
                    genotype[j] = Random.Next(1, numberOfCities - j + 1);
                }

                population[i] = new Individual(genotype, cities); ;
            }

            return population;
        }

        // generowanie populacji startowej w reprezentacji ścieżkowej
        private Individual[] GeneratePopulationInPathRepresentation()
        {
            Individual[] population = new Individual[populationSize];
            Individual firstIndividual = null;
            int numberOfCities = cities.cities.Length;
            int[] firstGenotype = new int[numberOfCities];

            for (int i = 0; i < numberOfCities; i++)
            {
                firstGenotype[i] = i + 1;
            }
            firstIndividual = new Individual(cities, firstGenotype);

            for (int i = 0; i < populationSize; i++)
            {
                int[] genotype = new int[numberOfCities];

                genotype = firstIndividual.tourList.Shuffle();
                
                population[i] = new Individual(cities, genotype);
            }
            return population;
        }
    }
}

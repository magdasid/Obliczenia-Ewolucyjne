using System;

namespace ProjektZaliczeniowy
{
    class Program // projekt z reprezentacją ścieżkową
    {
        public static int[] Shuffle(this int[] array)
        {
            var random = new Random();
            int[] newArray = new int[array.Length];
            Array.Copy(array, newArray, array.Length);

            for (int i = array.Length; i > 1; i--)
            {
                int j = random.Next(i);
                int tmp = newArray[j];
                newArray[j] = newArray[i - 1];
                newArray[i - 1] = tmp;
            }
            return newArray;
        }

        public class Individual
        {
            public int[] tourList;
            public double tourLength;

            public Individual(Cities cities, int[] tour)
            {
                tourList = tour;
                tourLength = FindTourDistance(cities, tour);
            }

            // to będzie dane
            public double FindDistanceBetweenCities(double x1, double x2, double y1, double y2)
            {
                double distance = Math.Sqrt((Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2)));
                return distance;
            }
            public double FindTourDistance(Cities cities, int[] tourList)
            {
                double partSum = 0;
                double[] firstCityData = new double[2];
                double[] secondCityData = new double[2];

                for (int i = 0; i < tourList.Length; i++)
                {
                    if (i < tourList.Length - 1)
                    {
                        firstCityData[0] = cities.cities[tourList[i] - 1].X;
                        firstCityData[1] = cities.cities[tourList[i] - 1].Y;

                        secondCityData[0] = cities.cities[tourList[i + 1] - 1].X;
                        secondCityData[1] = cities.cities[tourList[i + 1] - 1].Y;
                    }
                    else {
                        firstCityData[0] = cities.cities[tourList[i] - 1].X;
                        firstCityData[1] = cities.cities[tourList[i] - 1].Y;

                        secondCityData[0] = cities.cities[tourList[0] - 1].X;
                        secondCityData[1] = cities.cities[tourList[0] - 1].Y;
                    }
                    partSum += FindDistanceBetweenCities(firstCityData[0], secondCityData[0], firstCityData[1], secondCityData[1]);
                }

                return partSum;
            }
        }
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

            public void Process()
            {
                Individual[] currentPopulation;
                currentPopulation = GeneratePopulationInPathRepresentation();

                for (int i = 0; i < numberOfGenerations; i++)
                {
                    currentPopulation = ProcessEpoche(currentPopulation);
                    //bestResults[i] = generation.GetShortestPathFromGeneration();
                    Console.WriteLine($"Epoka: {(i + 1)}, Najlepszy wynik: {bestResults[i]}");
                }
            }

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
            public Individual[] FindParents(Individual[] population)
            {
                Individual[] parent = null;
                parent[1] = TournamentSelection(population);
                parent[2] = TournamentSelection(population);
                return parent;
            }
            public Individual TournamentSelection(Individual[] population)
            {
                Individual parent = null;
                int size = population.Length;
                int index1 = Random.Next(0, size);
                int index2 = Random.Next(0, size);

                var tourLength1 = population[index1].tourLength;
                var tourLength2 = population[index2].tourLength;

                if (tourLength1 < tourLength2)
                {
                    parent = new Individual(cities, population[index1].tourList);
                }
                else
                {
                    parent = new Individual(cities, population[index2].tourList);
                }
                return parent;
            }

            public Individual[] ProcessEpoche(Individual[] currentPopulation)
            {
                Individual[] newPopulation = new Individual[currentPopulation.Length];

                for (int i = 0; i < currentPopulation.Length; i++)
                {
                    Individual[] parents = FindParents(currentPopulation);
                    Individual child = crossoverOperator.Cross(parents, cities);
                    Individual mutatedChild = methodOfMutation.Mutate(cities, child);

                    newPopulation[i] = mutatedChild;
                }

                currentPopulation = newPopulation;
                return currentPopulation;
            }
        }
        static void Main(string[] args)

        {
            Console.WriteLine("Hello World!");
        }
    }
}

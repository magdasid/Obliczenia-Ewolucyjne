using System;
using System.Globalization;
using System.IO;

namespace TSP
{
    public class Individual
    {
        public int[] genotype;
        public int[] tourList;

        public Individual(int[] g)
        {
            genotype = g;
            tourList = TourList(g, 29);
        }

        public int[] TourList(int[] genotype, int numberOfCities)
        {
            int[] freeList = new int[numberOfCities];
            int[] tourList = new int[numberOfCities];

            for (int i = 0; i < numberOfCities; i++)
            {
                freeList[i] = i + 1;
            }

            for (int i = 0; i < genotype.Length; i++)
            {
                int element = genotype[i] - 1;
                tourList[i] = FindCity(freeList, element);
            }

            return tourList;
        }

        public int FindCity(int[] freeList, int element)
        {
            int value = -1;
            int indexOfFreeElements = 0;

            for (int i = 0; i < freeList.Length; i++)
            {
                if (freeList[i] != -1)
                {
                    if (element == indexOfFreeElements)
                    {
                        value = freeList[i];
                        freeList[i] = -1;
                        break;
                    }
                    else
                    {
                        indexOfFreeElements++;
                    }
                }
            }
            return value;
        }
    }

    class Program
    {
        public static Random random = new Random();

        // pomocnicza do obliczania długości całej trasy
        public static double FindDistanceBetweenCities(double x1, double x2, double y1, double y2)
        {
            double distance = Math.Sqrt((Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2)));

            return distance;
        }

        // obliczanie fitness value
        public static double FindTourDistance(int[] tourList)
        {
            double partSum = 0;
            String file = @"C:\Users\Madzia\Desktop\distances.txt";
            string[] lines = new string[tourList.Length];

            lines = File.ReadAllLines(file);
            string[] firstCityData = null;
            string[] secondCityData = null;

            //Console.WriteLine("tourLength" + tourList.Length);
            for (int i = 0; i < tourList.Length; i++)
            {
                if (i < tourList.Length - 1)
                {
                    firstCityData = lines[tourList[i] - 1].Split(' ');
                    //Console.WriteLine("l;ol" + (tourList[i] - 1));
                    secondCityData = lines[tourList[i + 1] - 1].Split(' ');
                    //Console.WriteLine("lol" + (tourList[i+1] - 1));
                } else
                {
                    firstCityData = lines[tourList[i] - 1].Split(' ');
                    secondCityData = lines[tourList[0] - 1].Split(' ');
                }

                double x1 = double.Parse(firstCityData[1], CultureInfo.InvariantCulture);
                //Console.WriteLine(x1);
                double y1 = double.Parse(firstCityData[2], CultureInfo.InvariantCulture);
                //Console.WriteLine(y1);
                double x2 = double.Parse(secondCityData[1], CultureInfo.InvariantCulture);
                //Console.WriteLine(x2);
                double y2 = double.Parse(secondCityData[2], CultureInfo.InvariantCulture);
                //Console.WriteLine(y2);

                partSum += FindDistanceBetweenCities(x1, x2, y1, y2);
                //Console.WriteLine("partSum" + partSum);
            }

            //Console.WriteLine("sum:" + partSum);

            return partSum;
        }

        // generowanie populacji startowej
        public static Individual[] GeneratePopulation(int populationSize, int numberOfCities)
        {
            Individual[] population = new Individual[populationSize];

            for (int i = 0; i < populationSize; i++)
            {
                int[] genotype = new int[numberOfCities];
                for (int j = 0; j < numberOfCities; j++)
                {
                    genotype[j] = random.Next(1, numberOfCities - j + 1);
                }

                Individual individual = new Individual(genotype);

                population[i] = individual;
                
            }
            return population;
        }

        public static Individual[] FindParents(Individual[] population)
        {
            Individual[] parents = new Individual[2];

            parents[0] = TournamentSelection(population);
            parents[1] = TournamentSelection(population);
            
            return parents;
        }

        public static Individual TournamentSelection(Individual[] population)
        {
            Individual parent = null;
            int size = population.Length;
            int index1 = random.Next(0, size);
            int index2 = random.Next(0, size);

            var genotype1 = population[index1].genotype;
            var genotype2 = population[index2].genotype;

            var phenotype1 = population[index1].tourList;
            var phenotype2 = population[index2].tourList;


            if (FindTourDistance(phenotype1) > FindTourDistance(phenotype2))
            {
                parent = new Individual(genotype1);
            }
            else
            {
                parent = new Individual(genotype2);
            }

            return parent;
        }

        public static Individual CreateChild(Individual[] parents)
        {
            int size = parents[0].genotype.Length;
            int splitPoint = random.Next(1, size);
            //Console.WriteLine("split point: "+splitPoint );
            var mother = parents[0];
            var father = parents[1];
            
            Individual child = null;

            int[] childGenotype = new int[size];

            for (int i = 0; i < childGenotype.Length; i++)
            {
                if(i <= splitPoint)
                {
                    childGenotype[i] = mother.genotype[i];
                } else
                {
                    childGenotype[i] = father.genotype[i];
                }
            }

            child = new Individual(childGenotype);

            return child;
        }

        public static Individual[] CreateEpoch(Individual[] startingPopulation, int populationSize)
        {
            Individual[] newPopulation = new Individual[populationSize];

            for (int i = 0; i < startingPopulation.Length; i++)
            {
                Individual[] parents = FindParents(startingPopulation);
                Individual child = CreateChild(parents);

                /* sprawdzenie
                for (int j = 0; j < child.genotype.Length; j++)
                {
                    Console.WriteLine("mother:" + parents[0].genotype[j]);
                    Console.WriteLine("father:" + parents[1].genotype[j]);
                    Console.WriteLine("child:" + child.genotype[j]);
                }*/

                newPopulation[i] = child;
            }
            return newPopulation;
        }

        public static double FindShortestPath(Individual[] population)
        {
            double? bestResult = null;
            
            for (int i = 0; i < population.Length - 1; i++)
            {
                if (FindTourDistance(population[i].tourList) < FindTourDistance(population[i+1].tourList))
                {
                    bestResult = FindTourDistance(population[i].tourList);
                }
                else
                {
                    bestResult = FindTourDistance(population[i+1].tourList);
                }
            }
            
            return bestResult.Value;
        }

        static void Main(string[] args)
        {
            /*
            CreateEpoch(startingPopulation); */

            int populationSize = 1000;
            int numberOfEpoch = 10000;

            Individual[] startingPopulation = GeneratePopulation(populationSize, 29);
            
            for (int i = 1; i <= numberOfEpoch; i++)
            {
                Individual[] newPopulation = CreateEpoch(startingPopulation, populationSize);

                startingPopulation = newPopulation;
                
                Console.WriteLine("Epoka: " + i + ", Najlepszy wynik: " + FindShortestPath(newPopulation));
            }

            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}

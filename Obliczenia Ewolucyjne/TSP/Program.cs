using System;
using System.Globalization;
using System.IO;

namespace TSP
{
    public class Cities
    {
        public City[] cities;

        public Cities(string file)
        {
            cities = ReadCitiesFromFile(file);
        }

        public City[] ReadCitiesFromFile(string file)
        {
            string[] lines = File.ReadAllLines(file);
            string[] cityData = null;
            City[] cities = new City[lines.Length];
            
            for (int i = 0; i < lines.Length; i++)
            {
                cityData = lines[i].Split(' ');
                cities[i] = new City(Int32.Parse(cityData[0]), double.Parse(cityData[1], CultureInfo.InvariantCulture), double.Parse(cityData[2], CultureInfo.InvariantCulture));
            }

            return cities;
        }
    }
    public class City
    {
        public int numberOfcity;
        public double x;
        public double y;

        public City(int number, double x1, double y1)
        {
            numberOfcity = number;
            x = x1;
            y = y1;
        }
    }
    public class Individual
    {
        public int[] genotype;
        public int[] tourList;
        public double tourLength;
       
        public Individual(int[] g, Cities cities)
        {
            genotype = g;
            tourList = TourList(g, 29);
            tourLength = FindTourDistance(cities, TourList(g, 29));
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

        public double FindDistanceBetweenCities(double x1, double x2, double y1, double y2)
        {
            double distance = Math.Sqrt((Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2)));

            return distance;
        }

        public double FindTourDistance(Cities cities, int[] tourList)
        {
            double partSum = 0;
            //String file = @"C:\Users\Madzia\Desktop\distances.txt";
            //string[] lines = new string[tourList.Length];

            //lines = File.ReadAllLines(file);
            double[] firstCityData = new double[2];
            double[] secondCityData = new double[2];

            //Console.WriteLine("tourLength" + tourList.Length);
            for (int i = 0; i < tourList.Length; i++)
            {
                if (i < tourList.Length - 1)
                {
                    firstCityData[0] = cities.cities[tourList[i]-1].x;
                    firstCityData[1] = cities.cities[tourList[i]-1].y;
                    //firstCityData = lines[tourList[i] - 1].Split(' ');
                    //Console.WriteLine("l;ol" + (tourList[i] - 1));

                    secondCityData[0] = cities.cities[tourList[i + 1]-1].x;
                    secondCityData[1] = cities.cities[tourList[i + 1]-1].y;
                    //Console.WriteLine("lol" + (tourList[i+1] - 1));
                }
                else
                {
                    firstCityData[0] = cities.cities[tourList[i]-1].x;
                    firstCityData[1] = cities.cities[tourList[i]-1].y;

                    secondCityData[0] = cities.cities[tourList[0]-1].x;
                    secondCityData[1] = cities.cities[tourList[0]-1].y;
                }

                partSum += FindDistanceBetweenCities(firstCityData[0], secondCityData[0], firstCityData[1], secondCityData[1]);
                //Console.WriteLine("partSum" + partSum);
            }

            //Console.WriteLine("sum:" + partSum);

            return partSum;
        }
    }

    class Program
    {
        public static Random random = new Random();
        
        // generowanie populacji startowej
        public static Individual[] GeneratePopulation(int populationSize, int numberOfCities, Cities cities)
        {
            Individual[] population = new Individual[populationSize];

            for (int i = 0; i < populationSize; i++)
            {
                int[] genotype = new int[numberOfCities];
                for (int j = 0; j < numberOfCities; j++)
                {
                    genotype[j] = random.Next(1, numberOfCities - j + 1);
                }

                Individual individual = new Individual(genotype, cities);

                population[i] = individual;
                
            }
            return population;
        }

        public static Individual[] FindParents(Individual[] population, Cities cities)
        {
            Individual[] parents = new Individual[2];

            parents[0] = Roulette(population, cities);
            parents[1] = Roulette(population, cities);
            
            return parents;
        }

        public static Individual TournamentSelection(Cities cities, Individual[] population)
        {
            Individual parent = null;
            int size = population.Length;
            int index1 = random.Next(0, size);
            int index2 = random.Next(0, size);

            var genotype1 = population[index1].genotype;
            var genotype2 = population[index2].genotype;

            var tourLength1 = population[index1].tourLength;
            var tourLength2 = population[index2].tourLength;


            if (tourLength1 < tourLength2)
            {
                parent = new Individual(genotype1, cities);
            }
            else
            {
                parent = new Individual(genotype2, cities);
            }

            return parent;
        }

        public static Individual Roulette(Individual[] population, Cities cities)
        {
            Individual parent = null;
            
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

            double randomElement = random.NextDouble() * sum;
            int j = 0;

            while (partialSum < randomElement)
            {
                partialSum += population.Length - j;
                parent = new Individual(population[j].genotype, cities);
                j++;
            }
            
            return parent;
        }
    

        public static Individual CreateChild(Individual[] parents, Cities cities)
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

            child = MutateChild(new Individual(childGenotype, cities), 0.5, cities);


            return child;
        }

        public static Individual MutateChild(Individual child, double probabilityOfMutation, Cities cities)
        {
            if (random.NextDouble() > probabilityOfMutation)
            {
                return child;
            }

            Individual mutatedChild = null;

            int childGenotypeLength = child.genotype.Length;
            int splitPoint = random.Next(0, childGenotypeLength);
            
            child.genotype[splitPoint] = random.Next(1, childGenotypeLength - splitPoint + 1);

            mutatedChild = new Individual(child.genotype, cities);

            return mutatedChild;
        }

        public static Individual[] CreateEpoch(Individual[] startingPopulation, int populationSize, Cities cities)
        {
            Individual[] newPopulation = new Individual[populationSize];

            for (int i = 0; i < startingPopulation.Length; i++)
            {
                Individual[] parents = FindParents(startingPopulation, cities);
                Individual child = CreateChild(parents, cities);

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
                if (population[i].tourLength < population[i+1].tourLength)
                {
                    bestResult = population[i].tourLength;
                }
                else
                {
                    bestResult = population[i+1].tourLength;
                }
            }
            
            return bestResult.Value;
        }

        static void Main(string[] args)
        {
            String file = @"C:\Users\Madzia\Desktop\distances.txt";

            Cities testCities = new Cities(file);
            
            int populationSize = 1000;
            int numberOfEpoch = 1000;
            double[] bestResults = new double[numberOfEpoch];

            Individual[] startingPopulation = GeneratePopulation(populationSize, 29, testCities);
            
            for (int i = 1; i <= numberOfEpoch; i++)
            {
                Individual[] newPopulation = CreateEpoch(startingPopulation, populationSize, testCities);

                startingPopulation = newPopulation;

                bestResults[i] = FindShortestPath(newPopulation);
                Console.WriteLine("Epoka: " + i + ", Najlepszy wynik: " + FindShortestPath(newPopulation));
            }

            double? bestResult = null;

            for (int i = 0; i < bestResults.Length; i++)
            {
                if (bestResults[i] < bestResults[i+1])
                    {
                        bestResult = bestResults[i];
                    }
                    else
                    {
                        bestResult = bestResults[i+1];
                    }
                
            }
            
           // Console.WriteLine("best" + bestResult);

            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}

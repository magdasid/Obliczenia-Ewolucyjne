using System;
using System.Globalization;
using System.IO;

namespace TSP
{
    public static class Extensions
    {
        public static bool Contains(this int[] array, int value)
        {
            return Array.IndexOf(array, value) != -1;
        }

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
    }

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

    public enum RepresentationType { Path, Ordinal };

    public class Individual
    {
        public int[] ordinalList;
        public int[] tourList;
        public double tourLength;
        public int[] adjacencyList;
        public RepresentationType representation;
        
        public Individual(int[] ordinalL, Cities cities)
        {
            ordinalList = ordinalL;
            tourList = TourList(ordinalL, ordinalL.Length);
            tourLength = FindTourDistance(cities, TourList(ordinalL, 29));
            representation = RepresentationType.Ordinal;
        }

        public Individual(Cities cities, int[] tour)
        {
            tourList = tour;
            adjacencyList = AdjacencyList(tourList);
            tourLength = FindTourDistance(cities, tour);
            representation = RepresentationType.Path;
        }

        public int[] AdjacencyList(int[] tourList)
        {
            int[] adjacencyList = new int[tourList.Length];

            for (int i = 0; i < adjacencyList.Length; i++)
            {
                int index = Array.IndexOf(tourList, i+1);

                if(index < adjacencyList.Length-1)
                {
                    adjacencyList[i] = tourList[index + 1];
                } else
                {
                    adjacencyList[i] = tourList[0];
                }
            }

            /*
            for (int i = 0; i < adjacencyList.Length; i++)
            {
                Console.WriteLine("adj" + adjacencyList[i]);
            }  */
            return adjacencyList;
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
                Console.WriteLine("tourList" + tourList[i]);
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

        // generowanie populacji startowej w reprezentacji porządkowej
        public static Individual[] GeneratePopulationInOrdinalRepresentation(int populationSize, int numberOfCities, Cities cities)
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

        // generowanie populacji startowej w reprezentacji ścieżkowej
        public static Individual[] GeneratePopulationInPathRepresentation(int populationSize, int numberOfCities, Cities cities)
        {
            Individual[] population = new Individual[populationSize];

            Individual firstIndividual = null;
            int[] firstGenotype = new int[numberOfCities];

            for (int i = 0; i < numberOfCities; i++)
            {
                firstGenotype[i] = i+1;
                
                Console.WriteLine("gen" + firstGenotype[i]);
            }
            firstIndividual = new Individual(cities, firstGenotype);

            for (int i = 0; i < populationSize; i++)
            {
                int[] genotype = new int[numberOfCities];

                genotype = firstIndividual.tourList.Shuffle();

                for (int j = 0; j < genotype.Length; j++)
                {
                    Console.WriteLine("gem" + genotype[j]);
                }
                
                Individual individual = new Individual(cities, genotype);

                population[i] = individual;
            }
            return population;
        }

        public static Individual[] FindParents(Individual[] population, Cities cities)
        {
            Individual[] parents = new Individual[2];

            parents[0] = TournamentSelection(cities, population);
            parents[1] = TournamentSelection(cities, population);
            
            return parents;
        }

        public static Individual TournamentSelection(Cities cities, Individual[] population)
        {
            Individual parent = null;
            RepresentationType type = population[1].representation;

            int size = population.Length;
            int index1 = random.Next(0, size);
            int index2 = random.Next(0, size);
            
            var tourLength1 = population[index1].tourLength;
            var tourLength2 = population[index2].tourLength;


            if (tourLength1 < tourLength2)
            {
                if (type == RepresentationType.Ordinal)
                {
                    parent = new Individual(population[index1].ordinalList, cities);
                } else
                {
                    parent = new Individual(population[index1].tourList, cities);
                }
                
            }
            else
            {
                if (type == RepresentationType.Ordinal)
                {
                    parent = new Individual(population[index2].ordinalList, cities);
                }
                else
                {
                    parent = new Individual(population[index2].tourList, cities);
                }
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
                parent = new Individual(population[j].ordinalList, cities);
                j++;
            }
            
            return parent;
        }
    
        // klasyczne krzyżowanie dla reprezentacji porządkowej
        public static Individual ClassicalCrossover(Individual[] parents, Cities cities)
        {
            int size = parents[0].ordinalList.Length;
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
                    childGenotype[i] = mother.ordinalList[i];
                } else
                {
                    childGenotype[i] = father.ordinalList[i];
                }
            }

            child = MutateChild(new Individual(childGenotype, cities), 0.5, cities);
            return child;
        }

        // mutacja dla reprezentacji porządkowej
        public static Individual MutateChild(Individual child, double probabilityOfMutation, Cities cities)
        {
            if (random.NextDouble() > probabilityOfMutation)
            {
                return child;
            }

            Individual mutatedChild = null;

            int childGenotypeLength = child.ordinalList.Length;
            int splitPoint = random.Next(0, childGenotypeLength);

            child.ordinalList[splitPoint] = random.Next(1, childGenotypeLength - splitPoint + 1);

            mutatedChild = new Individual(child.ordinalList, cities);

            return mutatedChild;
        }
        
        public static Individual PMX(Individual[] parents)
        {
            for (int i = 0; i < parents[0].tourList.Length; i++)
            {
                Console.WriteLine("parent 1 tour: " + parents[0].tourList[i]);
            }
            for (int i = 0; i < parents[0].tourList.Length; i++)
            {
                Console.WriteLine("parent 2 tour: " + parents[1].tourList[i]);
            }

            int size = parents[0].tourList.Length;
            Console.WriteLine("size: "+size);
            int splitPoint1 = random.Next(1, size);
            Console.WriteLine("splitPoint 1: " + splitPoint1);
            int splitPoint2 = random.Next(splitPoint1+1, size);
            Console.WriteLine("splitPoint 2: " + splitPoint2);

            Individual child = null;

            int[] childTourList = new int[size];
            int[] tab = new int[size];

            for (int i = 0; i < childTourList.Length; i++)
            {
                if (i < splitPoint1)
                {
                    tab[i] = -1;
                    childTourList[i] = -1;
                }
                else if (i >= splitPoint1 && i < splitPoint2)
                {
                    tab[i] = parents[0].tourList[i];
                    childTourList[i] = parents[0].tourList[i];
                }
            }
            
            for (int i = 0; i < childTourList.Length; i++)
            {
                if (i < splitPoint1 || i >= splitPoint2)
                {
                    if (Array.IndexOf(tab, parents[1].tourList[i]) == -1)
                    {
                        childTourList[i] = parents[1].tourList[i];
                        tab[i] = parents[1].tourList[i];
                    }
                    else
                    {
                        int element = parents[1].tourList[i];
                        int elementIndex = Array.IndexOf(parents[0].tourList, element);
                        int val = parents[1].tourList[elementIndex];
                        while (Array.IndexOf(childTourList, val) != -1)
                        {
                            element = val;
                            elementIndex = Array.IndexOf(parents[0].tourList, element);
                            val = parents[1].tourList[elementIndex];
                        }
                        childTourList[i] = val;
                        //childTourList[i] = parents[1].tourList[Array.IndexOf(parents[0].tourList, parents[1].tourList[i])];
     
                    }
                }
            }

            for (int i = 0; i < childTourList.Length; i++)
            {
                Console.WriteLine("childTour: " + childTourList[i]);
            }
            return child;
        }

        public static Individual OX(Individual[] parents)
        {
            int size = parents[0].tourList.Length;
            Console.WriteLine("size: " + size);
            int splitPoint1 = random.Next(1, size);
            //int splitPoint1 = 1;
            Console.WriteLine("splitPoint 1: " + splitPoint1);
            int splitPoint2 = random.Next(splitPoint1 + 1, size);
            //int splitPoint2 = 3;
            Console.WriteLine("splitPoint 2: " + splitPoint2);

            Individual child = null;

            int[] childTourList = new int[size];
            int[] tab = new int[size];

            // wyjmujemy środek z 1 rodzica pomiędzy wylosowanymi punktami
            for (int i = 0; i < childTourList.Length; i++)
            {
                if (i < splitPoint1)
                {
                    tab[i] = -1;
                    childTourList[i] = -1;
                }
                else if (i >= splitPoint1 && i < splitPoint2)
                {
                    tab[i] = parents[0].tourList[i];
                    childTourList[i] = parents[0].tourList[i];
                }
            }

            int index = splitPoint2;
            // uzupełniamy ogon 
            for (   int i = splitPoint2; i < childTourList.Length; i++)
            {
                Console.WriteLine("index" + index);
                
                int element = parents[1].tourList[index];
              
                while(Array.IndexOf(tab, element) != -1)
                {
                    Console.WriteLine("el przed"+element);
                    if (index == size - 1)
                    {
                        index = 0;
                    }
                    else
                    {
                        index++;
                    }
                    element = parents[1].tourList[index];
                    Console.WriteLine("el po" + element);

                }
                childTourList[i] = element;
                tab[i] = element;
            }
            
            //uzupełniam początek listy
            for (int i = 0; i < splitPoint1; i++)
            {
                int element = parents[1].tourList[index];

                while (Array.IndexOf(tab, element) != -1)
                {
                    Console.WriteLine("el przed" + element);
                    if (index == size - 1)
                    {
                        index = 0;
                    }
                    else
                    {
                        index++;
                    }
                    element = parents[1].tourList[index];

                }
                childTourList[i] = element;
                tab[i] = element;
            }
            
            // tylko do wyświetlania
            for (int i = 0; i < childTourList.Length; i++)
            {
                Console.WriteLine("childTour: " + childTourList[i]);
            }

            return child;

        }
        
        public static Individual CX(int[] parent, int[] parent2)
        {
            int size = parent.Length;
            Individual child = null;

            int[] childTourList = new int[size];
            int index = 0;
            int element = parent[index];

            Console.WriteLine("element"+element);

            // wprowadzamy to co możemy z 1 rodzica, dopóki nie zaczną się powtarzać miasta
            while (Array.IndexOf(childTourList, element) == -1)
            {
                childTourList[index] = element;
                index = Array.IndexOf(parent, parent2[index]);
                
                element = parent[index];
            }

            // dopisujemy resztę z rodzica nr 2
            for (int i = 0; i < childTourList.Length; i++)
            {
                if(childTourList[i] == 0)
                {
                    childTourList[i] = parent2[i];
                }
            }

            for (int i = 0; i < childTourList.Length; i++)
            {
                Console.WriteLine("childTour: " + childTourList[i]);
            }
            return child;

        }

        public static Individual SCC(int[] parent, int[] parent2)
        {
            Individual child = null;
            int size = parent.Length;
            int[] childTourList = new int[size];
            bool[] used = new bool[size + 1];

            int index = 0;
            int element = parent[index];
            bool takeFromFirstParent = true;
            int numberOfChunks = 2;

            // 1 Krok
            used[element] = true;
            used[index + 1] = true;
            childTourList[index] = element;
            index = element - 1;
            element = parent[index];
            numberOfChunks--;

            for (int i = 1; i < childTourList.Length; i++)
            {
                if (numberOfChunks <= 0)
                {
                    numberOfChunks = 2; 
                    takeFromFirstParent = !takeFromFirstParent;
                }

                if (takeFromFirstParent)
                {
                    if (i == childTourList.Length - 1) 
                    {
                        for (int j = 1; j <= size; j++)
                        {
                            if (!childTourList.Contains(j))
                            {
                                element = j;
                                break;
                            }
                        }
                    }
                    else
                    {
                        while (used[element] || childTourList.Contains(element))
                        {
                            element = random.Next(1, size + 1);
                        }
                    }

                    childTourList[index] = element;
                    used[element] = true;
                    index = element - 1;
                    element = parent2[index];
                }
                else
                {
                    if (i == childTourList.Length - 1)
                    {
                        for (int j = 1; j <= size; j++)
                        {
                            if (!childTourList.Contains(j))
                            {
                                element = j;
                                break;
                            }
                        }
                    }
                    else
                    {
                        while (used[element] || childTourList.Contains(element)) 
                        {
                            element = random.Next(1, size + 1); 
                        }
                    }

                    childTourList[index] = element; 
                    used[element] = true; 
                    index = element - 1; 
                    element = parent[index]; 
                }

                numberOfChunks--;
            }

            Console.WriteLine(string.Join(' ', used)); // wyświetli cała tablice w 1 linii  
            Console.WriteLine(string.Join(' ', childTourList)); // wyświetli cała tablice w 1 linii  
            return child;
        }

        public static Individual AEC(int[] parent, int[] parent2)
        {
            Individual child = null;
            int size = parent.Length;
            int[] childTourList = new int[size];
            bool[] used = new bool[size + 1];

            int index = 0;
            int element = parent[index]; 

            // 1 Krok
            used[element] = true; 
            used[index + 1] = true; 
            childTourList[index] = element; 
            index = element - 1; 
            element = parent2[index]; 

            for (int i = 1; i < childTourList.Length; i++)
            {
                if (i % 2 == 0)
                {
                    if (i == childTourList.Length - 1) // ostatni krok
                    {
                        for (int j = 1; j <= size; j++)
                        {
                            if (!childTourList.Contains(j))
                            {
                                element = j;
                                break;
                            }
                        }
                    }
                    else
                    {
                        while (used[element] || childTourList.Contains(element))
                        {
                            element = random.Next(1, size + 1);
                        }
                    }

                    childTourList[index] = element;
                    used[element] = true;
                    index = element - 1;
                    element = parent2[index];
                }
                else
                {
                    if (i == childTourList.Length - 1) // ostatni krok
                    {
                        for (int j = 1; j <= size; j++)
                        {
                            if (!childTourList.Contains(j))
                            {
                                element = j;
                                break;
                            }
                        }
                    }
                    else
                    {
                        while (used[element] || childTourList.Contains(element)) 
                        {
                            element = random.Next(1, size + 1);
                        }
                    }

                    childTourList[index] = element;
                    used[element] = true; 
                    index = element - 1; 
                    element = parent[index];
                }
            }


            Console.WriteLine(string.Join(' ', used)); // wyświetli cała tablice w 1 linii  
            Console.WriteLine(string.Join(' ', childTourList)); // wyświetli cała tablice w 1 linii  
            return child;
        }
        
        

        public static Individual[] CreateEpoch(Individual[] startingPopulation, int populationSize, Cities cities)
        {
            Individual[] newPopulation = new Individual[populationSize];

            for (int i = 0; i < startingPopulation.Length; i++)
            {
                Individual[] parents = FindParents(startingPopulation, cities);
                Individual child = ClassicalCrossover(parents, cities);

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
            /*
            int[] par1 = new int[9] { 2, 7, 8, 1, 3, 5, 9, 4, 6 };
            int[] par2 = new int[9] { 7, 1, 6, 2, 9, 4, 8, 5, 3 };

            //AEC(par1, par2);
            SCC(par1, par2);
            */

            int populationSize = 10;
            int numberOfEpoch = 10;
            String file = @"distances.txt";
            Cities testCities = new Cities(file);

            Individual[] startingPopulation = GeneratePopulationInOrdinalRepresentation(populationSize, 6, testCities);

            for (int i = 1; i <= numberOfEpoch; i++)
            {
                Individual[] newPopulation = CreateEpoch(startingPopulation, populationSize, testCities);

                startingPopulation = newPopulation;
            }

            Console.ReadKey();
        }
    }
}

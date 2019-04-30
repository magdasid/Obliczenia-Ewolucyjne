using System;
using TSP.Enums;

namespace TSP
{

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
            tourLength = FindTourDistance(cities, TourList(ordinalL, cities.cities.Length));
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
                int index = Array.IndexOf(tourList, i + 1);

                if (index < adjacencyList.Length - 1)
                {
                    adjacencyList[i] = tourList[index + 1];
                }
                else
                {
                    adjacencyList[i] = tourList[0];
                }
            }
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
            
            for (int i = 0; i < tourList.Length; i++)
            {
                if (i < tourList.Length - 1)
                {
                    firstCityData[0] = cities.cities[tourList[i] - 1].X;
                    firstCityData[1] = cities.cities[tourList[i] - 1].Y;
                    
                    secondCityData[0] = cities.cities[tourList[i + 1] - 1].X;
                    secondCityData[1] = cities.cities[tourList[i + 1] - 1].Y;
                }
                else
                {
                    firstCityData[0] = cities.cities[tourList[i] - 1].X;
                    firstCityData[1] = cities.cities[tourList[i] - 1].Y;

                    secondCityData[0] = cities.cities[tourList[0] - 1].X;
                    secondCityData[1] = cities.cities[tourList[0] - 1].Y;
                }

                partSum += FindDistanceBetweenCities(firstCityData[0], secondCityData[0], firstCityData[1], secondCityData[1]);
                ////Console.WriteLine("partSum" + partSum);
            }
            
            return partSum;
        }
    }
}

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
            tourLength = FindTourDistance(cities, tourList);
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
            int[] mappingTourList = new int[tourList.Length + 1];
            for (int i = 0; i < tourList.Length; i++)
                mappingTourList[tourList[i]] = i;

            for (int i = 0; i < adjacencyList.Length; i++)
            {
                int index = mappingTourList[i + 1];
                if (index < adjacencyList.Length - 1)
                    adjacencyList[i] = tourList[index + 1];
                else
                    adjacencyList[i] = tourList[0];
            }
            return adjacencyList;
        }

        public int[] TourList(int[] genotype, int numberOfCities)
        {
            int[] freeList = new int[numberOfCities];
            int[] tourList = new int[numberOfCities];

            for (int i = 0; i < numberOfCities; i++)
                freeList[i] = i + 1;

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
                        indexOfFreeElements++;
                }
            }
            return value;
        }

        public double FindTourDistance(Cities cities, int[] tourList)
        {
            double partSum = 0;
            int tourLength = tourList.Length;
            for (int i = 0; i < tourLength - 1; i++)
                partSum += cities.GetDistanceBetweenCities(tourList[i] - 1, tourList[i + 1] - 1);
            // Ostatnia suma - ostatnie miasto z pierwszym
            partSum += cities.GetDistanceBetweenCities(tourList[tourLength - 1] - 1, tourList[0] - 1);
            return partSum;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using TSP.Interfaces;

namespace TSP.Mutation
{
    public class TranspositionMutation : IMutation
    {
        private static Random Random = new Random();
        private double probabilityOfMutation;

        public TranspositionMutation(double probability)
        {
            probabilityOfMutation = probability;
        }

        public Individual Mutate(Cities cities, Individual child)
        {
            if (Random.NextDouble() > probabilityOfMutation)
            {
                return child;
            }

            Individual mutatedChild = null;
            int[] mutatedChildTourList = child.tourList;
            int firstCityIndex = Random.Next(1, child.tourList.Length);
            int firstCity = child.tourList[firstCityIndex];
            int secondCityIndex = Random.Next(1, child.tourList.Length);
            int secondCity = child.tourList[secondCityIndex];

            mutatedChildTourList[firstCityIndex] = secondCity;
            mutatedChildTourList[secondCityIndex] = firstCity;

            mutatedChild = new Individual(cities, mutatedChildTourList);

            return mutatedChild;
        }

        public override string ToString()
        {
            return "Transposition Mutation - losowa zamiana dwóch miast";
        }
    }
}

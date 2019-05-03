using System;
using TSP.Interfaces;

namespace TSP.Mutation
{
    public class TranspositionMutation : IMutation
    {
        private static Random Random = RandomGenerator.GetInstance();
        private double probabilityOfMutation;

        public TranspositionMutation(double probability)
        {
            probabilityOfMutation = probability;
        }

        public Individual Mutate(Cities cities, Individual child)
        {
            if (Random.NextDouble() > probabilityOfMutation)
                return child;

            int[] mutatedChildTourList = new int[child.tourList.Length];
            Array.Copy(child.tourList, mutatedChildTourList, child.tourList.Length);

            int firstCityIndex = Random.Next(1, child.tourList.Length);
            int firstCity = child.tourList[firstCityIndex];
            int secondCityIndex = Random.Next(1, child.tourList.Length);
            int secondCity = child.tourList[secondCityIndex];

            mutatedChildTourList[firstCityIndex] = secondCity;
            mutatedChildTourList[secondCityIndex] = firstCity;

            return new Individual(cities, mutatedChildTourList);
        }

        public override string ToString() => "Transposition Mutation - losowa zamiana dwóch miast";
    }
}

using System;
using TSP.Interfaces;

namespace TSP.Mutation
{
    public class InsertionMutation : IMutation
    {
        private static Random Random = new Random();
        private double probabilityOfMutation;

        public InsertionMutation(double probability)
        {
            probabilityOfMutation = probability;
        }

        public Individual Mutate(Cities cities, Individual child)
        {
            if (Random.NextDouble() > probabilityOfMutation)
                return child;

            Individual mutatedChild = null;
            int[] childTourList = child.tourList;
            int[] mutatedChildTourList = new int[childTourList.Length];

            int indexOfCity = Random.Next(1, childTourList.Length);
            Console.WriteLine(indexOfCity);
            int numberOfDisplacement = Random.Next(1, childTourList.Length);
            Console.WriteLine(numberOfDisplacement);

            if ((childTourList.Length - indexOfCity) - numberOfDisplacement > 0)
            {
                Array.Copy(childTourList, 0, mutatedChildTourList, 0, indexOfCity);
                Array.Copy(childTourList, indexOfCity + 1, mutatedChildTourList, indexOfCity, numberOfDisplacement);
                Array.Copy(childTourList, indexOfCity, mutatedChildTourList, numberOfDisplacement + indexOfCity, 1);
                Array.Copy(childTourList, indexOfCity + numberOfDisplacement + 1, mutatedChildTourList, indexOfCity + numberOfDisplacement + 1, childTourList.Length - (numberOfDisplacement + indexOfCity + 1));
            }
            else
            {
                Array.Copy(childTourList, 0, mutatedChildTourList, 0, Math.Abs((childTourList.Length - indexOfCity) - numberOfDisplacement));
                Array.Copy(childTourList, indexOfCity, mutatedChildTourList, Math.Abs((childTourList.Length - indexOfCity) - numberOfDisplacement), 1);
                int sum = 1 + Math.Abs((childTourList.Length - indexOfCity) - numberOfDisplacement);
                Array.Copy(childTourList, sum - 1, mutatedChildTourList, sum, indexOfCity - sum + 1);
                Array.Copy(childTourList, indexOfCity + 1, mutatedChildTourList, indexOfCity + 1, mutatedChildTourList.Length - indexOfCity - 1);
            }

            mutatedChild = new Individual(cities, mutatedChildTourList);
            return mutatedChild;
        }
        public override string ToString() => "Insertion Mutation - przesunięcie miasta";
    }
}

using System;
using TSP.Interfaces;

namespace TSP.Mutation
{
    public class InversionMutation : IMutation
    {
        private static Random Random = new Random();
        private double probabilityOfMutation;

        public InversionMutation(double probability)
        {
            probabilityOfMutation = probability;
        }

        public Individual Mutate(Cities cities, Individual child)
        {
            if (Random.NextDouble() > probabilityOfMutation)
                return child;

            int[] childTourList = child.tourList;
            int[] mutatedChildTourList = new int[childTourList.Length];
            int firstDivisionPoint = Random.Next(1, childTourList.Length - 1);
            Console.WriteLine(firstDivisionPoint);
            int secondDivisionPoint = Random.Next(firstDivisionPoint + 1, childTourList.Length - 1);
            Console.WriteLine(secondDivisionPoint);

            Array.Copy(childTourList, 0, mutatedChildTourList, 0, firstDivisionPoint);
            int j = secondDivisionPoint;

            for (int i = firstDivisionPoint; i <= secondDivisionPoint; i++)
            {
                mutatedChildTourList[i] = childTourList[j];
                j--;
            }
            Array.Copy(childTourList, secondDivisionPoint + 1, mutatedChildTourList, secondDivisionPoint + 1, childTourList.Length - secondDivisionPoint - 1);

            return new Individual(cities, mutatedChildTourList);
        }
        public override string ToString() => "Invertion Mutation - odwrócenie kolejności miast wewnątrz podciągu";
    }
}

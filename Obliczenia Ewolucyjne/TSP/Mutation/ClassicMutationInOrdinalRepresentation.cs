using System;
using TSP.Interfaces;

namespace TSP.Mutation
{
    public class ClassicMutationInOrdinalRepresentation : IMutation
    {
        private static Random Random = RandomGenerator.GetInstance();
        private double probabilityOfMutation;

        public ClassicMutationInOrdinalRepresentation(double probability)
        {
            probabilityOfMutation = probability;
        }

        public Individual Mutate(Cities cities, Individual child)
        {
            if (Random.NextDouble() > probabilityOfMutation)
                return child;

            int childGenotypeLength = child.ordinalList.Length;
            int splitPoint = Random.Next(0, childGenotypeLength);

            child.ordinalList[splitPoint] = Random.Next(1, childGenotypeLength - splitPoint + 1);

            return new Individual(child.ordinalList, cities);
        }

        public override string ToString() => "Mutacja jednego genu";
    }
}

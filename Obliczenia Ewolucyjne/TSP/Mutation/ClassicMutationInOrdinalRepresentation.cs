using System;
using System.Collections.Generic;
using System.Text;
using TSP.Interfaces;

namespace TSP.Mutation
{
    public class ClassicMutationInOrdinalRepresentation : IMutation
    {
        private static Random Random = new Random();
        private double probabilityOfMutation;

        public ClassicMutationInOrdinalRepresentation(double probability)
        {
            probabilityOfMutation = probability;
        }

        public Individual Mutate(Cities cities, Individual child)
        {
            if (Random.NextDouble() > probabilityOfMutation)
            {
                return child;
            }

            int childGenotypeLength = child.ordinalList.Length;
            int splitPoint = Random.Next(0, childGenotypeLength);

            child.ordinalList[splitPoint] = Random.Next(1, childGenotypeLength - splitPoint + 1);

            return new Individual(child.ordinalList, cities);
        }
        public override string ToString()
        {
            return "Mutacja jednego genu";
        }

    }
}

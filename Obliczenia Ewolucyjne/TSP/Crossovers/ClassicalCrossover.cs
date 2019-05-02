using System;
using TSP.Interfaces;

namespace TSP.Crossovers
{
    public class ClassicalCrossover : ICrossover, ICrossoverUsingOridinalRepresentation
    {
        public static Random Random = RandomGenerator.GetInstance();

        public Individual Cross(Individual[] parents, Cities cities)
        {
            int size = parents[0].ordinalList.Length;
            int splitPoint = Random.Next(1, size);
     
            var mother = parents[0];
            var father = parents[1];

            int[] childGenotype = new int[size];

            for (int i = 0; i < childGenotype.Length; i++)
            {
                if (i <= splitPoint)
                    childGenotype[i] = mother.ordinalList[i];
                else
                    childGenotype[i] = father.ordinalList[i];
            }

            return new Individual(childGenotype, cities);
        }

        public override string ToString() => "Classical crossover - Krzyżowanie klasyczne";
    }
}

using System;
using TSP.Interfaces;

namespace TSP.ParentSelectors
{
    public class TournamentSelection : IParentSelection
    {
        private static Random Random = RandomGenerator.GetInstance();

        public Individual FindParent(Individual[] population, Cities cities)
        {
            int size = population.Length;
            int index1 = Random.Next(0, size);
            int index2 = Random.Next(0, size);

            var tourLength1 = population[index1].tourLength;
            var tourLength2 = population[index2].tourLength;

            return tourLength1 < tourLength2 ? population[index1] : population[index2];
        }

        public override string ToString() => "Selekcja metodą turniejową";
    }
}

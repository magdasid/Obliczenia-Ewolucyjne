using System;
using TSP.Enums;
using TSP.Interfaces;

namespace TSP.ParentSelectors
{
    public class TournamentSelection : IParentSelection
    {
        private static Random Random = new Random();

        public Individual FindParent(Individual[] population, Cities cities)
        {
            Individual parent = null;
            RepresentationType type = population[1].representation;

            int size = population.Length;
            int index1 = Random.Next(0, size);
            int index2 = Random.Next(0, size);

            var tourLength1 = population[index1].tourLength;
            var tourLength2 = population[index2].tourLength;

            if (tourLength1 < tourLength2)
            {
                if (type == RepresentationType.Ordinal)
                {
                    parent = new Individual(population[index1].ordinalList, cities);
                }
                else
                {
                    parent = new Individual(cities, population[index1].tourList);
                }

            }
            else
            {
                if (type == RepresentationType.Ordinal)
                {
                    parent = new Individual(population[index2].ordinalList, cities);
                }
                else
                {
                    parent = new Individual(cities, population[index2].tourList);
                }
            }

            return parent;
        }

        public override string ToString()
        {
            return "Selekcja metodą turniejową";
        }
    }
}

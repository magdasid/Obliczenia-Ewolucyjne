using System;
using System.Collections.Generic;

namespace Zadanie3
{
    public class Individual
    {
        public uint genotype;
        public double phenotype;

        public Individual(uint g)
        {
            genotype = g;
            phenotype = Phenotype(g);
        }

        public double Phenotype(uint genotype)
        {
            return -2 + genotype / Math.Pow(10, 9);
        }
    }
    public class Generation
    {
        public static Random random = new Random();

        public List<Individual> startingPopulation;
        public List<Individual> newPopulation;

        public Generation(List<Individual> population)
        {
            startingPopulation = population;
            newPopulation = CreateNewPopulation(population);
        }

        public double Fitness(double x)
        {
            double value = 0;
            value = x * Math.Sin(x) * Math.Sin(10 * x);
            return value;
        }

        // funkcja zwracająca rodziców
        public List<Individual> TournamentSelection(List<Individual> population)
        {
            List<Individual> parents = new List<Individual>();

            parents[0] = FindParent(population);
            parents[1] = FindParent(population);

            //Console.WriteLine("matka: " + Convert.ToString(parents[0], 2).PadLeft(32, '0'));
            //Console.WriteLine("ojciec: " + Convert.ToString(parents[1], 2).PadLeft(32, '0'));

            return parents;
        }

        // funkcja do znalezienia rodzica
        public Individual FindParent(List<Individual> population)
        {
            Individual parent = null;
            int size = population.Count;
            int index1 = random.Next(0, size);
            int index2 = random.Next(0, size);

            var genotype1 = population[index1].genotype;
            var genotype2 = population[index2].genotype;

            var phenotype1 = population[index1].phenotype;
            var phenotype2 = population[index2].phenotype;

            if (Fitness(phenotype1) > Fitness(phenotype2))
            {
                parent = new Individual(genotype1);
            }
            else
            {
                parent = new Individual(genotype2);
            }

            return parent;
        }

        // krzyżowanie osobników
        public Individual CreateChild(List<Individual> parents)
        {
            int splitPoint = random.Next(1, 32);
            var mother = parents[0].genotype;
            var father = parents[1].genotype;

            //Console.WriteLine("split point: " + splitPoint);
            uint mask = ((uint)((1 << (32 - splitPoint)) - 1) << splitPoint);
            uint left = mother & mask;
            uint right = father & ~mask;
            Individual child = new Individual(left | right);

            //Console.WriteLine("dziecko: " + Convert.ToString(child, 2).PadLeft(32, '0'));

            return child;
        }

        //mutacja 
        public static Individual Mutation(Individual child)
        {
            int bitToChange = random.Next(1, 33);
            uint mask = (uint)(1 << (bitToChange - 1));
            Individual mutatedChild = new Individual(child.genotype ^ mask);

            //Console.WriteLine("bit do mutacji: " + bitToChange);
            //Console.WriteLine("dziecko po mutacji: " + Convert.ToString(mutatedChild, 2).PadLeft(32, '0'));

            return mutatedChild;
        }
        public List<Individual> CreateNewPopulation(List<Individual> startingPopulation)
        {
            List<Individual> newPop = new List<Individual>();

            for (int i = 0; i < startingPopulation.Count; i++)
            {
                List<Individual> parents = TournamentSelection(startingPopulation);
                Individual child = CreateChild(parents);
                newPopulation[i] = Mutation(child);
            }
            return newPop;
        }

    }
    public class Algorithm
    {
        public int numberOfGenerations;
        public int numberOfExecution;
        List<double> heaven = new List<double>();
        public double[] arrayOfBestResults;
        public List<Individual> startingPopulation = new List<Individual>();

        public static Random random = new Random();

        public Algorithm(int generation, int execution)
        {
            numberOfGenerations = generation;
            numberOfExecution = execution;
            arrayOfBestResults = new double[execution];
            startingPopulation = GeneratePopulation();
        }

        public List<Individual> GeneratePopulation()
        { 
            List<Individual> population = new List<Individual>(20);

            for (int i = 0; i < 20; i++)
            {
                uint genotype = (uint)random.Next();
                Individual individual = new Individual(genotype);
                population.Add(individual);

            }
            return population;
        }


    }

    class Program
    {
        static void Main(string[] args)
        {


            Console.ReadKey();


        }
    }
}

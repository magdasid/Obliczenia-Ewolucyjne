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
        public AlgorithmType typeOfAlgorithm;

        public Generation(List<Individual> population, AlgorithmType type)
        {
            startingPopulation = population;
            typeOfAlgorithm = type;
        }

        public double Fitness(double x)
        {
            double value = 0;
            value = x * Math.Sin(x) * Math.Sin(10 * x);
            return value;
        }

        // funkcja zwracająca rodziców
        private List<Individual> TournamentSelection()
        {
            List<Individual> parents = new List<Individual>
            {
                FindParent(),
                FindParent()
            };

            //Console.WriteLine("matka: " + Convert.ToString(parents[0], 2).PadLeft(32, '0'));
            //Console.WriteLine("ojciec: " + Convert.ToString(parents[1], 2).PadLeft(32, '0'));

            return parents;
        }

        // funkcja do znalezienia rodzica
        private Individual FindParent()
        {
            Individual parent = null;
            int size = startingPopulation.Count;
            int index1 = random.Next(0, size);
            int index2 = random.Next(0, size);

            var genotype1 = startingPopulation[index1].genotype;
            var genotype2 = startingPopulation[index2].genotype;

            var phenotype1 = startingPopulation[index1].phenotype;
            var phenotype2 = startingPopulation[index2].phenotype;

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
        private Individual CreateChild(List<Individual> parents)
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
        private Individual Mutation(Individual child)
        {
            int bitToChange = random.Next(1, 33);
            uint mask = (uint)(1 << (bitToChange - 1));
            Individual mutatedChild = new Individual(child.genotype ^ mask);

            //Console.WriteLine("bit do mutacji: " + bitToChange);
            //Console.WriteLine("dziecko po mutacji: " + Convert.ToString(mutatedChild, 2).PadLeft(32, '0'));

            return mutatedChild;
        }

        public List<Individual> CreateNewPopulation()
        {
            List<Individual> newPop = new List<Individual>();

            for (int i = 0; i < startingPopulation.Count; i++)
            {
                List<Individual> parents = TournamentSelection();
                Individual child = CreateChild(parents);
                Individual mutatedChild = Mutation(child);

                switch (typeOfAlgorithm)
                {
                    case AlgorithmType.Default:
                        newPop.Add(mutatedChild);
                        break;

                    case AlgorithmType.RemoveChildOutOfBoundary:
                        if (mutatedChild.phenotype > 2.0)
                        {
                            i--;
                        }
                        else
                        {
                            newPop.Add(mutatedChild);
                        }
                        break;

                    case AlgorithmType.ChangePhenotypeToBoundaryValue:
                        if (mutatedChild.phenotype > 2.0)
                        {
                            mutatedChild.phenotype = 2.0;
                            mutatedChild.genotype = 4000000000;

                            newPop.Add(mutatedChild);
                        }
                        else
                        {
                            newPop.Add(mutatedChild);
                        }
                        break;
                }

            }

            startingPopulation = newPop;
            return newPop;
        }

        public double[] FindBest()
        {
            double bestResult = 0;
            double x = 0;
            double[] result = new double[2];

            for (int i = 0; i < startingPopulation.Count - 1; i++)
            {
                if (Fitness(startingPopulation[i].phenotype) > Fitness(startingPopulation[i + 1].phenotype))
                {
                    bestResult = Fitness(startingPopulation[i].phenotype);
                    x = startingPopulation[i].phenotype;
                }
                else
                {
                    bestResult = Fitness(startingPopulation[i + 1].phenotype);
                    x = startingPopulation[i + 1].phenotype;
                }
            }

            result[0] = bestResult;
            result[1] = x;
            return result;
        }
    }

    public enum AlgorithmType { Default, RemoveChildOutOfBoundary, ChangePhenotypeToBoundaryValue };

    public class Algorithm
    {
        public static Random random = new Random();

        public int numberOfGenerations;
        public int numberOfExecution;
        public double[] arrayOfBestResults;
        public AlgorithmType typeOfAlgorithm;
        public List<Individual> currentPopulation;
        public List<double> heaven;

        public Algorithm(int generation, int execution, AlgorithmType type)
        {
            numberOfGenerations = generation;
            numberOfExecution = execution;
            typeOfAlgorithm = type;
            arrayOfBestResults = new double[execution];
        }

        private List<Individual> GeneratePopulation()
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

        public void Process()
        {
            int i = 0;
            while (i < numberOfExecution)
            {
                currentPopulation = GeneratePopulation();
                heaven = new List<double>();

                for (int j = 1; j <= numberOfGenerations; j++)
                {
                    Generation generation = new Generation(currentPopulation, typeOfAlgorithm);
                    currentPopulation = generation.CreateNewPopulation();

                    var best = generation.FindBest();

                    if (heaven.Count != 0)
                    {
                        if (best[0] > generation.Fitness(heaven[0]))
                        {
                            heaven[0] = best[1];
                        }
                    }
                    else
                    {
                        heaven.Add(best[1]);
                    }

                    Console.WriteLine("Epoka: " + j + ", Najlepszy wynik: " + best[0] + ", x = " + best[1] + " ,niebo:" + heaven[0]);
                }

                arrayOfBestResults[i] = heaven[0];
                i++;
            }

        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            Algorithm algorithm = new Algorithm(1000, 1, AlgorithmType.RemoveChildOutOfBoundary);

            algorithm.Process();

            Console.ReadKey();
        }
    }
}

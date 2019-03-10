using System;
using System.Collections.Generic;

namespace Zadanie2
{
    /* Należy "rozwiązać" problem osobników spoza dziedziny. -> usuwać dziecko, albo modyfikować genotyp
     * Należy dodać "niebo".
     * Należy dodać wielokrotne uruchamianie algorytmu i znajdowanie wartości średniej, mediany, etc.
     */

    class Program
    {
        public static Random random = new Random();

        //Funkcja obliczająca wartość f(x)=x*sin(x)*sin(10x) 
        public static double Fitness(double x)
        {
            double value = 0;
            value = x * Math.Sin(x) * Math.Sin(10 * x);
            return value;
        }

        // funkcja generującą populację startową (20 osobników)
        public static uint[] GeneratePopulation()
        {
            uint[] population = new uint[20];

            for (int i = 0; i < population.Length; i++)
            {
                population[i] = (uint)random.Next();
                //Console.WriteLine("osobnik"+ i + Phenotype(population[i]));

            }
            return population;
        }

        // funkcja zamieniająca genotyp na fenotyp
        public static double Phenotype(uint genotype)
        {
            return -2 + genotype / Math.Pow(10, 9);
        }

        // funkcja zamieniająca na genotyp
        public static uint Genotype(double phenotype)
        {
            return (uint)((phenotype + 2) * Math.Pow(10, 9));
        }

        // funkcja zwracająca rodziców
        public static uint[] TournamentSelection(uint[] population)
        {
            uint[] parents = new uint[2];

            parents[0] = FindParent(population);
            parents[1] = FindParent(population);

            //Console.WriteLine("matka: " + Convert.ToString(parents[0], 2).PadLeft(32, '0'));
            //Console.WriteLine("ojciec: " + Convert.ToString(parents[1], 2).PadLeft(32, '0'));

            return parents;
        }

        // funkcja do znalezienia rodzica
        public static uint FindParent(uint[] population)
        {
            uint parent = 0;
            int size = population.Length;
            int index1 = random.Next(0, size);
            int index2 = random.Next(0, size);

            var genotype1 = population[index1];
            var genotype2 = population[index2];

            var phenotype1 = Phenotype(genotype1);
            var phenotype2 = Phenotype(genotype2);

            if (Fitness(phenotype1) > Fitness(phenotype2))
            {
                parent = genotype1;
            }
            else
            {
                parent = genotype2;
            }

            return parent;
        }

        // krzyżowanie osobników
        public static uint CreateChild(uint[] parents)
        {
            int splitPoint = random.Next(1, 32);
            var mother = parents[0];
            var father = parents[1];

            //Console.WriteLine("split point: " + splitPoint);
            uint mask = ((uint)((1 << (32 - splitPoint)) - 1) << splitPoint);
            uint left = mother & mask;
            uint right = father & ~mask;
            uint child = left | right;
            //Console.WriteLine("dziecko: " + Convert.ToString(child, 2).PadLeft(32, '0'));

            if (Phenotype(child) > 2.0)
            {
                child = Genotype(2.0);
            }

            return child;
        }

        //mutacja 
        public static uint Mutation(uint child)
        {
            int bitToChange = random.Next(1, 33);
            uint mask = (uint)(1 << (bitToChange - 1));
            uint mutatedChild = child ^ mask;

            //Console.WriteLine("bit do mutacji: " + bitToChange);
            //Console.WriteLine("dziecko po mutacji: " + Convert.ToString(mutatedChild, 2).PadLeft(32, '0'));

            if (Phenotype(mutatedChild) > 2.0)
            {
                mutatedChild = Genotype(2.0);
            }

            return mutatedChild;
        }

        public static uint[] CreateEpoch(uint[] startingPopulation)
        {
            uint[] newPopulation = new uint[20];

            for (int i = 0; i < startingPopulation.Length; i++)
            {
                uint[] parents = TournamentSelection(startingPopulation);
                uint child = CreateChild(parents);
                newPopulation[i] = Mutation(child);
            }
            return newPopulation;
        }

        public static double[] FindBest(uint[] population)
        {
            double bestResult = 0;
            double x = 0;
            double[] result = new double[2];

            for (int i = 0; i < population.Length - 1; i++)
            {
                if (Fitness(Phenotype(population[i])) > Fitness(Phenotype(population[i + 1])))
                {
                    bestResult = Fitness(Phenotype(population[i]));
                    x = Phenotype(population[i]);
                }
                else
                {
                    bestResult = Fitness(Phenotype(population[i + 1]));
                    x = Phenotype(population[i + 1]);
                }
            }

            result[0] = bestResult;
            result[1] = x;
            return result;
        }

        static double CountAverage(double[] results)
        {
            double sum = 0;

            for (int i = 0; i < results.Length; i++)
            {
                sum += results[i];
            }

            return sum / results.Length;
        }

        static double CountMedian(double[] results)
        {
            Array.Sort(results);
            int middleIndex = results.Length / 2;
            double median = 0;

            if(results.Length % 2 != 0)
            {
                median = results[middleIndex];
            }
            else
            {
                median = (results[middleIndex] + results[middleIndex - 1]) / 2;
            }
            return median;
        }

        static void Main(string[] args)
        {
            int numberOfExecute = 0;
            double[] arrayOfBestResults = new double[10];

            while (numberOfExecute < 10)
            {
                uint[] startingPopulation = GeneratePopulation();
                int numberOfEpoch = 1000;

                // dodanie nieba (niebo ma tylko jeden element)
                List<double> heaven = new List<double>();

                for (int i = 1; i <= numberOfEpoch; i++)
                {
                    uint[] newPopulation = CreateEpoch(startingPopulation);

                    startingPopulation = newPopulation;

                    if (heaven.Count != 0)
                    {
                        if ((FindBest(newPopulation)[0]) > Fitness(heaven[0]))
                        {
                            heaven[0] = FindBest(newPopulation)[1];
                        }
                    }
                    else
                    {
                        heaven.Add(FindBest(newPopulation)[1]);
                    }

                    Console.WriteLine("Epoka: " + i + ", Najlepszy wynik: " + FindBest(newPopulation)[0] + ", x = " + FindBest(newPopulation)[1] + " ,niebo:" + heaven[0]);
                }

                arrayOfBestResults[numberOfExecute] = heaven[0];
                numberOfExecute++;
            }

            Console.WriteLine("Średni najlepszy wynik z 10 epok: " + CountAverage(arrayOfBestResults));
            Console.WriteLine("Mediana z 10 epok: " + CountMedian(arrayOfBestResults));

            Console.ReadKey();
        }
    }
}

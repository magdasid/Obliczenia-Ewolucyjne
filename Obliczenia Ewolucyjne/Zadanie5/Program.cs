﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

/*
    Zaprojektuj i wykonaj eksperymenty wykorzystując dotychczasowy kod.
    Przygotuj sprawozdanie.
*/
namespace Zadanie5
{
    public class Individual
    {
        public uint genotype;
        public double phenotype;
        public double fitnessValue;

        public Individual(uint g)
        {
            genotype = g;
            phenotype = Phenotype(g);
            fitnessValue = Fitness(Phenotype(g));
        }

        public double Phenotype(uint genotype)
        {
            return -2 + genotype / Math.Pow(10, 9);
        }

        public double Fitness(double x)
        {
            double value = 0;
            value = x * Math.Sin(x) * Math.Sin(10 * x);
            return value;
        }
    }
    public interface IParentSelection
    {
        Individual FindParent(List<Individual> population);
    }

    public class TournamentSelection : IParentSelection
    {
        private Random random;

        public TournamentSelection(Random random)
        {
            this.random = random;
        }

        public Individual FindParent(List<Individual> population)
        {
            Individual parent = null;
            int size = population.Count;
            int index1 = random.Next(0, size);
            int index2 = random.Next(0, size);

            if (population[index1].fitnessValue > population[index2].fitnessValue)
            {
                parent = population[index1];
            }
            else
            {
                parent = population[index2];
            }

            return parent;
        }

        public override string ToString()
        {
            return "Selekcja turniejowa";
        }
    }

    public class Roulette1Selection : IParentSelection
    {
        private Random random;

        public Roulette1Selection(Random random)
        {
            this.random = random;
        }

        public Individual FindParent(List<Individual> population)
        {
            // liczymy funkcje dopasowania dlanaszych osobników
            Individual parent = null;
            double sum = 0;
            double min = 0;
            double[] arrayOfFittnes = new double[20];

            for (int i = 0; i < population.Count; i++)
            {
                arrayOfFittnes[i] = population[i].fitnessValue;

                if (arrayOfFittnes[i] < min)
                {
                    min = arrayOfFittnes[i];
                }
            }

            for (int i = 0; i < arrayOfFittnes.Length; i++)
            {
                arrayOfFittnes[i] = arrayOfFittnes[i] + (-1) * min;
                sum += arrayOfFittnes[i];

            }

            double randomElement = random.NextDouble();
            double partialSum = 0;
            int j = 0;

            while (partialSum < randomElement)
            {
                partialSum += arrayOfFittnes[j] / sum;

                parent = population[j];
                j++;
            }

            return parent;
        }

        public override string ToString()
        {
            return "Selekcja metodą ruletka wartościowa";
        }
    }

    public class Roulette2Selection : IParentSelection
    {
        private Random random;

        public Roulette2Selection(Random random)
        {
            this.random = random;
        }

        public Individual FindParent(List<Individual> population)
        {
            // liczymy funkcje dopasowania dlanaszych osobników
            Individual parent = null;
            Individual[] individuals = new Individual[20];

            for (int i = 0; i < population.Count; i++)
            {
                individuals[i] = population[i];
            }

            Array.Sort(individuals, (x, y) => {
                if (x.fitnessValue > y.fitnessValue)
                {
                    return -1;
                }
                else if (x.fitnessValue < y.fitnessValue)
                {
                    return 1;
                }
                return 0;
            });

            double partialSum = 0;
            int sum = (individuals.Length * (individuals.Length + 1)) / 2;
            double randomElement = random.NextDouble() * sum;
            int j = 0;

            while (partialSum < randomElement)
            {
                partialSum += individuals.Length - j;
                parent = individuals[j];
                j++;
            }

            return parent;
        }

        public override string ToString()
        {
            return "Selekcja metodą ruletka rankingowa";
        }
    }

    public class Generation
    {
        public static Random random = new Random();
        private IParentSelection parentSelection;

        public List<Individual> startingPopulation;
        public AlgorithmType typeOfAlgorithm;
        public double probabilityOfMutation;

        public Generation(List<Individual> population, double probability, AlgorithmType type, IParentSelection parentSelection)
        {
            startingPopulation = population;
            probabilityOfMutation = probability;
            typeOfAlgorithm = type;
            this.parentSelection = parentSelection;
        }

        public double Fitness(double x)
        {
            double value = 0;
            value = x * Math.Sin(x) * Math.Sin(10 * x);
            return value;
        }

        // funkcja zwracająca rodziców
        private (Individual mother, Individual father) FindParents()
        {
            Individual mother = parentSelection.FindParent(startingPopulation);
            Individual father = parentSelection.FindParent(startingPopulation);

            return (mother, father);
        }

        // krzyżowanie osobników
        private Individual CreateChild(Individual mother, Individual father)
        {
            int splitPoint = random.Next(1, 32);
            var motherGenotype = mother.genotype;
            var fatherGenotype = father.genotype;

            //Console.WriteLine("split point: " + splitPoint);
            uint mask = ((uint)((1 << (32 - splitPoint)) - 1) << splitPoint);
            uint left = motherGenotype & mask;
            uint right = fatherGenotype & ~mask;
            Individual child = new Individual(left | right);

            //Console.WriteLine("dziecko: " + Convert.ToString(child, 2).PadLeft(32, '0'));

            return child;
        }
        //mutacja 
        private Individual Mutation(Individual child)
        {
            if (random.NextDouble() > probabilityOfMutation)
            {
                return child;
            }

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
                var parents = FindParents();
                Individual child = CreateChild(parents.mother, parents.father);
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

        public (double fitnessValue, double phenotype) FindBest()
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

            return (bestResult, x);
        }
    }

    public enum AlgorithmType { Default, RemoveChildOutOfBoundary, ChangePhenotypeToBoundaryValue };

    public class Algorithm
    {
        private static Random random = new Random();

        public int numberOfGenerations;
        public int GenerationWhere80PercetangeBestAchieved = 0;
        public int GenerationWhere90PercetangeBestAchieved = 0;
        public int GenerationWhere95PercetangeBestAchieved = 0;
        private AlgorithmType typeOfAlgorithm;
        private IParentSelection parentSelection;
        private double probabilityOfMutation;
        private List<Individual> currentPopulation;
        public List<double> heaven;

        public Algorithm(int generation, double probability, AlgorithmType type, IParentSelection parentSelection)
        {
            numberOfGenerations = generation;
            probabilityOfMutation = probability;
            typeOfAlgorithm = type;
            this.parentSelection = parentSelection;
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

        public void CheckPercentage(double current, int currentEpoche)
        {
            if (heaven.Count == 0)
                return;

            var best80Percentage = 0.8 * heaven[0];
            var best90Percentage = 0.9 * heaven[0];
            var best95Percentage = 0.95 * heaven[0];

            if (GenerationWhere80PercetangeBestAchieved == 0 && current >= best80Percentage)
            {
                // lepszy niż 80% najlepszego
                GenerationWhere80PercetangeBestAchieved = currentEpoche;
            }
            if (GenerationWhere90PercetangeBestAchieved == 0 && current >= best90Percentage)
            {
                // lepszy niż 90% najlepszego
                GenerationWhere90PercetangeBestAchieved = currentEpoche;
            }
            if (GenerationWhere95PercetangeBestAchieved == 0 && current >= best95Percentage)
            {
                // lepszy niż 95% najlepszego
                GenerationWhere95PercetangeBestAchieved = currentEpoche;
            }
        }

        public void Process()
        {
            currentPopulation = GeneratePopulation();
            GenerationWhere80PercetangeBestAchieved = 0;
            GenerationWhere90PercetangeBestAchieved = 0;
            GenerationWhere95PercetangeBestAchieved = 0;
            heaven = new List<double>();

            for (int j = 1; j <= numberOfGenerations; j++)
            {
                Generation generation = new Generation(currentPopulation, probabilityOfMutation, typeOfAlgorithm, parentSelection);
                currentPopulation = generation.CreateNewPopulation();

                var best = generation.FindBest();
                CheckPercentage(best.phenotype, j);

                if (heaven.Count != 0)
                {
                    if (best.fitnessValue > generation.Fitness(heaven[0]))
                    {
                        heaven[0] = best.fitnessValue;
                    }
                }
                else
                {
                    heaven.Add(best.fitnessValue);
                }
            }
        }

        public string GetInformations()
        {
            return $"Typ algorytmu: {typeOfAlgorithm.ToString()}\n" +
                   $"Wybór rodzica metodą: {parentSelection.ToString()}\n" +
                   $"Prawdopodobieństwo mutacji: {probabilityOfMutation}\n" +
                   $"Liczba pokoleń: {numberOfGenerations}";
        }
    }


    public class AlgorithmExecutor // xdddd
    {
        private int NumberOfExecutions;
        private Algorithm AlgorithmToExecute;
        private double[] ArrayOfBestResults;
        private StringBuilder Informations = new StringBuilder();

        public AlgorithmExecutor(Algorithm algorithm, int numberOfExecutions)
        {
            NumberOfExecutions = numberOfExecutions;
            ArrayOfBestResults = new double[numberOfExecutions];
            AlgorithmToExecute = algorithm;
            Informations.AppendLine(algorithm.GetInformations());
        }

        public void Start()
        {
            Informations.AppendLine($"Wywołano: {DateTime.Now.ToString("U")}");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int i = 0;
            while (i < NumberOfExecutions)
            {
                AlgorithmToExecute.Process();

                ArrayOfBestResults[i] = AlgorithmToExecute.heaven[0];
                i++;
            }
            stopwatch.Stop();

            Informations
                .AppendLine($"Mediana z najlepszych: {CountMedian(ArrayOfBestResults)}")
                .AppendLine($"Średnia z najlepszych: {CountAverage(ArrayOfBestResults)}")
                .AppendLine($"Odchylenie standardowe najlepszych wyników: {CountStandardDeviation(ArrayOfBestResults)}")
                .AppendLine($"Przedziały ufności: {FindConfidenceInterval(ArrayOfBestResults)}")
                .AppendLine($"Czas wykonania: {stopwatch.Elapsed} ({stopwatch.ElapsedMilliseconds}ms)")
                .AppendLine("#####################")
                .AppendLine($"80% najlepszego zostało osiągnięte w {AlgorithmToExecute.GenerationWhere80PercetangeBestAchieved} epoce")
                .AppendLine($"90% najlepszego zostało osiągnięte w {AlgorithmToExecute.GenerationWhere90PercetangeBestAchieved} epoce")
                .AppendLine($"95% najlepszego zostało osiągnięte w {AlgorithmToExecute.GenerationWhere95PercetangeBestAchieved} epoce")
                .AppendLine($"Liczba wykonań programu: {NumberOfExecutions}")
                .AppendLine($"--------------------------------------------")
                .AppendLine();
        }

        public double CountAverage(double[] results)
        {
            double sum = 0;

            for (int i = 0; i < results.Length; i++)
            {
                sum += results[i];
            }

            return sum / results.Length;
        }

        public double CountMedian(double[] results)
        {
            Array.Sort(results);
            int middleIndex = results.Length / 2;
            double median = 0;

            if (results.Length % 2 != 0)
            {
                median = results[middleIndex];
            }
            else
            {
                median = (results[middleIndex] + results[middleIndex - 1]) / 2;
            }
            return median;
        }

        public double CountStandardDeviation(double[] results)
        {
            // odchylenie standardowe, potrzeba średniej
            double average = CountAverage(results);
            double partialSum = 0;

            for (int i = 0; i < results.Length; i++)
            {
                partialSum += Math.Pow((results[i] - average), 2);
            }

            double standardDeviation = Math.Sqrt(partialSum / results.Length);

            return standardDeviation;
        }

        public (double lower, double upper) FindConfidenceInterval(double[] results)
        {
            // średnia +/- błąd standardowy * 1,96
            double standardError = CountStandardDeviation(results) / Math.Sqrt(results.Length);
            double average = CountAverage(results);

            double lowerConfidenceInterval = average - (standardError * 1.96);
            double upperConfidenceInterval = average + (standardError * 1.96);

            return (lowerConfidenceInterval, upperConfidenceInterval);
        }

        public void SaveInformations(string filename)
        {
            File.AppendAllText(filename, Informations.ToString());
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string filename = DateTime.Now.ToString("yyyy-MM-dd dddd");
            Algorithm algorithm1 = new Algorithm(1000, 0.1, AlgorithmType.Default, new Roulette2Selection(new Random()));

            Console.WriteLine("Wyniki ALGORYTM 1");

            AlgorithmExecutor algorithmExecutor = new AlgorithmExecutor(algorithm1, 10);

            algorithmExecutor.Start();
            algorithmExecutor.SaveInformations(filename);

            /*Algorithm algorithm2 = new Algorithm(1000, 1, 0.1, AlgorithmType.Default, new Roulette2Selection(new Random()));

            Console.WriteLine("Wyniki ALGORYTM 2");

            algorithm2.Process(); */

            /*

            Algorithm algorithm2 = new Algorithm(10000, 1, 0.1, AlgorithmType.ChangePhenotypeToBoundaryValue);

            Console.WriteLine("Wyniki ALGORYTM 2");
            algorithm2.Process();

            Algorithm algorithm3 = new Algorithm(1000, 10, 0.1, AlgorithmType.RemoveChildOutOfBoundary);

            Console.WriteLine("Wyniki ALGORYTM 3");
            algorithm3.Process();
            */

            Console.ReadKey();
        }
    }
}

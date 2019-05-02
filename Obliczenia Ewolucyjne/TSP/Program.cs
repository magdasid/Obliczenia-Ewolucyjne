using System;
using TSP.Crossovers;
using TSP.Interfaces;
using TSP.Mutation;
using TSP.ParentSelectors;

namespace TSP
{
    class Program
    {
        static void Main(string[] args)
        {             
            int populationSize = 1000;
            int numberOfEpoch = 10000;
            int numberOfExecution = 1;
            double probabilityOfMutation = 0.1;
            string file = @"WesternSahara.txt";
            Cities testCities = new Cities(file);
            IParentSelection parentSelection = new TournamentSelection();
            ICrossover crossover = new OX();
            IMutation mutation = new TranspositionMutation(probabilityOfMutation);
            string filename = DateTime.Now.ToString("yyyy-MM-dd HH;mm");

            // Test 1 - PMX
            TSPAlgorithm tsp = new TSPAlgorithm(numberOfEpoch, populationSize, testCities, parentSelection, mutation, crossover);
            AlgorithmExecutor AE = new AlgorithmExecutor(tsp, numberOfExecution);
            AE.Start();
            //AE.SaveInformations(filename + ".txt");
            //AE.ExportStatisticsToCsv(filename + "_stats.csv");

            //// Test 2 - CX
            //crossover = new CX();
            //tsp = new TSPAlgorithm(numberOfEpoch, populationSize, testCities, parentSelection, mutation, crossover);
            //AE = new AlgorithmExecutor(tsp, numberOfExecution);
            //AE.Start();
            //AE.SaveInformations(filename + ".txt");
            //AE.ExportStatisticsToCsv(filename + "_stats.csv");

            //// Test 3 - OX
            //crossover = new OX();
            //tsp = new TSPAlgorithm(numberOfEpoch, populationSize, testCities, parentSelection, mutation, crossover);
            //AE = new AlgorithmExecutor(tsp, numberOfExecution);
            //AE.Start();
            //AE.SaveInformations(filename + ".txt");
            //AE.ExportStatisticsToCsv(filename + "_stats.csv");

            //// Test 4 - AEX
            //crossover = new AEX();
            //tsp = new TSPAlgorithm(numberOfEpoch, populationSize, testCities, parentSelection, mutation, crossover);
            //AE = new AlgorithmExecutor(tsp, numberOfExecution);
            //AE.Start();
            //AE.SaveInformations(filename + ".txt");
            //AE.ExportStatisticsToCsv(filename + "_stats.csv");

            //// Test 5 - SCC
            //crossover = new AEX();
            //tsp = new TSPAlgorithm(numberOfEpoch, populationSize, testCities, parentSelection, mutation, crossover);
            //AE = new AlgorithmExecutor(tsp, numberOfExecution);
            //AE.Start();
            //AE.SaveInformations(filename + ".txt");
            //AE.ExportStatisticsToCsv(filename + "_stats.csv");

            //// Test 6 - SCC
            //crossover = new ClassicalCrossover();
            //mutation = new ClassicMutationInOrdinalRepresentation(probabilityOfMutation);
            //tsp = new TSPAlgorithm(numberOfEpoch, populationSize, testCities, parentSelection, mutation, crossover);
            //AE = new AlgorithmExecutor(tsp, numberOfExecution);
            //AE.Start();
            //AE.SaveInformations(filename + ".txt");
            //AE.ExportStatisticsToCsv(filename + "_stats.csv");

            Console.ReadKey();
        }
    }
}

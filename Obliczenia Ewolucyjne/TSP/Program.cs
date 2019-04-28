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
            int numberOfEpoch = 1000;
            int numberOfExecution = 1;
            double probabilityOfMutation = 0.1;
            string file = @"WesternSahara.txt";
            Cities testCities = new Cities(file);
            IParentSelection parentSelection = new TournamentSelection();
            ICrossover crossover = new PMX();
            IMutation mutation = new TranspositionMutation(probabilityOfMutation);
            string filename = DateTime.Now.ToString("yyyy-MM-dd HH;mm") + ".txt";

            TSPAlgorithm tsp = new TSPAlgorithm(numberOfEpoch, populationSize, testCities, parentSelection, mutation, crossover);

            AlgorithmExecutor AE = new AlgorithmExecutor(tsp, numberOfExecution);
            AE.Start();
            AE.SaveInformations(filename);
            
            Console.ReadKey();
        }
    }
}

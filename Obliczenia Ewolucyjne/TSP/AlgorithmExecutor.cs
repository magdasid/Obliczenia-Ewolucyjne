using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace TSP
{
    class AlgorithmExecutor
    {
        private int NumberOfExecutions;
        private TSPAlgorithm Algorithm;
        private double[] ArrayOfBestResults;
        private StringBuilder Informations = new StringBuilder();

        public AlgorithmExecutor(TSPAlgorithm algorithm, int numberOfExecutions)
        {
            NumberOfExecutions = numberOfExecutions;
            ArrayOfBestResults = new double[numberOfExecutions];
            Algorithm = algorithm;
            Informations.AppendLine(algorithm.GetInformations());
        }

        public void Start()
        {
            Informations.AppendLine($"Liczba wykonań programu: {NumberOfExecutions}\n");
            Informations.AppendLine($"Wywołano: {DateTime.Now.ToString()}");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int i = 0;
            while (i < NumberOfExecutions)
            {
                Algorithm.Process();
                ArrayOfBestResults[i] = Algorithm.GetBestResult();

                Informations.AppendLine($"Najkrótsza trasa w {i+1} wykonaniu: {ArrayOfBestResults[i]}\n");
                i++;
            }
            stopwatch.Stop();

            Informations.AppendLine($"Czas wykonania: {stopwatch.Elapsed} ({stopwatch.ElapsedMilliseconds}ms)")
                        .AppendLine($"Najlepszy wynik globalny: {ArrayOfBestResults.Min()}");
        }

        public void SaveInformations(string filename)
        {
            File.AppendAllText(filename, Informations.ToString());
        }
    }
}

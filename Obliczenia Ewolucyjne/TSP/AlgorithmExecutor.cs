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
        private double[][] AlgorithmStatistics;
        private StringBuilder Informations = new StringBuilder();

        public AlgorithmExecutor(TSPAlgorithm algorithm, int numberOfExecutions)
        {
            NumberOfExecutions = numberOfExecutions;
            ArrayOfBestResults = new double[numberOfExecutions];
            Algorithm = algorithm;
            Informations.AppendLine(algorithm.GetInformations());
            AlgorithmStatistics = new double[numberOfExecutions][];
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
                AlgorithmStatistics[i] = Algorithm.GetBestResults();

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

        public void ExportStatisticsToCsv(string filename)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Operator krzyżowania: { Algorithm.crossoverOperator}");
            sb.AppendLine("Nr epoki; Średnia;");
            int numberOfEpoches = AlgorithmStatistics[0].Length;

            for (int i = 0; i < numberOfEpoches; i++)
            {
                var epocheStats = GetColumn(AlgorithmStatistics, i);
                sb.AppendLine($"{i + 1};{CountAverage(epocheStats)}");
            }
            sb.AppendLine();

            File.AppendAllText(filename, sb.ToString(), Encoding.UTF8);
        }

        private double[] GetColumn(double[][] array, int columnIndex)
        {
            double[] column = new double[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                column[i] = array[i][columnIndex];
            }
            return column;
        }

        private double CountAverage(double[] results)
        {
            double sum = 0;
            for (int i = 0; i < results.Length; i++)
            {
                sum += results[i];
            }
            return sum / results.Length;
        }

        /*
        public void ExportStatictiscToCsv(string filename)
        {

            var numberOfEpoches = Algorithm.numberOfGenerations;
            var stringBuilder = new StringBuilder();
            
            stringBuilder.AppendLine($"Wywołano {DateTime.Now.ToString("U")}");
            stringBuilder.AppendLine("Nr wykonania programu; Max");
            for (int i = 0; i < AlgorithmsStatictics.Count; i++)
            {
                var currentStats = AlgorithmsStatictics[i];

                stringBuilder.AppendLine($"{i + 1}; {currentStats.EpocheStatistics.Max(x => x.BestFitnessValue)}");
            }

            stringBuilder.AppendLine("##########################");
            stringBuilder.AppendLine("Nr epoki; Średnia; Max; Min");
            for (int i = 1; i <= numberOfEpoches; i++)
            {
                // to oznacza, że z tych statystyk weź element, gdzie numer epoki jest taki jak i
                var epocheStatictics = AlgorithmsStatictics.Select(x => x.EpocheStatistics.First(y => y.NumberOfEpoche == i));
                var bestFitnessValues = epocheStatictics.Select(x => x.BestFitnessValue).ToArray();
                stringBuilder.AppendLine($"{i}; {CountAverage(bestFitnessValues)}; {bestFitnessValues.Max()}; {bestFitnessValues.Min()}");
            }


            File.AppendAllText(filename, stringBuilder.ToString());
        }*/
    }
}

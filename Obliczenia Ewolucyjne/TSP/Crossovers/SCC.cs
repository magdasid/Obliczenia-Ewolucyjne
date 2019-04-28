using System;
using TSP.Interfaces;

namespace TSP.Crossovers
{
    public class SCC : ICrossover, ICrossoverUsingPathRepresentation
    {
        private static Random Random = new Random();

        public Individual Cross(Individual[] parents, Cities cities)
        {
            int[] parent = parents[0].adjacencyList;
            int[] parent2 = parents[1].adjacencyList;

            Individual child = null;
            int size = parent.Length;
            int[] childAdjacencyList = new int[size];
            int[] childTourList = new int[size];
            bool[] used = new bool[size + 1];

            int index = 0;
            int element = parent[index];
            bool takeFromFirstParent = true;
            int numberOfChunks = 2;

            // 1 Krok
            used[element] = true;
            used[index + 1] = true;
            childAdjacencyList[index] = element;
            index = element - 1;
            element = parent[index];
            numberOfChunks--;

            for (int i = 1; i < childAdjacencyList.Length; i++)
            {
                if (numberOfChunks <= 0)
                {
                    numberOfChunks = 2;
                    takeFromFirstParent = !takeFromFirstParent;
                }

                if (takeFromFirstParent)
                {
                    if (i == childAdjacencyList.Length - 1)
                    {
                        for (int j = 1; j <= size; j++)
                        {
                            if (!childAdjacencyList.Contains(j))
                            {
                                element = j;
                                break;
                            }
                        }
                    }
                    else
                    {
                        while (used[element] || childAdjacencyList.Contains(element))
                        {
                            element = Random.Next(1, size + 1);
                        }
                    }

                    childAdjacencyList[index] = element;
                    used[element] = true;
                    index = element - 1;
                    element = parent2[index];
                }
                else
                {
                    if (i == childAdjacencyList.Length - 1)
                    {
                        for (int j = 1; j <= size; j++)
                        {
                            if (!childAdjacencyList.Contains(j))
                            {
                                element = j;
                                break;
                            }
                        }
                    }
                    else
                    {
                        while (used[element] || childAdjacencyList.Contains(element))
                        {
                            element = Random.Next(1, size + 1);
                        }
                    }

                    childAdjacencyList[index] = element;
                    used[element] = true;
                    index = element - 1;
                    element = parent[index];
                }

                numberOfChunks--;
            }

            Console.WriteLine(string.Join(' ', used));
            Console.WriteLine(string.Join(' ', childAdjacencyList));
            childTourList = ConvertFromAdjacencyList(childAdjacencyList);
            Console.WriteLine(string.Join(' ', childTourList));

            child = new Individual(cities, childTourList);

            return child;
        }

        private int[] ConvertFromAdjacencyList(int[] adjList)
        {
            int[] tourList = new int[adjList.Length];

            int index = 0;

            int i = 0;
            while (i < tourList.Length)
            {
                int value = adjList[index];
                tourList[i] = value;

                index = value - 1;
                i++;
            }

            Console.WriteLine(string.Join(' ', tourList));
            return tourList;
        }

        public override string ToString()
        {
            return "Subtour Chunks Crossover - Krzyżowanie przez wymianę podtras";

        }
    }
}

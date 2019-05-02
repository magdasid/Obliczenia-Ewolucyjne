using System;
using System.Globalization;
using System.IO;

namespace TSP
{
    public class Cities
    {
        public City[] cities { get; private set; }
        public string setName;

        public Cities(string file)
        {
            setName = Path.GetFileNameWithoutExtension(file);
            cities = ReadCitiesFromFile(file);
        }

        private City[] ReadCitiesFromFile(string file)
        {
            string[] lines = File.ReadAllLines(file);
            string[] cityData = null;
            int numberOfCities = 0;
            int i = 0;
            while (i < lines.Length)
            {
                string line = lines[i];
                if (line.Contains("DIMENSION"))
                {
                    numberOfCities = int.Parse(line.Split(':')[1]);
                }
                if (line.Contains("NODE_COORD_SECTION"))
                {
                    i++;
                    break;
                }
                i++;
            }

            City[] cities = new City[numberOfCities];
            int k = 0;
            while(i < lines.Length)
            {
                if (lines[i].Contains("EOF"))
                    break;
                cityData = lines[i].Split(' ');
                cities[k] = new City(Int32.Parse(cityData[0]), double.Parse(cityData[1], CultureInfo.InvariantCulture), double.Parse(cityData[2], CultureInfo.InvariantCulture));
                i++;
                k++;
            }

            return cities;
        }

        private double FindDistanceBetweenCities(double x1, double x2, double y1, double y2)
            => Math.Sqrt((Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2)));

        public double GetDistanceBetweenCities(int city1, int city2)
            => FindDistanceBetweenCities(cities[city1].X, cities[city2].X, cities[city1].Y, cities[city2].Y);
    }
}

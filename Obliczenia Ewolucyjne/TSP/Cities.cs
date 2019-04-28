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
            cities = ReadCitiesFromFile(file);
            setName = Path.GetFileNameWithoutExtension(file);
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
    }
}

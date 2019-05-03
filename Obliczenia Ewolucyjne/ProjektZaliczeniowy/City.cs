namespace ProjektZaliczeniowy
{
    public class City
    {
        public int NumberOfcity { get; private set; }
        public double X { get; private set; }
        public double Y { get; private set; }

        public City(int number, double x1, double y1)
        {
            NumberOfcity = number;
            X = x1;
            Y = y1;
        }
    }

}

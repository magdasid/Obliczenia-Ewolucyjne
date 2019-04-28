namespace TSP.Interfaces
{
    public interface ICrossover
    {
        Individual Cross(Individual[] parents, Cities cities);
    }
}

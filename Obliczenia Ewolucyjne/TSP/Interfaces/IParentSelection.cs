namespace TSP.Interfaces
{
    public interface IParentSelection
    {
        Individual FindParent(Individual[] population, Cities cities);
    }
}

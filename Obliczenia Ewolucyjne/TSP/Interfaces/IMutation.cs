using System;
using System.Collections.Generic;
using System.Text;

namespace TSP.Interfaces
{
    public interface IMutation
    {
        Individual Mutate(Cities cities, Individual child);
    }
}

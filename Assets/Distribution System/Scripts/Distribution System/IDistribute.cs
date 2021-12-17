using System.Collections;
using System.Collections.Generic;

namespace DistributionSystem
{
    public interface IDistribute
    {
        enum ChemistryTypes
        {
            HEAT,
            COLD,
            ELECTRICITY,
            NOTHING
        }
    }
}
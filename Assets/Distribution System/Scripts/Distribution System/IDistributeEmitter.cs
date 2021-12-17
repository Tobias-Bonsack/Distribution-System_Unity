using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DistributionSystem
{
    public interface IDistributeEmitter : IDistribute
    {
        public enum Type
        {
            STANDARD,
            GRID_MEMBER,
            GRID_CONNECTOR
        }
        void RemoveType(IDistribute.ChemistryTypes type, float radiance);
        void AddType(IDistribute.ChemistryTypes type, float radiance);

        void RemoveReceiver(IDistributeReceiver receiver);
    }
}

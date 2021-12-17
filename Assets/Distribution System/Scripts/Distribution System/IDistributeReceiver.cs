using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DistributionSystem
{
    public interface IDistributeReceiver : IDistribute
    {
        public enum Status
        {
            ENTER,
            STAY,
            EXIT
        }

        void RemoveEmitType(ADistributeEmitter emitter, IDistribute.ChemistryTypes type);
        void NewEmitType(ADistributeEmitter emitter, IDistribute.ChemistryTypes type);
    }
}

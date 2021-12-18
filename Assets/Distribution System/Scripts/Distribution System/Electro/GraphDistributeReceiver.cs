using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DistributionSystem
{
    public class GraphDistributeReceiver : ADistributeReceiver
    {
        protected override void TriggerEvents(IDistributeReceiver.Status status, Collider other)
        {
            if (other.gameObject.TryGetComponent<ADistributeEmitter>(out ADistributeEmitter chemistryEmitter))
            {
                for (int i = 0; i < chemistryEmitter.Types.Count; i++)
                {
                    IDistribute.ChemistryTypes type = chemistryEmitter.Types[i];
                    float radiance = chemistryEmitter.Radiance[i];
                    TriggerElementEvents(status, chemistryEmitter, type, radiance);
                }
            }
        }

        //TODO override from new emit type ... no enter or exit event

        public override void RemoveEmitType(ADistributeEmitter emitter, IDistribute.ChemistryTypes type)
        {
            if (emitter is GraphDistributeEmitter) _activeEmitter.Remove((IDistributeEmitter)emitter);
        }
    }
}

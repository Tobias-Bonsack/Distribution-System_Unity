using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace DistributionSystem
{
    public class DistributeEmitter : ADistributeEmitter
    {
        // Standard Emitter

        private void Awake()
        {
            _emitType = IDistributeEmitter.Type.STANDARD;
        }
    }
}

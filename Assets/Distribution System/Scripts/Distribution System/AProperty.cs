using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DistributionSystem
{
    public abstract class AProperty : MonoBehaviour
    {
        [Header("Generally Parameter")]
        [SerializeField] protected IDistribute.ChemistryTypes _type;
        [SerializeField] protected AElementReceiver _elementReceiver;

        protected ADistributeReceiver _chemistryReceiver;



        protected virtual void Awake()
        {
            _chemistryReceiver = _elementReceiver.DistributionReceiver;
        }
    }
}

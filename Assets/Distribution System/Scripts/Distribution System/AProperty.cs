using System;
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
        protected List<EventHandler<ADistributeReceiver.OnReceiveElementArgs>> _subscribedMethods = new List<EventHandler<ADistributeReceiver.OnReceiveElementArgs>>();


        protected virtual void Awake()
        {
            _chemistryReceiver = _elementReceiver.DistributionReceiver;
        }

        protected void RegisterMethodToElement(IDistribute.ChemistryTypes type, Action<object, DistributeReceiver.OnReceiveElementArgs> method)
        {
            EventHandler<ADistributeReceiver.OnReceiveElementArgs> eventHandler = new EventHandler<ADistributeReceiver.OnReceiveElementArgs>(method);
            _subscribedMethods.Add(eventHandler);
            _chemistryReceiver.UnSubscriberToElement(true, type, eventHandler);
        }
        protected virtual void OnDestroy()
        {
            Debug.Log("Destroy: " + this.GetType());
            foreach (EventHandler<ADistributeReceiver.OnReceiveElementArgs> eventHandler in _subscribedMethods)
            {
                _chemistryReceiver.UnSubscriberToElement(false, _type, eventHandler);
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DistributionSystem
{
    [RequireComponent(typeof(Collider), typeof(Rigidbody))]
    public abstract class ADistributeReceiver : MonoBehaviour, IDistributeReceiver
    {
        #region events
        public event EventHandler<OnReceiveElementArgs> _onReceiveHeat;
        public event EventHandler<OnReceiveElementArgs> _onReceiveFrost;
        public event EventHandler<OnReceiveElementArgs> _onReceiveElectricity;
        #endregion

        #region event args
        public class OnReceiveElementArgs : EventArgs
        {
            public IDistributeReceiver.Status _status;
            public float _radiance;
            public IDistributeEmitter.Type _emitterType;
        }
        #endregion

        #region trigger
        public void OnReceiveHeatTrigger(object sender, OnReceiveElementArgs args) => _onReceiveHeat?.Invoke(sender, args);
        public void OnReceiveFrostTrigger(object sender, OnReceiveElementArgs args) => _onReceiveFrost?.Invoke(sender, args);
        public void OnReceiveElectricityTrigger(object sender, OnReceiveElementArgs args) => _onReceiveElectricity?.Invoke(sender, args);
        #endregion

        #region parameter
        [SerializeField] protected bool _burnItself = false, _frostItself = false, _shockItself = false;
        [SerializeField] protected ADistributeEmitter _ownEmitter;
        #endregion

        protected HashSet<IDistributeEmitter> _activeEmitter = new HashSet<IDistributeEmitter>();

        protected void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<IDistributeEmitter>(out IDistributeEmitter emitter)) _activeEmitter.Add(emitter);
            TriggerEvents(IDistributeReceiver.Status.ENTER, other);
        }
        protected void OnTriggerStay(Collider other)
        {
            TriggerEvents(IDistributeReceiver.Status.STAY, other);

        }
        protected void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent<IDistributeEmitter>(out IDistributeEmitter emitter)) _activeEmitter.Remove(emitter);
            TriggerEvents(IDistributeReceiver.Status.EXIT, other);
        }
        protected virtual void TriggerEvents(IDistributeReceiver.Status status, Collider other)
        {
            if (other.gameObject.TryGetComponent<ADistributeEmitter>(out ADistributeEmitter chemistryEmitter))
            {
                for (int i = 0; i < chemistryEmitter.Types.Count; i++)
                {
                    IDistribute.ChemistryTypes type = chemistryEmitter.Types[i];
                    float radiance = chemistryEmitter.Radiance[i];
                    if (radiance > 0f) TriggerElementEvents(status, chemistryEmitter, type, radiance);
                }
            }
        }

        protected void TriggerElementEvents(IDistributeReceiver.Status status, ADistributeEmitter chemistryEmitter, IDistribute.ChemistryTypes type, float radiance)
        {
            OnReceiveElementArgs onReceiveArgs = new OnReceiveElementArgs { _status = status, _radiance = radiance, _emitterType = chemistryEmitter._emitType };

            switch (type)
            {
                case IDistribute.ChemistryTypes.HEAT:
                    if (IsStrangerEmitter(chemistryEmitter, _burnItself))
                    {
                        OnReceiveHeatTrigger(chemistryEmitter, onReceiveArgs);
                    }
                    break;
                case IDistribute.ChemistryTypes.COLD:
                    if (IsStrangerEmitter(chemistryEmitter, _frostItself))
                    {
                        OnReceiveFrostTrigger(chemistryEmitter, onReceiveArgs);
                    }
                    break;
                case IDistribute.ChemistryTypes.ELECTRICITY:
                    if (IsStrangerEmitter(chemistryEmitter, _shockItself))
                    {
                        OnReceiveElectricityTrigger(chemistryEmitter, onReceiveArgs);
                    }
                    break;
                default:
                    Debug.LogWarning("Unknown type");
                    break;
            }
        }

        protected bool IsStrangerEmitter(ADistributeEmitter chemistryEmitter, bool typeItself)
        {
            return typeItself || _ownEmitter == null || _ownEmitter != chemistryEmitter;
        }

        public void NewEmitType(ADistributeEmitter emitter, IDistribute.ChemistryTypes type)
        {
            _activeEmitter.Add((IDistributeEmitter)emitter);
            TriggerElementEvents(IDistributeReceiver.Status.ENTER, emitter, type, emitter.Radiance[emitter.Types.IndexOf(type)]);
        }
        public virtual void RemoveEmitType(ADistributeEmitter emitter, IDistribute.ChemistryTypes type)
        {
            if (emitter is GraphMemberEmitter) _activeEmitter.Remove((IDistributeEmitter)emitter);
            TriggerElementEvents(IDistributeReceiver.Status.EXIT, emitter, type, 0f);
        }

        protected void OnDestroy()
        {
            foreach (IDistributeEmitter emitter in _activeEmitter)
            {
                emitter.RemoveReceiver(this);
            }
        }

    }

}

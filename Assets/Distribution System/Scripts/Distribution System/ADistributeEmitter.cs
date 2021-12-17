using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using CustomAttribute;

namespace DistributionSystem
{
    [RequireComponent(typeof(Collider))]
    public abstract class ADistributeEmitter : MonoBehaviour, IDistributeEmitter
    {
        [Header("Emit Parameter")]
        [HideInNormalInspector] public IDistributeEmitter.Type _emitType;

        [Tooltip("Types of chemistry this GameObject is able to emit, each type needs an _radiance float"), SerializeField] protected List<IDistribute.ChemistryTypes> _types;
        public ReadOnlyCollection<IDistribute.ChemistryTypes> Types
        {
            get
            {
                return new ReadOnlyCollection<IDistribute.ChemistryTypes>(_types);
            }
        }


        [Tooltip("Radiance per second, Position mirrors _types"), Range(0f, 1f), SerializeField] protected List<float> _radiance;
        public ReadOnlyCollection<float> Radiance
        {
            get
            {
                return new ReadOnlyCollection<float>(_radiance);
            }
        }


        [Header("Effect Parameter")]

        [Tooltip("Types of chemistry for the effects")] public List<IDistribute.ChemistryTypes> _typeOfEffect;

        [Tooltip("Types of VisualEffects, Position mirrors _typeOfEffect")] public List<VisualEffect> _effects;

        protected HashSet<IDistributeReceiver> _activeReceiver = new HashSet<IDistributeReceiver>();

        public virtual void AddType(IDistribute.ChemistryTypes type, float radiance)
        {
            int position = Types.IndexOf(type);
            if (position == -1)
            {
                _types.Add(type);
                _radiance.Add(radiance);
                foreach (IDistributeReceiver receiver in _activeReceiver)
                {
                    receiver.NewEmitType(this, type);
                }
            }
            else
            {
                _radiance[position] += radiance;
            }

            UpdateVisualEffects(true, type);
        }

        public virtual void RemoveType(IDistribute.ChemistryTypes type, float radiance)
        {
            int position = Types.IndexOf(type);
            if (position != -1)
            {
                _radiance[position] -= radiance;

                if (_radiance[position] <= 0f)
                {
                    _types.Remove(type);
                    _radiance.RemoveAt(position);
                    foreach (IDistributeReceiver receiver in _activeReceiver)
                    {
                        receiver.RemoveEmitType(this, type);
                    }

                    if (_types.Count == 0 && _radiance.Count == 0) gameObject.SetActive(false);
                }
            }

            UpdateVisualEffects(false, type);
        }

        protected virtual void UpdateVisualEffects(bool toActivate, IDistribute.ChemistryTypes type)
        {
            int pos = _typeOfEffect.IndexOf(type);
            if (pos == -1) return;

            _effects[pos].enabled = toActivate;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<IDistributeReceiver>(out IDistributeReceiver receiver))
                _activeReceiver.Add(receiver);
        }
        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent<IDistributeReceiver>(out IDistributeReceiver receiver))
                _activeReceiver.Remove(receiver);
        }

        public void RemoveReceiver(IDistributeReceiver receiver)
        {
            _activeReceiver.Remove(receiver);
        }

        void OnDisable()
        {
            List<IDistribute.ChemistryTypes> copyTypes = new List<IDistribute.ChemistryTypes>(_types);
            foreach (IDistribute.ChemistryTypes type in copyTypes)
            {
                foreach (IDistributeReceiver receiver in _activeReceiver)
                {
                    receiver.RemoveEmitType(this, type);
                }
            }
            _activeReceiver.Clear();

        }

        void OnDestroy()
        {
            List<IDistribute.ChemistryTypes> copyTypes = new List<IDistribute.ChemistryTypes>(_types);
            foreach (IDistribute.ChemistryTypes type in copyTypes)
            {
                RemoveType(type, _radiance[Types.IndexOf(type)]);
            }
        }
    }
}

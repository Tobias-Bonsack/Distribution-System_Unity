using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DistributionSystem
{
    public class GraphDistributeEmitter : ADistributeEmitter
    {
        [SerializeField] GraphMember _member;
        public string GRAPH_NAME
        {
            get
            {
                return _member._originalGraph;
            }
        }
        private void Awake()
        {
            _emitType = IDistributeEmitter.Type.GRID_MEMBER;

            if (!_types.Contains(IDistribute.ChemistryTypes.ELECTRICITY)) _types.Add(IDistribute.ChemistryTypes.ELECTRICITY);
            if (_radiance.Count != _types.Count) _radiance.Add(0f);
        }

        public override void AddType(IDistribute.ChemistryTypes type, float radiance)
        {
            int position = Types.IndexOf(type);
            bool isFromZero = _radiance[position] == 0f;
            _radiance[position] += radiance;
            if (isFromZero)
            {
                foreach (IDistributeReceiver receiver in _activeReceiver)
                {
                    receiver.NewEmitType(this, type);
                }
            }

            UpdateVisualEffects(true, type);
        }

        public override void RemoveType(IDistribute.ChemistryTypes type, float radiance)
        {
            int position = Types.IndexOf(type);
            if (position != -1) _radiance[position] = Mathf.Clamp(_radiance[position] - radiance, 0f, 1f);
            bool isToZero = _radiance[position] == 0f;
            if (isToZero)
            {
                foreach (IDistributeReceiver receiver in _activeReceiver)
                {
                    receiver.RemoveEmitType(this, type);
                }
            }

            UpdateVisualEffects(false, type);
        }
    }
}

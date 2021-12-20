using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DistributionSystem
{
    public class RemoveEmitter : AProperty
    {
        [Header("Propertie-Parameter")]
        [SerializeField] ADistributeEmitter _emitter;
        [SerializeField, Range(0f, 1f)] float _percentToRemove;
        [SerializeField] bool _destroyAfterRemove = false;
        [SerializeField] GameObject _toDestroy;
        [SerializeField, Range(0f, 1f)] float _afterRemovePercentToDestroy;
        [SerializeField] GameObject _reducer;
        bool _isElementRemoved = false;
        protected override void Awake()
        {
            base.Awake();
            _elementReceiver._onElementPercentChange += ElementPercentChange;
        }

        private void ElementPercentChange(object sender, EventArgs e)
        {
            if (!_isElementRemoved && _elementReceiver.ElementPercent >= _percentToRemove)
            {
                _emitter.RemoveType(_type, float.MaxValue);
                _isElementRemoved = true;
                _reducer.SetActive(true);
            }

            if (_destroyAfterRemove && _isElementRemoved && _elementReceiver.ElementPercent <= _afterRemovePercentToDestroy)
            {
                Destroy(_toDestroy);
            }
        }
    }
}

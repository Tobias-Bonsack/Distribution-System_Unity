using System;
using System.Collections;
using System.Collections.Generic;
using DistributionSystem;
using UnityEngine;

namespace DistributionSystem
{
    public class ChangeToEmitter : AProperty
    {
        [SerializeField] ADistributeEmitter _emitter;
        [SerializeField, Range(0f, 1f)] float _pointToChange;
        [SerializeField] bool _receiveRemeins = true;
        [SerializeField, Range(0f, 2f)] float _multiplierForSusceptibility = 1f;
        [SerializeField, Range(0f, 1f)] float _radiance;

        private Coroutine _queue;
        private bool _isEmitter = false;
        private int _numberOfChanges = 0;

        protected override void Awake()
        {
            base.Awake();
            _elementReceiver._onElementPercentChange += OnBurnPercentChange;
        }

        private void OnBurnPercentChange(object sender, EventArgs e)
        {
            if (_numberOfChanges < 0) _numberOfChanges = 0;
            ++_numberOfChanges;
            if (_queue == null)
            {
                _queue = StartCoroutine(CheckPercentChange());
            }
        }

        private IEnumerator CheckPercentChange()
        {
            while (_numberOfChanges-- > 0)
            {
                yield return new WaitForSeconds(Time.fixedDeltaTime * 1.01f);
                if (!_isEmitter && _elementReceiver.ElementPercent >= _pointToChange)
                {
                    _isEmitter = true;
                    ChangeEmitter(true);
                    _elementReceiver.MultiplieSusceptibility(_multiplierForSusceptibility);
                    _elementReceiver.gameObject.SetActive(_receiveRemeins);
                }
                else if (_isEmitter && _elementReceiver.ElementPercent <= _pointToChange)
                {
                    _isEmitter = false;
                    ChangeEmitter(false);
                    _elementReceiver.MultiplieSusceptibility(1f / _multiplierForSusceptibility);
                }
            }
            _queue = null;
        }

        private void ChangeEmitter(bool isAdd)
        {
            _emitter.gameObject.SetActive(true);

            if (isAdd) _emitter.AddType(_type, _radiance);
            else _emitter.RemoveType(_type, _radiance);
        }
    }
}

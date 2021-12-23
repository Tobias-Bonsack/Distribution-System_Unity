using System;
using System.Collections;
using System.Collections.Generic;
using DistributionSystem;
using UnityEngine;

namespace DistributionSystem
{
    public class ReduceElementPercent : AProperty
    {
        [Header("Propertie-Parameter")]
        [SerializeField, Tooltip("Starts the Routine in the Awake-Mehtod")] bool _alwaysReduce = false;
        [SerializeField] IDistribute.ChemistryTypes _weaknessType;
        [SerializeField, Range(0f, 2f), Tooltip("If weakness type is in range, _timeBetweenCooldown get multiplied with")] float _timeMultiplier = 0.5f;
        [SerializeField] float _timeBetweenCooldown = 1f;
        [SerializeField, Range(0f, 1f)] float _rateToCooldown = 0.01f;
        [Header("Grid-Variablen")]
        [SerializeField] bool _ignoreTriggerChange = false;
        private Coroutine _cooldown;

        protected override void Awake()
        {
            base.Awake();
            if (_alwaysReduce)
            {
                _cooldown = StartCoroutine(AllwaysReduceCoroutine());
            }
            else
            {
                if (!_ignoreTriggerChange) _elementReceiver._onActiveTriggerChange += TriggerChange;
                _elementReceiver._onAbleToReceiveChange += AbleToReceiveChange;
            }

            RegisterMethodToElement(_weaknessType, WeaknesEnterTrigger);
            RegisterMethodToElement(_weaknessType, WeaknesExitTrigger);
        }

        private void AbleToReceiveChange(object sender, EventArgs e)
        {
            StartReduce(!_elementReceiver.AbleToReceive);
        }

        private void TriggerChange(object sender, EventArgs e)
        {
            StartReduce(_elementReceiver.ActiveTriggers <= 0);
        }

        private void StartReduce(bool start)
        {
            if (start)
            {
                StopAllCoroutines();
                _cooldown = StartCoroutine(CooldownRoutine());
            }
            else
            {
                if (_cooldown != null) StopCoroutine(_cooldown);
            }
        }

        #region weaknes mehtods
        private void WeaknesEnterTrigger(object sender, DistributeReceiver.OnReceiveElementArgs e)
        {
            if (e._status == IDistributeReceiver.Status.ENTER) _timeBetweenCooldown *= _timeMultiplier;
        }

        private void WeaknesExitTrigger(object sender, DistributeReceiver.OnReceiveElementArgs e)
        {
            if (e._status == IDistributeReceiver.Status.EXIT) _timeBetweenCooldown /= _timeMultiplier;
        }
        #endregion

        IEnumerator CooldownRoutine()
        {
            while (_elementReceiver.ElementPercent > 0f)
            {
                _elementReceiver.ElementPercent -= _rateToCooldown;
                yield return new WaitForSeconds(_timeBetweenCooldown);
            }
            _elementReceiver.ElementPercent = 0f;
        }

        IEnumerator AllwaysReduceCoroutine()
        {
            while (true)
            {
                float newValue = Mathf.Clamp(_elementReceiver.ElementPercent - _rateToCooldown, 0f, 1f);
                _elementReceiver.ElementPercent = newValue;
                yield return new WaitForSeconds(_timeBetweenCooldown);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (!_alwaysReduce)
            {
                if (!_ignoreTriggerChange) _elementReceiver._onActiveTriggerChange -= TriggerChange;
                _elementReceiver._onAbleToReceiveChange -= AbleToReceiveChange;
            }
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DistributionSystem
{
    public class TransformChange : AProperty
    {
        [Header("Propertie-Parameter")]
        [SerializeField] Transform _toChange;
        [SerializeField, Range(0f, 1f)] float _pointToStartChagne = 0f, _pointToEndChange = 1f;
        private bool _knowOrigin = false;

        [Header("Position")]
        [SerializeField] bool _changePosition;
        private Vector3 _originPosition;
        [SerializeField] Vector3 _addPosition;

        [Header("Rotation")]
        [SerializeField] bool _changeRotation;
        private Vector3 _originRotation;
        [SerializeField] Vector3 _addRotation;

        [Header("Scale")]
        [SerializeField] bool _changeScale;
        private Vector3 _originScale;
        [SerializeField] Vector3 _addScale;

        protected override void Awake()
        {
            base.Awake();
            UpdateOriginalTransform();

            _elementReceiver._onElementPercentChange += ChangeScale;
        }

        private void ChangeScale(object sender, EventArgs e)
        {
            if (!_knowOrigin && _elementReceiver.ElementPercent >= _pointToStartChagne)
            {
                _knowOrigin = true;
                UpdateOriginalTransform();
            }

            float range = _pointToEndChange - _pointToStartChagne;
            float fromStart = _elementReceiver.ElementPercent - _pointToStartChagne;
            if (fromStart < range && fromStart >= 0f)
            {
                float changePercent = Mathf.Clamp(fromStart, 0f, range) / range;
                if (_changePosition) _toChange.position = _originPosition + _addPosition * changePercent;
                if (_changeRotation) _toChange.rotation = Quaternion.Euler(_originRotation + _addRotation * changePercent);
                if (_changeScale) _toChange.localScale = _originScale + _addScale * changePercent;
            }
        }

        private void UpdateOriginalTransform()
        {
            _originPosition = _toChange.position;
            _originRotation = _toChange.rotation.eulerAngles;
            _originScale = _toChange.localScale;
        }
    }
}

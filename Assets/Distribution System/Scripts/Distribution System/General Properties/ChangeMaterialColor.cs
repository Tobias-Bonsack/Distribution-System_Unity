using System;
using System.Collections;
using System.Collections.Generic;
using DistributionSystem;
using UnityEngine;

namespace DistributionSystem
{
    public class ChangeMaterialColor : AProperty
    {
        [SerializeField] Renderer _renderer;
        [SerializeField] Color _endColor;
        private Color _materialColor, _stepColor;
        protected override void Awake()
        {
            base.Awake();
            _elementReceiver._onElementPercentChange += OnBurnPercentChange;
            _materialColor = _renderer.material.GetColor("_BaseColor");

            _stepColor = _endColor - _materialColor;
        }
        private void OnBurnPercentChange(object sender, EventArgs e)
        {
            Color color = _materialColor + (_stepColor * _elementReceiver.ElementPercent);
            _renderer.material.SetColor("_BaseColor", color);
        }
    }
}

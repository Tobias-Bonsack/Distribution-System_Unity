using System;
using System.Collections;
using System.Collections.Generic;
using PlayerInteraction;
using UnityEngine;

namespace PlayerInteraction
{
    public class MudPropertyChanger : DistributionSystem.AProperty
    {

        [SerializeField] Mud _mudScript;
        [SerializeField] Mud.MudType _mudType;
        [SerializeField] float _changeMudType;

        protected override void Awake()
        {
            base.Awake();
            _elementReceiver._onElementPercentChange += ElementPercentChange;
        }

        private void ElementPercentChange(object sender, EventArgs e)
        {
            if (_elementReceiver.ElementPercent >= _changeMudType) _mudScript.State = _mudType;
            else _mudScript.State = Mud.MudType.STANDART;
        }
    }
}

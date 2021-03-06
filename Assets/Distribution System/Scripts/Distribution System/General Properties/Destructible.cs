using System;
using System.Collections;
using System.Collections.Generic;
using DistributionSystem;
using UnityEngine;
using UnityEngine.VFX;

namespace DistributionSystem
{

    [RequireComponent(typeof(VisualEffect))]
    public class Destructible : AProperty
    {
        [Header("Propertie-Parameter")]
        [SerializeField] GameObject _objectToDestroy;
        [SerializeField, Range(0f, 1f)] float _pointToDestroy = 1f;
        private VisualEffect _visualEffect;

        protected override void Awake()
        {
            base.Awake();
            _visualEffect = GetComponent<VisualEffect>();
            RegisterMethodToElement(_type, StayTrigger);
        }

        private void StayTrigger(object sender, DistributeReceiver.OnReceiveElementArgs e)
        {
            if (e._status == IDistributeReceiver.Status.STAY && _elementReceiver.ElementPercent >= _pointToDestroy)
            {
                transform.parent = null;
                Destroy(_objectToDestroy);
                StartCoroutine(WaitToDestroy(_visualEffect.GetFloat("Lifetime(to)")));
            }
        }

        IEnumerator WaitToDestroy(float maxLifespan)
        {
            _visualEffect.enabled = true;
            yield return new WaitForSeconds(maxLifespan);
            Destroy(gameObject);
        }
    }
}

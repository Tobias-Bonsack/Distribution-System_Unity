using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DistributionSystem
{
    public class SpawnPrefab : AProperty
    {
        [Header("Propertie-Parameter")]
        [SerializeField] GameObject _preFabToSpawn;
        [SerializeField] GameObject _objectForRespawn;
        [SerializeField, Tooltip("If null -> Destroy GameObject")] Transform _respawn;

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
            {
                Debug.Log("Hit Water");

                Instantiate(_preFabToSpawn, transform.position, new Quaternion(0f, 0f, 0f, 0f));

                if (_respawn == null) Destroy(gameObject);

                _objectForRespawn.GetComponent<Rigidbody>().velocity = Vector3.zero;
                _objectForRespawn.transform.position = _respawn.position;
                _objectForRespawn.transform.rotation = _respawn.rotation;
            }
        }

    }
}

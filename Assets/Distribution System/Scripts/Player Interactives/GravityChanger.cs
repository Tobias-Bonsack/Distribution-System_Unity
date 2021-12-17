using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerInteraction
{
    public class GravityChanger : MonoBehaviour
    {
        [SerializeField] float _gravityChange;
        void OnTriggerEnter(Collider other)
        {
            other.gameObject.GetComponent<IPlayerInteraction>().GravityChange(true, _gravityChange);
        }

        void OnTriggerExit(Collider other)
        {
            other.gameObject.GetComponent<IPlayerInteraction>().GravityChange(false, _gravityChange);

        }
    }
}

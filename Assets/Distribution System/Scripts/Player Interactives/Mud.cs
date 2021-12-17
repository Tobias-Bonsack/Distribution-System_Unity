using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerInteraction
{
    public class Mud : MonoBehaviour
    {
        public enum MudType
        {
            STANDART,
            DRY,
            FROST
        }

        private IPlayerInteraction _playerInteraction;

        public MudType State
        {
            get { return _state; }
            set
            {
                if (_playerInteraction != null) _playerInteraction.MudChanges(false, _values[_state]);
                _state = value;
                if (_playerInteraction != null) _playerInteraction.MudChanges(true, _values[_state]);
            }
        }

        private Dictionary<MudType, float[]> _values = new Dictionary<MudType, float[]>
        { // 0 = jumbHeight, 1 = maxSpeed, 2 = acceleration
            [MudType.STANDART] = new[] { 0.5f, 0.5f, 1f },
            [MudType.DRY] = new[] { 1f, 1f, 1f },
            [MudType.FROST] = new[] { 1f, 1.5f, 1f }
        };

        [SerializeField] MudType _state;



        private void OnTriggerEnter(Collider other)
        {
            _playerInteraction = other.gameObject.GetComponent<IPlayerInteraction>();
            _playerInteraction.MudChanges(true, _values[_state]);
        }

        private void OnTriggerExit(Collider other)
        {
            _playerInteraction.MudChanges(false, _values[_state]);
            _playerInteraction = null;
        }
    }
}

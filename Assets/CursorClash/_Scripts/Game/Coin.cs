using System;
using UnityEngine;

namespace MichiTheDev
{
    public class Coin : MonoBehaviour
    {
        [SerializeField] protected Vector2 _targetPosition;

        protected bool _collecting;
        protected float _currentSpeed = 7;
        
        public void Collect()
        {
            if(_collecting) return;
            
            _collecting = true;
        }

        private void Update()
        {
            if(!_collecting) return;

            Vector3 direction = _targetPosition - (Vector2) transform.position;
            transform.position += _currentSpeed * Time.deltaTime * direction.normalized;
            _currentSpeed += .05f;
        }
    }
}
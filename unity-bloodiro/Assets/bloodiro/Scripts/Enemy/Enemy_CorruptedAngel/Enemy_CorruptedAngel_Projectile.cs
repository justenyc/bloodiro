using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quickjam.Player;

namespace Quickjam.Enemy.CorruptedAngel
{
    public class Enemy_CorruptedAngel_Projectile : MonoBehaviour
    {
        PlayerController _playerTarget;
        [SerializeField] float _moveSpeed = 1;
        [SerializeField] float _trackSpeed = 1;
        [SerializeField] float _lifeTime = 10f;
        bool track = true;
        
        // Start is called before the first frame update
        void Start()
        {
            _playerTarget = FindObjectOfType<PlayerController>();
            Destroy(this.gameObject, _lifeTime);
        }

        public void Initialize(Vector3 startingDirection)
        {
            transform.rotation = Quaternion.Euler(startingDirection);
            track = true;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            TrackTarget();
            transform.position += transform.forward * Time.fixedDeltaTime * _moveSpeed;
        }

        void TrackTarget()
        {
            if (_playerTarget != null && track == true)
            {
                Vector3 modifiedSelf = new Vector3(transform.position.x, _playerTarget.transform.position.y, transform.position.z);
                Vector3 dirFromSelfToTarget = (modifiedSelf - _playerTarget.transform.position).normalized;
                float dot = Vector3.Dot(transform.forward, dirFromSelfToTarget);
                Debug.Log(dot);
                if (dot > 0)
                {
                    track = false;
                }

                Vector3 rot = transform.rotation.eulerAngles;
                Vector3 direction = _playerTarget.transform.position - transform.position;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction.normalized, Vector3.up), _trackSpeed * Time.deltaTime);
            }
        }
    }
}
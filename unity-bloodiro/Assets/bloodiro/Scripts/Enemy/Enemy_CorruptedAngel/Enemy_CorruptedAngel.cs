using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quickjam.Player;

namespace Quickjam.Enemy.CorruptedAngel
{
    [RequireComponent(typeof(CharacterController))]
    public class Enemy_CorruptedAngel : Enemy
    {
        Enemy_CorruptedAngel_State _currentState;
        public string _currentStateName = "";

        public bool _move = false;

        [Space(10)]
        [SerializeField] CharacterController _characterController;
        [SerializeField] AggroDetector _aggroDetector;
        [SerializeField] GameObject _modelPrefab;
        [SerializeField] Animator _animatorController;

        [Space(10)]
        public PatrolStateProperties _patrolStateProperties;
        public AggroStateProperties _aggroStateProperties;
        public AttackStateProperties _attackStateProperties;

        [Space(10)]
        public float moveSpeed = 1;
        public Vector3 _targetPosition;

        #region StateProperties Classes

        [System.Serializable]
        public class PatrolStateProperties
        {
            [Tooltip("How long the Squid Guy waits before determining a new position after it reaches a patrol target position")]
            public float newPositionDelay = 1;
            [Tooltip("How far left and down a new patrol position can be generated")]
            public Vector2 minRange;
            [Tooltip("How far right and up a new patrol position can be generated")]
            public Vector2 maxRange;
        }

        [System.Serializable]
        public class AggroStateProperties
        {
            [Tooltip("How long it takes for the Corrupted Angel to de-aggro")]
            public float exitTime;
            [Tooltip("The origin of the character's line of sight")]
            public Transform characterEyePosition;
        }

        [System.Serializable]
        public class AttackStateProperties
        {
            public float ascentDuration;
            public float ascentSpeed;
            public AnimationCurve ascentCurve;
            [Space(10)]
            public float descentDuration;
            public float descentSpeed;
            public AnimationCurve descentCurve;
            [Space(10)]
            public GameObject projectilePrefab;
        }
        #endregion

        #region Getters
        public GameObject GetModel()
        {
            return _modelPrefab;
        }

        public CharacterController GetCharacterController()
        {
            return _characterController;
        }
        #endregion

        private void Start()
        {
            //To Do
            _currentState = new Enemy_CorruptedAngel_Patrol(this, transform.position);
            _aggroDetector.Initialize(AggroDetectorTriggerEnter, AggroDetectorTriggerStay, AggroDetectorTriggerExit);
        }

        private void FixedUpdate()
        {
            _currentState.StateFixedUpdate();
            Move();
        }

        public float GetMoveSpeed()
        {
            return moveSpeed;
        }

        void Move()
        {
            if (_move && Mathf.Abs(_targetPosition.x - transform.position.x) > 0.1f)
            {
                Vector3 moveVector = _targetPosition - transform.position;
                _characterController.Move(new Vector3(Mathf.Sign(moveVector.x) * moveSpeed * Time.fixedDeltaTime, -9.81f * Time.fixedDeltaTime, 0));
            }

            if (_targetPosition.x - transform.position.x > 0)
            {
                _modelPrefab.transform.rotation = Quaternion.Euler(Vector3.zero);
                _modelPrefab.transform.localScale = new Vector3(_modelPrefab.transform.localScale.x, _modelPrefab.transform.localScale.y, -1);
                return;
            }

            _modelPrefab.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            _modelPrefab.transform.localScale = new Vector3(_modelPrefab.transform.localScale.x, _modelPrefab.transform.localScale.y, 1);
            return;
        }

        public void ResetState()
        {
            Debug.Log("ResetState()");
            //ToDo
            //_currentState = new Enemy_CorruptedAngel_Patrol(this, transform.position);
        }

        public void SetState(Enemy_CorruptedAngel_State newState)
        {
            _currentState = newState;
        }

        public void SetTargetPosition(Vector3 targetPos)
        {
            _targetPosition = targetPos;
        }

        void AggroDetectorTriggerEnter(PlayerController playerController)
        {
            _currentState.AggroDetectorTriggerEnter(playerController);
        }

        void AggroDetectorTriggerStay(PlayerController playerController)
        {
            _currentState.AggroDetectorTriggerStay(playerController);
        }

        void AggroDetectorTriggerExit()
        {
            _currentState.AggroDetectorTriggerExit();
        }
    }
}

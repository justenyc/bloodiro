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
        [SerializeField] LayerMask _sightLayers;

        [Space(10)]
        public PatrolStateProperties _patrolStateProperties;
        public AggroStateProperties _aggroStateProperties;
        public AttackStateProperties _attackStateProperties;

        [Space(10)]
        public float moveSpeed = 1;
        public bool _applyGravity = true;
        public Vector3 _targetPosition;
        public Vector3 moveVector;

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

        //Unused state. You can ignore these
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
            public GameObject projectilePrefab;
            public float projectileDamage = 5;

            [Space(10)]
            [Tooltip("How long it takes for the Corrupted Angel to de-aggro")]
            public float exitTime;

            [Space(10)]
            public float flightSpeed;
            public float flightHeight;
            public float flightTime;
            [Tooltip("How often the Corrupted will fly upwards and out of reach")]
            [Range(1, 100)]
            public float flightFrequency;

            [Space(10)]
            [Tooltip("The minimum delay between each Corrupted Angel attack")]
            [Range(1, 100)]
            public float attackSpeed;
            [Tooltip("A modifier to adjust the delay randomly between attacks. Negative numbers will result in a faster attack speed. Where X is Min and Y is Max")]
            public Vector2 attackSpeedRandomizer;
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

        public LayerMask GetSightLayerMask()
        {
            return _sightLayers;
        }

        public Animator GetAnimator()
        {
            return _animatorController;
        }
        #endregion

        private void Start()
        {
            //To Do
            _currentState = new Enemy_CorruptedAngel_Patrol(this, transform.position);
            _aggroDetector.Initialize(AggroDetectorTriggerEnter, AggroDetectorTriggerStay, AggroDetectorTriggerExit);
            _targetPosition = transform.position;
        }

        private void FixedUpdate()
        {
            _currentState.StateFixedUpdate();
        }

        public float GetMoveSpeed()
        {
            return moveSpeed;
        }

        public void Move()
        {
            if (_move && Vector3.Distance(_targetPosition, transform.position) > 0.1f)
            {
                moveVector = _targetPosition - transform.position;
                moveVector = _applyGravity ? new Vector3(Mathf.Sign(moveVector.x), -9.81f, 0) : moveVector;
                _characterController.Move(moveVector * moveSpeed * Time.fixedDeltaTime);
            }
        }

        public void ResetState()
        {
            _currentState = new Enemy_CorruptedAngel_Patrol(this, transform.position);
        }

        public void SetState(Enemy_CorruptedAngel_State newState)
        {
            _currentState = newState;
        }

        public void SetTargetPosition(Vector3 targetPos)
        {
            _targetPosition = targetPos;
        }

        public void CalculateRotation()
        {
            if (_targetPosition.x - transform.position.x > 0.05f)
            {
                //Face Right
                _modelPrefab.transform.rotation = Quaternion.Euler(Vector3.zero);
                _modelPrefab.transform.localScale = new Vector3(_modelPrefab.transform.localScale.x, _modelPrefab.transform.localScale.y, -1);
                return;
            }

            //Face Left
            _modelPrefab.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            _modelPrefab.transform.localScale = new Vector3(_modelPrefab.transform.localScale.x, _modelPrefab.transform.localScale.y, 1);
            return;
        }

        public void CalculateRotation(float overrideDirection)
        {
            if (overrideDirection > 0)
            {
                //Face Right
                _modelPrefab.transform.rotation = Quaternion.Euler(Vector3.zero);
                _modelPrefab.transform.localScale = new Vector3(_modelPrefab.transform.localScale.x, _modelPrefab.transform.localScale.y, -1);
                return;
            }

            //Face Left
            _modelPrefab.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            _modelPrefab.transform.localScale = new Vector3(_modelPrefab.transform.localScale.x, _modelPrefab.transform.localScale.y, 1);
            return;
        }

        public void ResetAnimationParameters()
        {
            _animatorController.SetBool("Flight", false);
            _animatorController.SetBool("IdleReturn", false);
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

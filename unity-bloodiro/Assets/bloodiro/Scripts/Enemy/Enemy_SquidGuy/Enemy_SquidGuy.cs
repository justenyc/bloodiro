using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quickjam.Player;

namespace Quickjam.Enemy.Squidguy
{
    [RequireComponent(typeof(CharacterController))]
    public class Enemy_SquidGuy : Enemy
    {
        #region State Property Classes
        [System.Serializable]
        public class StalkStateProperties
        {
            [Tooltip("How long it takes for the Squid Guy to de-aggro")]
            public float exitTime;
        }

        [System.Serializable]
        public class PatrolStateProperties
        {
            [Tooltip("How long the Squid Guy waits before determining a new position after it reaches a patrol target position")] 
            public float newPositionDelay = 1;
            [Tooltip("How far left and down a new patrol position can be generated")]
            public Vector2 minRange;
            [Tooltip("How far right and up a new patrol position can be generated")]
            public Vector2 maxRange;
            public LayerMask sightRaycastLayerMask;
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
            public float delayUntilNextAttack = 0;
        }
        #endregion

        Enemy_SquidGuy_State _currentState;

        public string _currentStateName = "";

        [Space(10)]
        [SerializeField] CharacterController _characterController;
        [SerializeField] AggroDetector _aggroDetector;
        [SerializeField] GameObject _modelPrefab;
        [SerializeField] Animator _animatorController;

        [Space(10)]
        public float moveSpeed = 1;
        public Vector3 _targetPosition;

        public StalkStateProperties _stalkStateProperties;
        public PatrolStateProperties _patrolStateProperties;
        public AggroStateProperties _aggroStateProperties;
        public AttackStateProperties _attackStateProperties;

        [Header("Debug")]
        public bool _move = false;
        public bool debugBreak = false;
        [SerializeField] Vector3 moveVector = Vector3.zero;

        #region Getters
        public Animator GetAnimator()
        {
            return _animatorController;
        }
        #endregion

        void OnValidate()
        {
            _characterController = _characterController ?? GetComponent<CharacterController>() ?? null;
            _aggroDetector = _aggroDetector ?? GetComponentInChildren<AggroDetector>() ?? null;
            _animatorController = _animatorController ?? GetComponentInChildren<Animator>() ?? null;
        }

        private void Start()
        {
            _currentState = new Enemy_SquidGuy_Patrol(this, transform.position);
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
                _modelPrefab.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                _modelPrefab.transform.localScale = new Vector3(1, 1, -1);
                return;
            }
            _modelPrefab.transform.rotation = Quaternion.Euler(Vector3.zero);
            _modelPrefab.transform.localScale = new Vector3(1, 1, 1);
        }

        public void ResetState()
        {
            Debug.Log("ResetState()");
            _currentState = new Enemy_SquidGuy_Patrol(this, transform.position);
        }

        public void SetState(Enemy_SquidGuy_State newState)
        {
            _currentState = newState;
            if(debugBreak)
                Debug.Break();
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
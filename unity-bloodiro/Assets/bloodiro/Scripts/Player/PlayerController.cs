using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Quickjam.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        public CharacterController m_characterController;
        public string m_currentStateName;
        public HitBox m_hitbox;
        public Animator m_playerAnimator;
        public Transform m_headPosition;
        public Properties m_properties;
        public PropertyCurves m_propertyCurves;
        public States m_states;
        public Modifiers m_modifiers;
        public List<AttackData> m_attackDataList;
        public Dictionary<string, AttackData> m_attackDataDict { get; private set; } = new Dictionary<string, AttackData>();
        public bool debugBreak = false;
        int frame;
        float time;

        PlayerState m_currentState;

        #region Variable Class Groups
        [System.Serializable]
        public class Properties
        {
            public bool applyGravity = true;
            public float gravity = -9.81f;
            public float terminalVelocity = -2f;
            public float moveSpeed = 10;
            public float parkourDistance = 0.6f;
            public LayerMask groundedLayers;
            public LayerMask parkourLayers;
        }
        [System.Serializable]
        public class PropertyCurves
        {
            public float gravityVertCurveLerpSpeed = 1;
            public AnimationCurve gravityVertCurve;
            public float gravityHorzCurveLerpSpeed = 1;
            public AnimationCurve gravityHorzCurve;
            public float moveAccelerationLerpSpeed = 1;
            public AnimationCurve moveAcceleration;
        }
        [System.Serializable]
        public class States
        {
            public bool grounded;
            public bool nearLadder;
        }
        [System.Serializable]
        public class Modifiers
        {
            public Vector2 moveVector;
            public float parkourTime = 0.5f;
            public float moveAccelerationTime;
            public float gravityVertCurveTime;
            public float gravityHorzCurveTime;
        }
        #endregion

        private void Start()
        {
            m_currentState = new PlayerFreeMovementState(this);
            if (m_attackDataList.Count > 0) { PopulateAttackDataDict(m_attackDataList); }
        }

        void PopulateAttackDataDict(List<AttackData> attackDataList)
        {
            foreach(AttackData ad in m_attackDataList)
            {
                m_attackDataDict.Add(ad.id, ad);
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            CheckForGround();
            m_currentState.StateFixedUpdate();
        }

        #region Events
        void OnControllerColliderHit(ControllerColliderHit hit)
        {
            m_currentState.OnStateControllerColliderHit(hit);
        }

        private void OnTriggerEnter(Collider other)
        {
            switch(other.tag)
            {
                case ("Ladder"):
                    m_states.nearLadder = true;
                    break;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Reset();
        }
        #endregion

        #region Public Functions
        public void Reset() //WIP
        {
            m_states.nearLadder = false;
            m_properties.applyGravity = true;
            m_characterController.enabled = true;
            this.enabled = true;
        }

        public void SetState(PlayerState newState)
        {
            m_currentState = newState;
        }
        #endregion

        void CheckForGround()
        {
            RaycastHit hit;

            if (Physics.SphereCast(transform.position, 0.25f, -Vector3.up, out hit, 0.6f))
            {
                m_states.grounded = true;
                return;
            }
            m_states.grounded = false;
        }

        #region Player Input Listeners
        public void MoveInputListener(InputAction.CallbackContext ctx)
        {
            m_currentState.GetType().GetMethod("StateMoveInputListener")?.Invoke(m_currentState, new object[] { ctx });
        }

        public void SlashInputListener(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                Debug.Log("OnSlash() called");
                m_currentState.GetType().GetMethod("StateSlashInputListener")?.Invoke(m_currentState, new object[] { ctx });
            }
        }

        public void ThrustInputListener(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                Debug.Log("OnThrust() called");
                m_currentState.GetType().GetMethod("StateThrustInputListener")?.Invoke(m_currentState, new object[] { ctx });
            }
        }

        public void DodgeInputListener(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                Debug.Log("OnDodge() called");
                m_currentState.GetType().GetMethod("StateDodgeInputListener")?.Invoke(m_currentState, new object[] { ctx });
            }
        }

        public void InteractInputListener(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                Debug.Log("OnInteract() called");
            }
        }

        public void GunInputListener(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                Debug.Log("OnGun() called");
            }
        }

        public void ParryInputListener(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                Debug.Log("OnParry() called");
            }
        }

        public void HealInputListener(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                Debug.Log("OnHeal() called");
            }
        }
        #endregion

        #region Temp stuff
        IEnumerator DelayMoveForParkourTest(Vector3 basePosition)
        {
            m_characterController.enabled = false;
            this.enabled = false;

            yield return new WaitForSeconds(m_modifiers.parkourTime);

            transform.position = basePosition + Vector3.up;
            Reset();
        }
        #endregion
    }
}

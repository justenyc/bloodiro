using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Quickjam.Player
{
    public class PlayerFreeMovementState : PlayerState
    {
        PlayerController m_manager;

        public PlayerFreeMovementState(PlayerController manager)
        {
            m_stateName = "Free Movement";
            StateStart(manager);
        }

        void StateStart(PlayerController manager)
        {
            m_manager = manager;
            m_manager.m_currentStateName = m_stateName;
            AnimationCheck();
        }

        #region Inherited Overrides
        public override void StateFixedUpdate()
        {
            ApplyGravity();
            Move();
            ParkourCheck();
        }

        public override void OnStateControllerColliderHit(ControllerColliderHit hit)
        {

        }

        public override void OnStateTriggerEnter(Collider other)
        {
            m_manager.m_states.nearLadder = true;
        }

        public override void OnStateTriggerExit(Collider other)
        {
            m_manager.Reset();
        }
        #endregion

        #region Logic
        float ApplyGravity()
        {
            if (!m_manager.m_properties.applyGravity)
            {
                return 0;
            }
            return m_manager.m_properties.gravity;
        }

        void GroundedMovementCurveLerp()
        {
            if (Mathf.Abs(m_manager.m_modifiers.moveVector.x) > 0)
            {
                m_manager.m_modifiers.moveAccelerationTime += Time.fixedDeltaTime * m_manager.m_propertyCurves.moveAccelerationLerpSpeed;
                m_manager.m_modifiers.moveAccelerationTime = Mathf.Clamp01(m_manager.m_modifiers.moveAccelerationTime);
            }
            else
            {
                m_manager.m_modifiers.moveAccelerationTime = 0;
            }
        }

        void GravityCurveLerp()
        {
            if (!m_manager.m_states.grounded)
            {
                m_manager.m_modifiers.gravityVertCurveTime += Time.fixedDeltaTime * m_manager.m_propertyCurves.gravityVertCurveLerpSpeed;
                m_manager.m_modifiers.gravityVertCurveTime = Mathf.Clamp01(m_manager.m_modifiers.gravityVertCurveTime);

                m_manager.m_modifiers.gravityHorzCurveTime += Time.fixedDeltaTime * m_manager.m_propertyCurves.gravityHorzCurveLerpSpeed;
                m_manager.m_modifiers.gravityHorzCurveTime = Mathf.Clamp01(m_manager.m_modifiers.gravityHorzCurveTime);
            }
            else
            {
                m_manager.m_modifiers.gravityVertCurveTime = 0;
                m_manager.m_modifiers.gravityHorzCurveTime = 0;
            }
        }

        void Move()
        {
            GroundedMovementCurveLerp();
            GravityCurveLerp();
            Vector2 moveVector;

            if (m_manager.m_states.grounded)
            {
                moveVector = new Vector2(
                    m_manager.m_modifiers.moveVector.x * m_manager.m_properties.moveSpeed * Time.fixedDeltaTime * m_manager.m_propertyCurves.moveAcceleration.Evaluate(m_manager.m_modifiers.moveAccelerationTime),
                    ApplyGravity() * Time.fixedDeltaTime);
                m_manager.m_characterController.Move(moveVector);
            }
            else
            {
                moveVector = new Vector2(
                    m_manager.m_characterController.velocity.x * m_manager.m_propertyCurves.gravityHorzCurve.Evaluate(m_manager.m_modifiers.gravityHorzCurveTime) * Time.fixedDeltaTime,
                    (ApplyGravity() + m_manager.m_properties.terminalVelocity * m_manager.m_propertyCurves.gravityVertCurve.Evaluate(m_manager.m_modifiers.gravityVertCurveTime)) * Time.fixedDeltaTime);
                m_manager.m_characterController.Move(moveVector);
            }
        }

        void ParkourCheck()
        {
            RaycastHit hit;
            if (m_manager.m_states.grounded)
            {
                //Debug.DrawRay(m_manager.transform.position, m_manager.transform.up * 2, Color.cyan);
                if (Physics.Raycast(m_manager.transform.position, m_manager.transform.right, out hit, m_manager.m_properties.parkourDistance, m_manager.m_properties.parkourLayers))
                {
                    //Debug.Log(hit.collider.name);
                    m_manager.StartCoroutine(DelayMoveForParkourTest(hit.point));
                }

                if (Physics.Raycast(m_manager.transform.position, -m_manager.transform.right, out hit, m_manager.m_properties.parkourDistance, m_manager.m_properties.parkourLayers))
                {
                    //Debug.Log(hit.collider.name);
                    m_manager.StartCoroutine(DelayMoveForParkourTest(hit.point));
                }
            }
        }

        void AnimationCheck()
        {
            if (m_manager.m_modifiers.moveVector.x != 0)
            {
                m_manager.m_playerAnimator.SetBool("Running", true);

                if (m_manager.m_modifiers.moveVector.x > 0)
                {
                    m_manager.m_playerAnimator.transform.localScale = new Vector3(1, 1, -1);
                }
                else if (m_manager.m_modifiers.moveVector.x < 0)
                {
                    m_manager.m_playerAnimator.transform.localScale = new Vector3(1, 1, 1);
                }
            }
            else
            {
                m_manager.m_playerAnimator.SetBool("Running", false);
            }
        }
        #endregion

        #region Input Listeners
        public void StateMoveInputListener(InputAction.CallbackContext ctx)
        {
            m_manager.m_modifiers.moveVector = ctx.ReadValue<Vector2>();
            
            if (m_manager.m_states.nearLadder && m_manager.m_modifiers.moveVector.y != 0)
            {
                m_manager.SetState(new PlayerLadderState(m_manager));
                return;
            }

            AnimationCheck();
        }

        public void StateSlashInputListener(InputAction.CallbackContext ctx)
        {
            m_manager.SetState(new PlayerAttack(m_manager, "Slash"));
        }

        public void StateThrustInputListener(InputAction.CallbackContext ctx)
        {
            m_manager.SetState(new PlayerAttack(m_manager, "Thrust"));
        }

        public void StateDodgeInputListener(InputAction.CallbackContext ctx)
        {
            m_manager.SetState(new PlayerAttack(m_manager, "Dodge"));
        }
        #endregion

        #region Temp Debug
        IEnumerator DelayMoveForParkourTest(Vector3 basePosition)
        {
            m_manager.m_characterController.enabled = false;
            m_manager.enabled = false;

            yield return new WaitForSeconds(m_manager.m_modifiers.parkourTime);

            m_manager.transform.position = basePosition + Vector3.up;
            m_manager.Reset();
        }
        #endregion
    }
}

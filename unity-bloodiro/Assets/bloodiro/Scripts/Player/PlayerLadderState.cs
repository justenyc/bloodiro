using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Quickjam.Player
{
    public class PlayerLadderState : PlayerState
    {
        PlayerController m_manager;

        public PlayerLadderState(PlayerController manager)
        {
            m_stateName = "On Ladder";
            StateStart(manager);
        }

        void StateStart(PlayerController manager)
        {
            m_manager = manager;
            m_manager.m_currentStateName = m_stateName;
            m_manager.m_playerAnimator.SetBool("OnLadder", true);
        }

        public override void StateFixedUpdate()
        {
            Move();
            ParkourCheck();
        }

        void Move()
        {
            Vector2 moveVector;

            moveVector = new Vector2(
                    m_manager.m_modifiers.moveVector.x * m_manager.m_properties.moveSpeed * Time.fixedDeltaTime,
                    m_manager.m_modifiers.moveVector.y * m_manager.m_properties.moveSpeed * Time.fixedDeltaTime);
                m_manager.m_characterController.Move(moveVector);

            if(!m_manager.m_states.nearLadder)
            {
                m_manager.m_playerAnimator.SetBool("OnLadder", false);
                m_manager.SetState(new PlayerFreeMovementState(m_manager));
            }
        }

        void ParkourCheck()
        {
            RaycastHit hit;
            if (m_manager.m_states.nearLadder)
            {
                if (Physics.Raycast(m_manager.transform.position, m_manager.transform.up, out hit, m_manager.m_properties.parkourDistance, m_manager.m_properties.parkourLayers))
                {
                    m_manager.StartCoroutine(DelayMoveForParkourTest(new Vector3(hit.point.x, hit.collider.transform.position.y, hit.point.z)));
                }
            }
        }

        IEnumerator DelayMoveForParkourTest(Vector3 basePosition)
        {
            m_manager.m_characterController.enabled = false;
            m_manager.enabled = false;

            yield return new WaitForSeconds(m_manager.m_modifiers.parkourTime);

            m_manager.transform.position = basePosition + Vector3.up;
            m_manager.Reset();
        }

        public void StateMoveInputListener(InputAction.CallbackContext ctx)
        {
            m_manager.m_modifiers.moveVector = ctx.ReadValue<Vector2>();
        }
    }
}
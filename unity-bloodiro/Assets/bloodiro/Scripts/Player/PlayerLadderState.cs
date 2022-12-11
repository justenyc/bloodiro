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

        public void StateMoveInputListener(InputAction.CallbackContext ctx)
        {
            m_manager.m_modifiers.moveVector = ctx.ReadValue<Vector2>();
        }
    }
}
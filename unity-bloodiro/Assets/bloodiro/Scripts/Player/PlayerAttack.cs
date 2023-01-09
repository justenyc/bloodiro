using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Quickjam.Player
{
    public class PlayerAttack : PlayerState
    {
        PlayerController m_manager;

        public int m_currentCancelPriority { get; private set; }
        public AttackData m_currentAttackData { get; private set; }

        int m_currentFrame = 0;
        int m_lengthInFrames;

        bool cancel = false;

        public PlayerAttack(PlayerController manager, string animationId)
        {
            m_stateName = "Player Attack";
            StateStart(manager, animationId);
        }

        void StateStart(PlayerController manager, string animationId)
        {
            m_manager = manager;
            m_manager.m_currentStateName = m_stateName;

            m_currentAttackData = m_manager.m_attackDataDict[animationId];
            m_currentCancelPriority = m_currentAttackData.cancelPriority;

            if (m_currentAttackData)
            {
                m_lengthInFrames = m_currentAttackData.lengthInFrames;
                m_manager.m_playerAnimator.SetTrigger(m_currentAttackData.id);
            }
        }

        public override void StateFixedUpdate()
        {
            if (m_currentFrame != m_lengthInFrames)
            {
                m_currentFrame++;
                Debug.Log(m_currentFrame);
                //Debug.Break();
                return;
            }
            m_manager.SetState(new PlayerFreeMovementState(m_manager));
        }

        void CanCancel()
        {
            cancel = true;
        }

        public void StateMoveInputListener(InputAction.CallbackContext ctx)
        {
            m_manager.m_modifiers.moveVector = ctx.ReadValue<Vector2>();
        }
    }
}
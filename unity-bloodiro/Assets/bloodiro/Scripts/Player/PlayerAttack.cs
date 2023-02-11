using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Quickjam.Player
{
    public class PlayerAttack : PlayerState
    {
        public class PlayerAttackTimeTracking
        {
            public float moveCurveTime = 0;
            public float gravityCurveTime = 0;
        }

        protected PlayerController m_manager;
        protected PlayerAttackTimeTracking m_timeTracking;

        public int m_currentCancelPriority { get; private set; }
        public AttackData m_currentAttackData { get; private set; }
        int m_currentFrame = 0;
        int m_lengthInFrames;

        bool cancel = false;

        protected System.Action<PlayerController, PlayerAttackTimeTracking> m_attackBehaviour { get; private set; }

        public PlayerAttack(PlayerController manager, string animationId)
        {
            m_stateName = "Player Attack";
            StateStart(manager, animationId);
        }

        protected void StateStart(PlayerController manager, string animationId)
        {
            m_manager = manager;
            m_manager.m_currentStateName = m_stateName;
            m_timeTracking = new PlayerAttackTimeTracking();

            m_currentAttackData = m_manager.m_attackDataDict[animationId];
            m_currentCancelPriority = m_currentAttackData.cancelPriority;
            m_attackBehaviour = m_currentAttackData.GetBehaviour();

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
                m_timeTracking.moveCurveTime += 1 / (Time.fixedDeltaTime * m_lengthInFrames) / 60;

                if (m_attackBehaviour != null) { m_attackBehaviour(m_manager, m_timeTracking); }
                
                if (m_currentAttackData.debugBreak){ Debug.Break(); }
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
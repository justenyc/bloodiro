using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quickjam.Player
{
    public class Thrust : PlayerAttack
    {
        public Thrust(PlayerController manager, string animationId) : base(manager, animationId)
        {
            base.m_manager = manager;
            StateStart(manager, animationId);
        }

        void StateStart(PlayerController manager, string animationId)
        {
            base.StateStart(manager, animationId);
        }
    }
}
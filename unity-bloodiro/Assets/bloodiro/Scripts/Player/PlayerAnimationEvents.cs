using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quickjam.Player
{
    public class PlayerAnimationEvents : MonoBehaviour
    {
        public PlayerController m_playerController;
        
        public void EnableParryBox()
        {
            m_playerController.m_parryBox?.SetActive(true);
        }

        public void DisableParryBox()
        {
            m_playerController.m_parryBox?.SetActive(false);
        }

        public void EnableHurtBox()
        {
            m_playerController.m_hurtBox?.SetActive(true);
        }

        public void DisableHurtBox()
        {
            m_playerController.m_hurtBox?.SetActive(false);
        }

        public void EnableHitBox()
        {
            m_playerController.m_hitbox?.gameObject.SetActive(true);
        }

        public void DisableHitBox()
        {
            m_playerController.m_hitbox?.gameObject.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quickjam.Player;

namespace Quickjam.Enemy.Crow
{
    public abstract class Enemy_Crow_State
    {
        public virtual void StateFixedUpdate()
        {

        }

        public virtual void AggroDetectorTriggerEnter(PlayerController playerController)
        {

        }

        public virtual void AggroDetectorTriggerStay(PlayerController playerController)
        {

        }

        public virtual void AggroDetectorTriggerExit()
        {

        }
    }
}
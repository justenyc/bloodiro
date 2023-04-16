using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quickjam.Player;

namespace Quickjam.Enemy.CorruptedAngel
{
    //This state is unused because the CorruptedAngels attack range is the same as their aggro range.
    public class Enemy_CorruptedAngel_Aggro : Enemy_CorruptedAngel_State
    {
        Enemy_CorruptedAngel _self;

        bool countdown = true;
        float exitTimer;

        public Enemy_CorruptedAngel_Aggro(Enemy_CorruptedAngel self)
        {
            _self = self;
            _self._currentStateName = "Aggro";
            exitTimer = self._aggroStateProperties.exitTime;
        }

        public override void StateFixedUpdate()
        {
            if (exitTimer > 0 && countdown)
            {
                exitTimer -= Time.fixedDeltaTime;
            }
            else if (!countdown)
            {
                exitTimer = _self._aggroStateProperties.exitTime;
            }
            else if (exitTimer <= 0)
            {
                _self.SetState(new Enemy_CorruptedAngel_Patrol(_self, _self.transform.position));
            }
        }

        public override void AggroDetectorTriggerStay(PlayerController playerController)
        {
            Vector3 playerPosition = new Vector3(playerController.m_headPosition.position.x, playerController.m_headPosition.position.y, 0);
            Vector3 mySightOrigin = new Vector3(_self._aggroStateProperties.characterEyePosition.position.x, _self._aggroStateProperties.characterEyePosition.position.y, 0);

            RaycastHit hit;
            if(Physics.Raycast(mySightOrigin, playerPosition - mySightOrigin, out hit, _self.GetSightLayerMask() ,1000))
            {
                Debug.Log(hit.collider.name);
                if(hit.collider.tag.ToLower() == "player")
                {
                    _self.SetState(new Enemy_CorruptedAngel_Attack(_self, "JumpThrow"));
                }
            }

            countdown = false;
        }

        public override void AggroDetectorTriggerExit()
        {
            countdown = true;
        }
    }
}
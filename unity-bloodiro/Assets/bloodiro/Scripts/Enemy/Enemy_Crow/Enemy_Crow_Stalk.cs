using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quickjam.Player;

namespace Quickjam.Enemy.Crow
{
    public class Enemy_Crow_Stalk : Enemy_Crow_State
    {
        Enemy_Crow _self;

        bool countdown = true;
        float exitTimer;

        public Enemy_Crow_Stalk(Enemy_Crow self)
        {
            _self = self;
            _self._currentStateName = "Stalk";
            exitTimer = self._stalkStateProperties.exitTime;
        }

        public override void StateFixedUpdate()
        {
            if(exitTimer > 0 && countdown)
            {
                exitTimer -= Time.fixedDeltaTime;
            }
            else if(!countdown)
            {
                exitTimer = _self._stalkStateProperties.exitTime;
            }
            else if(exitTimer <= 0)
            {
                _self.SetState(new Enemy_Crow_Patrol(_self, _self.transform.position));
            }
        }

        public override void AggroDetectorTriggerStay(PlayerController playerController)
        {
            Vector3 playerPosition = playerController.transform.position;
            _self.SetTargetPosition(new Vector3(playerPosition.x, _self.transform.position.y, 0));
            if(Vector3.Distance(_self.transform.position, playerPosition) < 3f)
            {
                _self.SetState(new Enemy_Crow_Attack(_self));
            }
            countdown = false;
        }

        public override void AggroDetectorTriggerExit()
        {
            countdown = true;
        }
    }
}
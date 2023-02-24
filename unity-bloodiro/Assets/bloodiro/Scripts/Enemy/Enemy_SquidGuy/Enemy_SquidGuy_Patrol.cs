using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quickjam.Player;

namespace Quickjam.Enemy.Squidguy
{
    public class Enemy_SquidGuy_Patrol : Enemy_SquidGuy_State
    {
        Enemy_SquidGuy _self;
        Vector3 _patrolRoot;
        bool _waiting;

        public Enemy_SquidGuy_Patrol(Enemy_SquidGuy self, Vector3 patrolRoot)
        {
            _self = self;
            _self._currentStateName = "Patrol";
            _self._move = true;
            _patrolRoot = patrolRoot;
            Debug.Log($"patrolRoot: {patrolRoot}");
            _self.SetTargetPosition(patrolRoot);
        }

        public override void StateFixedUpdate()
        {
            if (_self.debugBreak)
            {
                Debug.Break();
            }

            if(Mathf.Abs(_self._targetPosition.x - _self.transform.position.x) < 0.1f)
            {
                if (_waiting == false)
                    GenerateRandomPosition();
            }
        }

        public override void AggroDetectorTriggerEnter(PlayerController playerController)
        {
            _self.SetState(new Enemy_SquidGuy_Stalk(_self));
        }

        async Task<Vector3> GenerateRandomPosition()
        {
            if(_self.debugBreak)
                Debug.Break();

            _waiting = true;
            Vector3 newTarget = new Vector3(
                _patrolRoot.x + Random.Range(_self._patrolStateProperties.minRange.x, _self._patrolStateProperties.maxRange.x),
                _self.transform.position.y,
                0);
            
            await DelayAction(1000);
            
            _self.SetTargetPosition(newTarget);
            _waiting = false;
            return newTarget;
        }

        Task DelayAction(int time)
        {
            return Task.Delay(time);
        }
    }
}
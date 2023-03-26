using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quickjam.Player;

namespace Quickjam.Enemy.Wildeman
{
    public class Enemy_Wildeman_Patrol : Enemy_Wildeman_State
    {
        Enemy_Wildeman _self;
        Vector3 _patrolRoot;
        bool _waiting;

        public Enemy_Wildeman_Patrol(Enemy_Wildeman self, Vector3 patrolRoot)
        {
            _self = self;
            _self._currentStateName = "Patrol";
            _self._move = true;
            _patrolRoot = patrolRoot;
            //Debug.Log($"patrolRoot: {patrolRoot}");
            _self.SetTargetPosition(patrolRoot);
        }

        public override void StateFixedUpdate()
        {
            if (_self.debugBreak)
            {
                //Debug.Break();
            }

            if(Mathf.Abs(_self._targetPosition.x - _self.transform.position.x) < 0.1f)
            {
                if (_waiting == false)
                    _self.StartCoroutine(GenerateRandomPosition());
            }
        }

        public override void AggroDetectorTriggerStay(PlayerController playerController)
        {
            _self.StopAllCoroutines();
            _self.SetState(new Enemy_Wildeman_Stalk(_self));
        }

        IEnumerator GenerateRandomPosition()
        {
            _waiting = true;
            Vector3 newTarget = new Vector3(
                _patrolRoot.x + Random.Range(_self._patrolStateProperties.minRange.x, _self._patrolStateProperties.maxRange.x),
                _self.transform.position.y,
                0);
            yield return new WaitForSeconds(_self._patrolStateProperties.newPositionDelay);
            _self.SetTargetPosition(newTarget);
            _waiting = false;
        }
    }
}
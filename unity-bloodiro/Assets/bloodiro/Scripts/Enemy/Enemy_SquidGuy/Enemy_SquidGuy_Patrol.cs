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
            //Debug.Log($"patrolRoot: {patrolRoot}");
            _self.SetTargetPosition(patrolRoot);
        }

        public override void StateFixedUpdate()
        {
            if(Mathf.Abs(_self._targetPosition.x - _self.transform.position.x) < 0.1f)
            {
                if (_waiting == false)
                    _self.StartCoroutine(GenerateRandomPosition());
            }
        }

        public override void AggroDetectorTriggerStay(PlayerController playerController)
        {
            Vector3 playerPosition = new Vector3(playerController.m_headPosition.position.x, playerController.m_headPosition.position.y, 0);
            Vector3 mySightOrigin = new Vector3(_self._aggroStateProperties.characterEyePosition.position.x, _self._aggroStateProperties.characterEyePosition.position.y, 0);

            RaycastHit hit;
            if (Physics.Raycast(mySightOrigin, playerPosition - mySightOrigin, out hit, _self._patrolStateProperties.sightRaycastLayerMask, 1000))
            {
                if(hit.collider.tag == "Player")
                {
                    _self.StopAllCoroutines();
                    _self.SetState(new Enemy_SquidGuy_Stalk(_self));
                }                
            }
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
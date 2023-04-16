using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quickjam.Player;

namespace Quickjam.Enemy.CorruptedAngel
{
    public class Enemy_CorruptedAngel_Patrol : Enemy_CorruptedAngel_State
    {
        Enemy_CorruptedAngel _self;
        Vector3 _patrolRoot;
        bool _waiting;

        public Enemy_CorruptedAngel_Patrol(Enemy_CorruptedAngel self, Vector3 patrolRoot)
        {
            _self = self;
            _self._currentStateName = "Patrol";
            _self._move = true;
            _patrolRoot = patrolRoot;
            AnimationCheck();
        }

        public override void StateFixedUpdate()
        {
            if (Mathf.Abs(_self._targetPosition.x - _self.transform.position.x) < 0.1f)
            {
                _self.GetAnimator().SetFloat("DistanceToTarget", 0);
                if (_waiting == false)
                {
                    _self.StartCoroutine(GenerateRandomPosition());
                }
            }
            _self.Move();
            _self.CalculateRotation();
        }

        public override void AggroDetectorTriggerStay(PlayerController playerController)
        {
            Vector3 playerPosition = new Vector3(playerController.m_headPosition.position.x, playerController.m_headPosition.position.y, 0);
            Vector3 mySightOrigin = new Vector3(_self._aggroStateProperties.characterEyePosition.position.x, _self._aggroStateProperties.characterEyePosition.position.y, 0);

            RaycastHit hit;
            if (Physics.Raycast(mySightOrigin, playerPosition - mySightOrigin, out hit, 1000, _self.GetSightLayerMask()))
            {
                //Debug.Log(hit.collider.name);
                if (hit.collider.tag.ToLower() == "player")
                {
                    _self.StopAllCoroutines();
                    _self.SetState(new Enemy_CorruptedAngel_Attack(_self, "JumpThrow"));
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
            _self.GetAnimator().SetFloat("DistanceToTarget", Vector3.Distance(_self._targetPosition, _self.transform.position));
            _waiting = false;
        }

        void AnimationCheck()
        {
            _self.ResetAnimationParameters();
            _self.GetAnimator().SetBool("IdleReturn", true);
        }
    }
}
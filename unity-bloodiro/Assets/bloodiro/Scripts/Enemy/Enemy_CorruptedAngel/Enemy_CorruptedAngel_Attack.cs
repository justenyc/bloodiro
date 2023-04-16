using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quickjam.Player;

namespace Quickjam.Enemy.CorruptedAngel
{
    public class Enemy_CorruptedAngel_Attack : Enemy_CorruptedAngel_State
    {
        Enemy_CorruptedAngel _self;
        CharacterController _charCon;

        float _exitTime;
        float _exitTimer;

        float _attackSpeed;
        float _attackTimer;

        float _flightFrequency;
        float _flightTimer;

        Vector3 lastPlayerPosition;

        CoroutineTask coroutine;

        public Enemy_CorruptedAngel_Attack(Enemy_CorruptedAngel self, string attackOverride = "")
        {
            string attack = attackOverride == "" ? RandomAttack() : attackOverride;
            _self = self;
            _self._currentStateName = "Attack";
            _charCon = _self.GetCharacterController();
            _self._applyGravity = false;

            _exitTime = _self._attackStateProperties.exitTime;
            _exitTimer = _exitTime;

            _attackSpeed = _self._attackStateProperties.attackSpeed;
            _attackTimer = _attackSpeed;

            _flightFrequency = _self._attackStateProperties.flightFrequency;
            _flightTimer = _flightFrequency;

            Vector3 curTargetPos = _self._targetPosition;
            _self._targetPosition = new Vector3(curTargetPos.x, _self.transform.position.y + 1, curTargetPos.z);
            _self.ResetAnimationParameters();
            _self.GetAnimator().SetBool("Flight", true);
        }

        public override void AggroDetectorTriggerStay(PlayerController playerController)
        {
            Vector3 playerPosition = new Vector3(playerController.m_headPosition.position.x, playerController.m_headPosition.position.y, 0);
            Vector3 mySightOrigin = new Vector3(_self._aggroStateProperties.characterEyePosition.position.x, _self._aggroStateProperties.characterEyePosition.position.y, 0);

            RaycastHit hit;
            if (Physics.Raycast(mySightOrigin, playerPosition - mySightOrigin, out hit, 1000, _self.GetSightLayerMask()))
            {
                if (hit.collider.tag.ToLower() == "player")
                {
                    _exitTimer = _exitTime;
                    lastPlayerPosition = hit.transform.position;
                    return;
                }
            }
        }

        public override void StateFixedUpdate()
        {
            ExitStateCountdown();
            AttackRandomness();
            FlightRandomness();
            _self.Move();
            _self.CalculateRotation(lastPlayerPosition.x - _self.transform.position.x);
        }

        void ExitStateCountdown()
        {
            if (_exitTimer > 0)
            {
                _exitTimer -= Time.fixedDeltaTime;
                return;
            }
            _self._applyGravity = true;
            _self.SetState(new Enemy_CorruptedAngel_Patrol(_self, _self.transform.position));
        }

        void AttackRandomness()
        {
            if (_attackTimer > 0)
            {
                _attackTimer -= Time.fixedDeltaTime;
                return;
            }

            _attackTimer = _attackSpeed + Random.Range(_self._attackStateProperties.attackSpeedRandomizer.x, _self._attackStateProperties.attackSpeedRandomizer.y);
            _self.GetAnimator().SetTrigger("Throw");
        }

        void FlightRandomness()
        {
            if (_flightTimer > 0)
            {
                _flightTimer -= Time.fixedDeltaTime;
                return;
            }

            if (coroutine == null)
            {
                coroutine = new CoroutineTask(FlightRoutine(), _self, () =>
                {
                    coroutine = null;
                    _flightTimer = _flightFrequency;
                });
                return;
            }
        }

        IEnumerator FlightRoutine(float flightTime = 5f)
        {
            flightTime = _self._attackStateProperties.flightTime;
            Vector3 curTarget = _self._targetPosition;
            _self.SetTargetPosition(new Vector3(curTarget.x, curTarget.y + _self._attackStateProperties.flightHeight, curTarget.z));
            yield return new WaitForSeconds(flightTime);
            _self.SetTargetPosition(curTarget);
        }

        string RandomAttack()
        {
            float rng = Random.Range(0, 11);
            if (rng <= 5)
            {
                return "GroundThrow";
            }
            return "JumpThrow";
        }
    }
}
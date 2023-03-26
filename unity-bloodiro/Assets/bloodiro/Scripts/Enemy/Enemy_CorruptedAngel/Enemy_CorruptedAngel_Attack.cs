using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quickjam;

namespace Quickjam.Enemy.CorruptedAngel
{
    public class Enemy_CorruptedAngel_Attack : Enemy_CorruptedAngel_State
    {
        Enemy_CorruptedAngel _self;
        CharacterController _charCon;

        bool _ascend;
        float _ascendDuration;

        public Enemy_CorruptedAngel_Attack(Enemy_CorruptedAngel self, string attackOverride = "")
        {
            string attack = attackOverride == "" ? RandomAttack() : attackOverride;
            _self = self;
            _charCon = _self.GetCharacterController();

            if (attack == "JumpThrow") { 
                _ascend = true;
                _ascendDuration = _self._attackStateProperties.ascentDuration;
            }
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

        public override void StateFixedUpdate()
        {
            if(_ascend)
            {
                if(_ascendDuration >= 0)
                {
                    _ascendDuration -= Time.fixedDeltaTime * _self._attackStateProperties.ascentSpeed;
                }
            }
        }
    }
}
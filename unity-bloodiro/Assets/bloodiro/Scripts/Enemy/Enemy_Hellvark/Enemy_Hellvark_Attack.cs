using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quickjam.Enemy.Hellvark
{
    public class Enemy_Hellvark_Attack : Enemy_Hellvark_State
    {
        Enemy_Hellvark _self;
        string _attackName;
        Animator _selfAnimator;

        public Enemy_Hellvark_Attack(Enemy_Hellvark self, string attackName = "")
        {
            _self = self;
            _selfAnimator = _self.GetAnimator();
            _attackName = attackName == "" ? RandomAttack() : attackName;

            _self._currentStateName = "Attack";
            _self.SetTargetPosition(_self.transform.position);
            _selfAnimator.SetTrigger(_attackName);
        }

        string RandomAttack()
        {
            float rng = Random.Range(0, 11);
            if(rng <= 5)
            {
                return "Bite";
            }
            return "Stab";
        }
    }

}

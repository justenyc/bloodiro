using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quickjam.Enemy.Squidguy
{
    public class Enemy_SquidGuy_Attack : Enemy_SquidGuy_State
    {
        Enemy_SquidGuy _self;
        string _attackName;
        Animator _selfAnimator;

        public Enemy_SquidGuy_Attack(Enemy_SquidGuy self, string attackName)
        {
            _self = self;
            _selfAnimator = _self.GetAnimator();
            _attackName = attackName;

            _self._currentStateName = "Attack";
            _self.SetTargetPosition(_self.transform.position);
            _selfAnimator.SetTrigger(attackName);
        }
    }

}

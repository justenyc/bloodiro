using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Quickjam.Enemy.Crow
{
    public class Enemy_Crow_AnimationEvents : MonoBehaviour
    {
        [SerializeField] Enemy_Crow _self;

        private void OnValidate()
        {
            _self = _self ?? GetComponentInParent<Enemy_Crow>() ?? null;
        }

        public async void ResetStateFromAttack()
        {
            await Delay((int)_self._attackStateProperties.delayUntilNextAttack * 1000);
            _self.SetState(new Enemy_Crow_Patrol(_self, _self.transform.position));
        }

        async Task Delay(int time)
        {
            await Task.Delay(time);
        }
    }
}
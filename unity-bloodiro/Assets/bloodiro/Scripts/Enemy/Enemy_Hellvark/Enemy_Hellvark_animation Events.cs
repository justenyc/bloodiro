using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Quickjam.Enemy.Hellvark
{
    public class Enemy_Hellvark_AnimationEvents : MonoBehaviour
    {
        [SerializeField] Enemy_Hellvark _self;

        private void OnValidate()
        {
            _self = _self ?? GetComponentInParent<Enemy_Hellvark>() ?? null;
        }

        public async void ResetStateFromAttack()
        {
            await Delay((int)_self._attackStateProperties.delayUntilNextAttack * 1000);
            _self.SetState(new Enemy_Hellvark_Patrol(_self, _self.transform.position));
        }

        async Task Delay(int time)
        {
            await Task.Delay(time);
        }
    }
}
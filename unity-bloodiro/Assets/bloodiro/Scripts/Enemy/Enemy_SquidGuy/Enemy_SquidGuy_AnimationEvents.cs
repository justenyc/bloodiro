using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Quickjam.Enemy.Squidguy
{
    public class Enemy_SquidGuy_AnimationEvents : MonoBehaviour
    {
        [SerializeField] Enemy_SquidGuy _self;

        private void OnValidate()
        {
            _self = _self ?? GetComponentInParent<Enemy_SquidGuy>() ?? null;
        }

        public async void ResetState()
        {
            await Delay(1000);
            _self.SetState(new Enemy_SquidGuy_Patrol(_self, _self.transform.position));
        }

        async Task Delay(int time)
        {
            await Task.Delay(time);
        }
    }
}
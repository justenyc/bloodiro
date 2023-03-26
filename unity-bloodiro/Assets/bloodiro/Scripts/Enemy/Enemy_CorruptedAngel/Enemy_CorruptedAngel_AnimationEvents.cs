using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quickjam.Player;

namespace Quickjam.Enemy.CorruptedAngel
{
    public class Enemy_CorruptedAngel_AnimationEvents : MonoBehaviour
    {
        [SerializeField] Enemy_CorruptedAngel _self;

        private void OnValidate()
        {
            _self = _self ?? GetComponentInParent<Enemy_CorruptedAngel>() ?? null;
        }

        public void ThrowProjectile()
        {
            GameObject newProj = Instantiate(_self._attackStateProperties.projectilePrefab);
            PlayerController player = FindObjectOfType<PlayerController>();
            Enemy_CorruptedAngel_Projectile newProjScript = newProj.GetComponent<Enemy_CorruptedAngel_Projectile>();
            newProjScript.Initialize(player.transform.position - transform.parent.transform.position);
        }
    }
}
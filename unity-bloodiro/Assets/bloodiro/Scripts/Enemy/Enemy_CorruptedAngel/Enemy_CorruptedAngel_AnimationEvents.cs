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
            GameObject newProj = Instantiate(_self._attackStateProperties.projectilePrefab, _self.transform.position, Quaternion.identity);
            Enemy_CorruptedAngel_Projectile newProjScript = newProj.GetComponent<Enemy_CorruptedAngel_Projectile>();
            Vector3 modelRot = _self.GetModel().transform.localRotation.eulerAngles;
            Vector3 projStartDir = modelRot.y > 0 ? Vector3.up * 270 : Vector3.up * 90;
            newProjScript.Initialize(projStartDir);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStats : MonoBehaviour
{
    [SerializeField] HitBox hitbox;
    [Tooltip("Affects damage dealt")] [SerializeField] int strength;

    private void Start()
    {
        hitbox.Initialize(this.gameObject, strength);
    }

    private void OnValidate()
    {
        hitbox = hitbox ?? GetComponentInChildren<HitBox>() ?? null;
    }
}

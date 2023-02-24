using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float maxHp;
    [SerializeField] float currentHp;

    public void DealDamage(float damage)
    {
        currentHp -= damage;
        if(currentHp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(this.gameObject);
    }
}
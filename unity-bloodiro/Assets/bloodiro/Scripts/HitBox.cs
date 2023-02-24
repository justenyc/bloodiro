using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    GameObject _originObject;
    float _damage;

    public void Initialize(GameObject originObject, float damage)
    {
        _damage = damage;
        _originObject = originObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        Health health = other.GetComponent<Health>();
        if (health != null && other.gameObject != _originObject)
        {
            health.DealDamage(_damage);
        }
    }
}

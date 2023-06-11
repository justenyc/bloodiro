using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField] bool _destroyOnHit;
    GameObject _originObject;
    float _damage;

    public void Initialize(GameObject originObject, float damage)
    {
        _damage = damage;
        _originObject = originObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != _originObject)
        {
            Health health = other.GetComponent<Health>();
            if (health != null) { health.DealDamage(_damage); }
            if (_destroyOnHit) { Destroy(this.gameObject); }
        }
    }
}
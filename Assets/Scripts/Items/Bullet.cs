using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] int damage;
    [SerializeField] int headShotMult;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;
    void Start()
    {
        Destroy(gameObject, destroyTime);
        rb.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger)
        {
            IDamage damageable = other.GetComponent<IDamage>();

            if (damageable != null)
            {
                if (other.GetComponent<SphereCollider>())
                {
                    damageable.TakeDamage(damage * headShotMult);
                }
                else
                {
                    damageable.TakeDamage(damage);

                }
                Destroy(gameObject);
            }
        }
    }
}

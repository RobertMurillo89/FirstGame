using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CcoProjectile : MonoBehaviour
{
    [SerializeField] GameObject Fly1;
    [SerializeField] GameObject Fly2;
    [SerializeField] GameObject Death1;
    [SerializeField] GameObject Death2;
    [SerializeField] GameObject Death3;
    [SerializeField] GameObject Death4;
    [SerializeField] GameObject Death5;
    [SerializeField] GameObject Death6;
    public bool isFlying = true;
    [SerializeField] Rigidbody rb;
    [SerializeField] int damage;
    [SerializeField] int headShotMult;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;
    void Start()
    {
        Destroy(gameObject, destroyTime);
        rb.velocity = transform.forward * speed;
        Fly1.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamage damageable = other.GetComponent<IDamage>();

        if (!other.isTrigger)
        {
            if (damageable != null && other.gameObject.CompareTag("Player"))
            {
                if (other.GetComponent<SphereCollider>())
                {
                    damageable.TakeDamage(damage * headShotMult);
                }
                else
                {
                    damageable.TakeDamage(damage);

                }
            }
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CcoProjectile : MonoBehaviour
{
    [SerializeField] GameObject Fly1;
    [SerializeField] GameObject Fly2;
    [SerializeField] GameObject Death;
    [SerializeField] Transform Spawnpoint;
    //public bool isFlying = true;
    [SerializeField] Rigidbody rb;
    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;
    void Start()
    {
        Destroy(gameObject, destroyTime);
        rb.velocity = transform.forward * speed;
        Fly1.SetActive(true);
        Invoke("MFly2", 0.057142f);
    }

    void MFly1()
    {
        Fly2.SetActive(false);
        Fly1.SetActive(true);
        Invoke("MFly2", 0.057142f);
    }

    void MFly2()
    {
        Fly1.SetActive(false);
        Fly2.SetActive(true);
        Invoke("MFly1", 0.057142f);
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamage damageable = other.GetComponent<IDamage>();

        if (!other.isTrigger)
        {
            if (damageable != null && other.gameObject.CompareTag("Player"))
            {

                damageable.TakeDamage(damage);
                Instantiate(Death, Spawnpoint.position, transform.rotation);

            }
        }
    }
}


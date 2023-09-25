using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CcoProjectile : MonoBehaviour
{
    [SerializeField] GameObject Fly1;
    [SerializeField] GameObject Fly2;
    [SerializeField] AudioClip WakeUp;
    [SerializeField] AudioClip Hit;
    public AudioSource CacoSounds;
    //public bool isFlying = true;
    [SerializeField] Rigidbody rb;
    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;
    [SerializeField] GameObject Death1;
    [SerializeField] GameObject Death2;
    [SerializeField] GameObject Death3;
    [SerializeField] GameObject Death4;
    [SerializeField] GameObject Death5;
    [SerializeField] GameObject Death6;
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
        MDeath1();
        IDamage damageable = other.GetComponent<IDamage>();
        if (!other.isTrigger)
        {
            if (damageable != null && other.gameObject.CompareTag("Player"))
            {
                damageable.TakeDamage(damage);
            }
        }

    }
    void MDeath1()
    {
        rb.velocity = new Vector3(0,0,0);
        CacoSounds.PlayOneShot(Hit);
        Fly2.SetActive(false);
        Fly1.SetActive(false);
        Death1.SetActive(true);
        Invoke("MDeath3", 0.057142f);
    }

    void MDeath2()
    {
        Death1.SetActive(false);
        Death2.SetActive(true);
        Invoke("MDeath3", 0.057142f);
    }
    void MDeath3()
    {
        Death2.SetActive(false);
        Death3.SetActive(true);
        Invoke("MDeath4", 0.057142f);
    }
    void MDeath4()
    {
        Death3.SetActive(false);
        Death4.SetActive(true);
        Invoke("MDeath5", 0.057142f);
    }
    void MDeath5()
    {
        Death4.SetActive(false);
        Death5.SetActive(true);
        Invoke("MDeath6", 0.057142f);
    }
    void MDeath6()
    {
        Death5.SetActive(false);
        Death6.SetActive(true);
        Invoke("MDeath", 0.057142f);
    }
    void MDeath()
    {
        Destroy(gameObject);
    }
}


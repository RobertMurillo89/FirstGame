using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CacoTestingmeshswop : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;

    [Header("----- Stats -----")]
    [SerializeField] int HP;


    [Header("----- Attack -----")]


    [Header("----- Movement -----")]
    [SerializeField] AudioClip WakeUp;
    [SerializeField] AudioClip Attack;
    [SerializeField] AudioClip Pain;
    [SerializeField] AudioClip Death;
    AudioSource CacoSounds;
    [SerializeField] int Speed;
    [SerializeField] GameObject Idel;
    [SerializeField] GameObject Attack1;
    [SerializeField] GameObject Attack2;
    [SerializeField] GameObject Attack3;
    [SerializeField] GameObject Pain1;
    [SerializeField] GameObject Pain1f;
    [SerializeField] GameObject Pain2;
    [SerializeField] GameObject Pain2f;

    // Start is called before the first frame update
    void Start()
    {
        CacoSounds = GetComponent<AudioSource>();
        GameManager.instance.updateGameGoal(1);
        Attack1.SetActive(false);
        Attack2.SetActive(false);
        Attack3.SetActive(false);
        Pain1.SetActive(false);
        Pain2.SetActive(false);
        Pain1f.SetActive(false);
        Pain2f.SetActive(false);
        Idel.SetActive(true);
        MIdel();
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(GameManager.instance.player.transform.position);
    }
    public void TakeDamage(int amount)
    {
        HP -= amount;
        MPain1();
        //StartCoroutine(flashDamage());

        if (HP <= 0)
        {
            GameManager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
    }
    IEnumerator flashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }

    void MIdel()
    {
        Attack3.SetActive(false);
        Pain1.SetActive(false);
        Pain2f.SetActive(false);
        Idel.SetActive(true);
        //Invoke("MAtt1", 0.142857f);
    }

    void MAtt1()
    {
        Pain1.SetActive(false);
        Idel.SetActive(false);
        Attack1.SetActive(true);
        Invoke("MAtt2", 0.142857f);
    }

    void MAtt2()
    {
        Attack1.SetActive(false);
        Attack2.SetActive(true);
        Invoke("MAtt3", 0.142857f);
    }
    void MAtt3()
    {
        Attack2.SetActive(false);
        Attack3.SetActive(true);
        Invoke("MPain1", 0.028571f);
    }

    void MPain1()
    {
        CacoSounds.PlayOneShot(Pain);
        Idel.SetActive(false);
        Attack1.SetActive(false);
        Attack2.SetActive(false);
        Attack3.SetActive(false);
        Pain1.SetActive(false);
        Pain2.SetActive(false);
        Pain1f.SetActive(false);
        Pain2f.SetActive(false);
        Pain1.SetActive(true);
        Invoke("MPain1f", 0.028571f);
    }

    void MPain1f()
    {
        Pain2f.SetActive(false);
        Pain1.SetActive(false);
        Pain1f.SetActive(true);
        Invoke("MPain2", 0.028571f);
    }

    void MPain2()
    {
        Pain1f.SetActive(false);
        Pain2.SetActive(true);
        Invoke("MPain2f", 0.028571f);
    }
    void MPain2f()
    {
        Pain2.SetActive(false);
        Pain2f.SetActive(true);
        Invoke("MPainend", 0.028571f);
    }
    void MPainend()
    {
        Pain2f.SetActive(false);
        Pain1.SetActive(true);
        Invoke("MIdel", 0.057142f);
    }

}

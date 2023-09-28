using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CacoTestingmeshswop : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;


    [Header("----- Stats -----")]
    [SerializeField] int HP;

    [Header("----- Attack -----")]
    [SerializeField] float shootRate;
    bool isShooting;

    [Header("----- Movement -----")]
    [SerializeField] int Speed;
    [SerializeField] int playerFaceSpeed; // this is to face the player smoothly instead of snapping.
    Vector3 playerDir;
    bool playerInRange;

    [SerializeField] AudioClip WakeUp;
    [SerializeField] AudioClip Attack;
    [SerializeField] AudioClip Pain;
    [SerializeField] AudioClip Death;
    public AudioSource CacoSounds;
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
        MIdol();
    }

    void Update()
    {
        if (playerInRange)
        {
            playerDir = GameManager.instance.player.transform.position - transform.position;

            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!isShooting)
                {
                isShooting = true;
                MAtt1();
                }
            }
            agent.SetDestination(GameManager.instance.player.transform.position);
        }
    }
    IEnumerator shoot()
    {

        Instantiate(bullet, shootPos.position, transform.rotation);
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    void facePlayer()
    {
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * playerFaceSpeed);
    }
    public void TakeDamage(int amount)
    {
        HP -= amount;
        MPain1();

        //PlayPainEmote();
        if (HP <= 0)
        {
            GameManager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
    public void PlayPainEmote()
    {
        MPain1();
    }
    void MIdol()
    {
        Attack3.SetActive(false);
        Pain1.SetActive(false);
        Pain2f.SetActive(false);
        Idel.SetActive(true);
        //Invoke("MAtt1", 0.142857f);
    }

    void MAtt1()
    {
        facePlayer();
        Pain1.SetActive(false);
        Idel.SetActive(false);
        Attack1.SetActive(true);
        Invoke("MAtt2", 0.142857f);
    }

    void MAtt2()
    {
        facePlayer();
        Attack1.SetActive(false);
        Attack2.SetActive(true);
        Invoke("MAtt3", 0.142857f);
    }
    void MAtt3()
    {
        //facePlayer();
        StartCoroutine(shoot());
        Attack2.SetActive(false);
        Attack3.SetActive(true);
        Invoke("MIdol", 0.142857f);
        //isShooting = false;
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
        Invoke("MIdol", 0.057142f);
    }

}

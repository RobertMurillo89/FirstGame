using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
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

    // Start is called before the first frame update
    void Start()
    {
        //report to the game manager that you exist.
        GameManager.instance.updateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange)
        {
            playerDir = GameManager.instance.player.transform.position - transform.position;

            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                facePlayer();
                if (!isShooting)
                {
                    StartCoroutine(shoot());

                }
            }
            agent.SetDestination(GameManager.instance.player.transform.position);
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;
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


        //if(gameObject.CompareTag("Player"))
        //{
            HP -= amount;
        //}
        StartCoroutine(flashDamage());

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

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
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
}

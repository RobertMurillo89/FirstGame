using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] Transform headPos;

    [Header("----- Enemy Stats -----")]
    [Range(1, 10)][SerializeField] int HP;
    [Range(1, 25)][SerializeField] int speed;
    [Range(1, 10)][SerializeField] int playerFaceSpeed;
    [Range(45, 360)][SerializeField] int viewAngle;
    [Range(1, 150)][SerializeField] int roamDist;
    [Range(0, 5)][SerializeField] int roamTimer;
    [SerializeField] int animChangeSpeed;

    [Header("----- Gun Stats -----")]
    [Range(0.1f, 3)][SerializeField] float shootRate;
    [Range(1, 360)][SerializeField] int shootAngle;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;
    [SerializeField] Light flashLight;

    //public spawner whereISpawned;
    //public spawnWave whereISpawnedWave;

    Vector3 playerDir;
    bool isShooting;
    bool playerInRange;
    float angleToPlayer;
    Vector3 startingPos;
    bool destinationChosen;
    float stoppingDistOrig;

    void Start()
    {
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
    }

    void Update()
    {
        if (agent.isActiveAndEnabled)
        {
            float agentVel = agent.velocity.normalized.magnitude;
            anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agentVel, Time.deltaTime * animChangeSpeed));

            if (playerInRange && !canSeePlayer())
            {
                StartCoroutine(roam());
            }

        }
    }

    bool canSeePlayer()
    {
        agent.stoppingDistance = stoppingDistOrig;
        playerDir = GameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward);

        Debug.Log(angleToPlayer);
        Debug.DrawRay(headPos.position, playerDir);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewAngle)
            {
                agent.SetDestination(GameManager.instance.player.transform.position);

                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    facePlayer();
                }

                if (!isShooting && angleToPlayer <= shootAngle)
                {
                    StartCoroutine(shoot());
                }

                flashLight.color = Color.red;
                return true;
            }
        }
        agent.stoppingDistance = 0;
        flashLight.color = Color.white;
        return false;

    }

    IEnumerator roam()
    {
        if (agent.remainingDistance < 0.05f && !destinationChosen)
        {
            destinationChosen = true;
            agent.stoppingDistance = 0;
            yield return new WaitForSeconds(roamTimer);

            Vector3 randomPos = UnityEngine.Random.insideUnitSphere * roamDist;
            randomPos += startingPos;

            NavMeshHit hit;
            NavMesh.SamplePosition(randomPos, out hit, roamDist, 1);
            agent.SetDestination(hit.position);

            destinationChosen = false;
        }
    }

    void facePlayer()
    {
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * playerFaceSpeed);
    }

    IEnumerator shoot()
    {
        isShooting = true;

        anim.SetTrigger("Shoot");
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    public void createBullet()
    {
        Instantiate(bullet, shootPos.position, transform.rotation);
    }
    public void takeDamage(int amount)
    {
        HP -= amount;

        if (HP <= 0)
        {
            StopAllCoroutines();
            anim.SetBool("Dead", true);
            agent.enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<SphereCollider>().enabled = false;
            //gameManager.instance.updateGameGoal(-1);
            //whereISpawnedWave.updateEnemyNumber(); //this brakes the old spawner.
            //Destroy(gameObject);

        }
        else
        {
            anim.SetTrigger("Damage");
            agent.SetDestination(GameManager.instance.player.transform.position);
            StartCoroutine(flashDamage());
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
            agent.stoppingDistance = 0;
        }
    }
}

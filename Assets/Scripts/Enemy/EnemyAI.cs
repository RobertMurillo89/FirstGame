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
    [SerializeField] Transform headPos;

    [Header("----- Stats -----")]
    [SerializeField] int HP;


    [Header("----- Attack -----")]
    [SerializeField] float shootRate;
    bool isShooting;


    [Header("----- Movement -----")]
    [SerializeField] int Speed;
    [SerializeField] int playerFaceSpeed; // this is to face the player smoothly instead of snapping.
    [SerializeField] int viewAngle;
    Vector3 playerDir;
    bool playerInRange;
    float angleToPlayer;

    // Start is called before the first frame update
    void Start()
    {
        //report to the game manager that you exist.
        GameManager.instance.updateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && canSeePlayer())
        {

        }
    }

    IEnumerator shoot()
    {
        isShooting = true;
        Instantiate(bullet, shootPos.position, headPos.rotation);
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    bool canSeePlayer()
    {
        playerDir = GameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);

        Debug.Log(angleToPlayer);
        Debug.DrawRay(headPos.position,playerDir);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position,playerDir, out hit))
        {
            //enemy can see player
            if(hit.collider.CompareTag("Player") && angleToPlayer <= viewAngle)
            {
                //RJM just in case, need to make a seperate shoot angle from view angle
                agent.SetDestination(GameManager.instance.player.transform.position);
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    facePlayer();
                }
                if (!isShooting)
                {
                    StartCoroutine(shoot());
                }
                return true;
            }
        }
        return false;
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
        Debug.Log(amount);
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

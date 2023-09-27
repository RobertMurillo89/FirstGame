using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Unity.VisualScripting.Member;

public class EnemyAI : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;
    [SerializeField] Animator anim;

    [Header("----- Stats -----")]
    [SerializeField] int HP;
    [SerializeField] Light flashLight;
    


    [Header("----- Attack -----")]
    [Range(0.01f, 3)][SerializeField] float shootRate;
    [Range(1, 360)][SerializeField] int shootAngle;
    bool isShooting;


    [Header("----- Movement -----")]
    [SerializeField] int Speed;
    [Range(1,10)][SerializeField] int playerFaceSpeed; // this is to face the player smoothly instead of snapping.
    [Range(45, 360)][SerializeField] int viewAngle;
    [Range(1, 150)][SerializeField] int roamDist;
    [Range(0, 20)][SerializeField] int roamTimer;
    [SerializeField] int animChangeSpeed;

    Vector3 playerDir;
    bool playerInRange;
    float angleToPlayer;
    Vector3 startingPos;
    bool destinationChosen;
    float stoppingDistOrig;

    [Header("Audio")]
    public AudioSource LocomotionSource;
    public AudioSource shootSource;
    public AudioSource EmoteSource;
    public AudioSource hitMarker;
    public AudioClip[] FootStepSFX;
    public AudioClip[] shootClipsSFX;
    public AudioClip[] hitMarkerClip;
    public AudioClip[] deathClips;

    // Start is called before the first frame update
    void Start()
    {
        //report to the game manager that you exist.
        GameManager.instance.updateGameGoal(1);
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
    }

    // Update is called once per frame
    void Update()
    {
        float agentVelo = agent.velocity.normalized.magnitude;

        anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agentVelo, Time.deltaTime * animChangeSpeed));//bigger number fast snap

        if (playerInRange && canSeePlayer())
        {
            StartCoroutine(Roam());
        }
        else if (agent.destination != GameManager.instance.player.transform.position)
        {
            StartCoroutine(Roam());
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;
        Instantiate(bullet, shootPos.position, shootPos.rotation);
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    bool canSeePlayer()
    {
        agent.stoppingDistance = stoppingDistOrig;
        playerDir = GameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x,0, playerDir.z), transform.forward);

        //Debug.Log(angleToPlayer);
        //Debug.DrawRay(headPos.position,playerDir);

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

    IEnumerator Roam()
    {
        if(agent.remainingDistance < 0.05 && !destinationChosen)
        {
            destinationChosen = true;
            agent.stoppingDistance = 0;
            yield return new WaitForSeconds(roamTimer);

            Vector3 randomPos = Random.insideUnitSphere * roamDist;
            randomPos += startingPos;

            //prevent hitting outside of navmesh
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

    public void TakeDamage(int amount)
    {
        HP -= amount;
        agent.SetDestination(GameManager.instance.player.transform.position);
        AudioFunctionalities.PlayRandomClip(EmoteSource, hitMarkerClip);
        StartCoroutine(flashDamage());
        if (HP <= 0)
        {
            AudioFunctionalities.PlayRandomClip(EmoteSource, deathClips);// does not work not sure why
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
            agent.stoppingDistance = 0;
        }
    }
    public void PlayEnemyFootsteps()
    {
        AudioFunctionalities.PlayRandomClip(LocomotionSource, FootStepSFX);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderItem : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    //[SerializeField] int Speed;
    [Range(45, 360)][SerializeField] int viewAngle;
    [Range(1, 150)][SerializeField] int roamDist;
    [Range(0, 20)][SerializeField] int roamTimer;
    [SerializeField] int animChangeSpeed;
    Vector3 startingPos;
    bool destinationChosen;
    float stoppingDistOrig;
    //public AudioSource LocomotionSource;
    public AudioSource EmoteSource;
    //public AudioClip[] FootStepSFX;
    public AudioClip[] EmoteClips;
    public AudioClip[] DeathClips;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
    }

    // Update is called once per frame
    void Update()
    {
        float agentVelo = agent.velocity.normalized.magnitude;

        //anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agentVelo, Time.deltaTime * animChangeSpeed));//bigger number fast snap
        StartCoroutine(Roam());
    }

    IEnumerator Roam()
    {
        if (agent.remainingDistance < 0.05 && !destinationChosen)
        {
            destinationChosen = true;
            agent.stoppingDistance = 0;
            //StartCoroutine(Emote());

                AudioFunctionalities.PlayRandomClip(EmoteSource, EmoteClips);

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.playerScript.JumpMax++;
            GameManager.instance.playerScript.jumpCooldown = 0;
            AudioFunctionalities.PlayRandomClip(PlayASound.instance.SFXSource, DeathClips);
            Destroy(gameObject);
        }
    }
    //IEnumerator Emote()
    //{
    //    AudioFunctionalities.PlayRandomClip(EmoteSource, EmoteClips);
    //    yield return new WaitForSeconds(2f);

    //}
}

using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

public class WanderItem : MonoBehaviour
{

    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [Range(1, 150)][SerializeField] int roamDist;
    [Range(0, 20)][SerializeField] int roamTimer;
    Vector3 startingPos;
    bool destinationChosen;
    float stoppingDistOrig;
    public AudioSource EmoteSource;
    public AudioClip[] EmoteClips;
    public AudioClip[] DeathClips;
    public Animator textAnimator;
    private float nextEmoteTime = 0f;

    void Start()
    {
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
        SetNextEmoteTime();
    }

    void Update()
    {
        if (agent.remainingDistance < 0.05 && !destinationChosen)
        {
            SetNewDestination();
        }

        if (Time.time > nextEmoteTime)
        {
            PlayEmote();
            SetNextEmoteTime();
        }
    }
    void SetNewDestination()
    {
        destinationChosen = true;
        agent.stoppingDistance = 0;

        Vector3 randomPos = Random.insideUnitSphere * roamDist;
        randomPos += startingPos;

        //prevent hitting outside of navmesh
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPos, out hit, roamDist, 1);
        agent.SetDestination(hit.position);

        destinationChosen = false;
    }

    void PlayEmote()
    {
        AudioFunctionalities.PlayRandomClip(EmoteSource, EmoteClips);
    }

    void SetNextEmoteTime()
    {
        nextEmoteTime = Time.time + Random.Range(2, 10);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.playerScript.JumpMax++;
            GameManager.instance.playerScript.jumpCooldown = 0;
            AudioFunctionalities.PlayRandomClip(PlayASound.instance.SFXSource, DeathClips);
            GameManager.instance.TextAnimation(textAnimator);
            Destroy(gameObject);
        }
    }

}

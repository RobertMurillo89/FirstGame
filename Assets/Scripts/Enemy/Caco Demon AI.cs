using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Caco_Demon_AI : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;

    [Header("----- Stats -----")]
    [SerializeField] int HP;


    [Header("----- Attack -----")]


    [Header("----- Movement -----")]
    [SerializeField] int Speed;
    [SerializeField] int playerFaceSpeed; // this is to face the player smoothly instead of snapping.
    Vector3 playerDir;

    [Header("----- Emotes -----")]
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
        //report to the game manager that you exist.
        GameManager.instance.updateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        playerDir = GameManager.instance.player.transform.position - transform.position;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            facePlayer();
        }
        agent.SetDestination(GameManager.instance.player.transform.position);
    }

    void facePlayer()
    {
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * playerFaceSpeed);
    }

    public void TakeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(flashDamage());

        if (HP <= 0)
        {
            GameManager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
    }


    //RJM Alex you might want to disable this co-routine in the take damage function above this. 
    IEnumerator flashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }
}

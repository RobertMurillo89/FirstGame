using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject ObjectToSpawn;
    [SerializeField] Transform[] SpawnPosList;
    [SerializeField] int NumberToSpawn;
    [SerializeField] float TimeBetweenSpawns;

    int numberSpawned;
    bool isSpawning;
    bool startSpawning;

    void Start()
    {
        if ( ObjectToSpawn.CompareTag("Enemy"))
        {
            GameManager.instance.updateGameGoal(NumberToSpawn);
        }
    }


    void Update()
    {
        if (startSpawning && !isSpawning && numberSpawned < NumberToSpawn)
        {
            StartCoroutine(Spawn());
        }
    }

    public IEnumerator Spawn()
    {
        isSpawning = true;

        Instantiate(ObjectToSpawn, SpawnPosList[Random.Range(0, SpawnPosList.Length)].position, ObjectToSpawn.transform.rotation);
        numberSpawned++;

        yield return new WaitForSeconds(TimeBetweenSpawns);
        isSpawning = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            startSpawning = true;
        }
    }
}

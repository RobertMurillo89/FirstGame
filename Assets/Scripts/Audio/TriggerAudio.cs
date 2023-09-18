using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAudio : MonoBehaviour
{
    public AudioSource TriggerSource;
    public AudioClip TriggerClip;
    public GameObject Player;

    void Start()
    {
        TriggerSource.clip = TriggerClip;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player)
        {
            TriggerSource.Play();
            gameObject.SetActive(false); // makes a single trigger then deactivates this script
        }
    }
}

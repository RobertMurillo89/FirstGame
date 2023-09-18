using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePad : MonoBehaviour
{
    public Animator PressurePadAnim;
    public GameObject PlayerF, PlayerT; //player first and third person view
    public AudioSource PressurePadSource;
    public AudioClip[] TriggeredClips;
    public AudioClip[] DeactivateClips;
 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == PlayerF || other.gameObject == PlayerT)
        {
            PressurePadAnim.SetBool("IsTriggered", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == PlayerF || other.gameObject == PlayerT)
        {
            PressurePadAnim.SetBool("IsTriggered", false);
        }
    }

    public void TriggeredAudio()
    {
        AudioFunctionalities.PlayRandomClip(PressurePadSource, TriggeredClips);
    }

    void DeactivateAuido()
    {
        AudioFunctionalities.PlayRandomClip(PressurePadSource, DeactivateClips);
    }
}

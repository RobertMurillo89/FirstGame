using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class SignLogic : MonoBehaviour
{
    public Animator PressurePadAnim;
    public AudioSource SignSource;
    public AudioClip signSqueakOpen;
    public AudioClip signSqueakClose;


    public void SqueakOpen()
    {
        SignSqueakOpen(SignSource, signSqueakOpen);
    }
    public void SqueakClose()
    {
        SignSqueakClose(SignSource, signSqueakClose);
    }
    public void SignSqueakOpen(AudioSource source, AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }
    public void SignSqueakClose(AudioSource source, AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }
}

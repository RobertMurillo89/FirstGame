using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayASound : MonoBehaviour
{
    public static PlayASound instance;
    public AudioSource MusicSource;
    public AudioClip MusicClip;
    public AudioSource AmbiSource;
    public AudioClip AmbiClip;
    public AudioSource SFXSource;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlaySound(MusicSource, MusicClip);
        PlaySound(AmbiSource, AmbiClip);

        //AudioFunctionalities.PlayRandomClip();

    }

    void PlaySound(AudioSource source, AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }
}

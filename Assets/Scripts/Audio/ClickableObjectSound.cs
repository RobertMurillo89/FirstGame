using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableObjectSound : MonoBehaviour
{
    public AudioSource ClickableSource;
    public AudioClip[] ClickableClip;
    public Vector2 PitchRange;
    private void OnMouseDown()
    {
        if (!ClickableSource.isPlaying)
        {
            AudioFunctionalities.PlayRandomClip(ClickableSource, ClickableClip, PitchRange.x, PitchRange.y);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTagger : MonoBehaviour
{
    public string bulletTag;

    public void SetTag(string tag)
    {
        bulletTag = tag;
        gameObject.tag = tag;
    }    
}

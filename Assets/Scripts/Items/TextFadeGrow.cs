using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class TextFadeGrow
{ 
    public static void PlayAnimation(Animator textAnimator)
    {
        if (textAnimator != null)
            textAnimator.SetTrigger("TextFadeGrow");
    }
}

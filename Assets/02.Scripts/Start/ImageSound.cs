using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageSound : MonoBehaviour
{
    public void DoSound()
    {
        SoundManager.instance.BubbleSFX.PlayOneShot(SoundManager.instance.BubbleSFX.clip);
        SoundManager.instance.BubbleSFX.pitch += 0.07f;
    }
}

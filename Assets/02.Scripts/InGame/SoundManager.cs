using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource MainBGM1;
    public AudioSource MainBGM2;
    public AudioSource MainBGM3;
    public AudioSource MainBGM4;
    public AudioSource MainBGM5;

    public AudioSource ClearSFX;

    public AudioSource BubbleSFX;
    public AudioSource[] BallHitSounds;
    public AudioSource PaintSound;

    private void Awake()
    {
        instance = this;
    }

    public void InitializeBubble()
    {
        BubbleSFX.pitch = 1;
    }

    public void PlayTargetSound(AudioSource AS)
    {
        AS.PlayOneShot(AS.clip);
;   }


}

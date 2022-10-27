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
    public AudioSource[]BallHitSounds;
    public AudioSource PaintSound;
    public AudioSource PortalSFX;
    public AudioSource KeySFX;
    public AudioSource BallWaveSFX;

    public bool isEnable_BallHitSound;

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
        if (!isEnable_BallHitSound)
        {
            for (int i = 0; i < BallHitSounds.Length; i++)
            {
                if (AS == BallHitSounds[i])
                {
                    return;
                }
            }
        }
        
        AS.PlayOneShot(AS.clip);
;   }


}

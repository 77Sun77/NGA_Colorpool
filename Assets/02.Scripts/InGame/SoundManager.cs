using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum State { InGame, Start };
    public State ManagerState;

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
    public AudioSource UI_StarSFX;

    public bool isEnable_BallHitSound;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            if(ManagerState == State.InGame) MainBGM5.Play();
        }
        
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

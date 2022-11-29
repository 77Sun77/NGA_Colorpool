using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum State { InGame, Start, Lobby };
    public State ManagerState;
    bool isPlayingGame;
    bool isPlayingLobby;


    public static SoundManager instance;

    public AudioSource MainBGM1;
    public AudioSource MainBGM2;
    public AudioSource MainBGM3;
    public AudioSource MainBGM4;
    public AudioSource MainBGM5;
    public AudioSource MainBGM6;

    public AudioSource ClearSFX;

    public AudioSource BubbleSFX;
    public AudioSource[] BallHitSounds;
    public AudioSource PaintSound;
    public AudioSource PortalSFX;
    public AudioSource KeySFX;
    public AudioSource BallWaveSFX;
    public AudioSource ButtonClickSFX;

    public bool isEnable_BallHitSound;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            ManagerState = State.Lobby;
            if (ManagerState == State.Lobby) MainBGM6.Play();
        }

    }

    private void Update()
    {
        if (ManagerState == State.InGame)
        {
            if (isPlayingGame)
                return;
            isPlayingGame = true;
            isPlayingLobby = false;

            PlayTargetSound(MainBGM5);
            MainBGM6.Stop();
        }
        else if (ManagerState == State.Lobby)
        {
            if (isPlayingLobby)
                return;
            isPlayingGame = false;
            isPlayingLobby = true;

            MainBGM5.Stop();
            PlayTargetSound(MainBGM6);
        }
    }

    public void DoSoundFade(AsyncOperation oper)
    {
        //StartCoroutine(DoSoundFade_Cor(oper));
    }


    IEnumerator DoSoundFade_Cor(AsyncOperation oper)
    {
        AudioSource FadeInSound = null;
        AudioSource FadeOutSound = null;
        if (isPlayingGame)
        {
            FadeInSound = MainBGM5;
            FadeOutSound = MainBGM6;
        }
        else if (isPlayingLobby)
        {
            FadeInSound = MainBGM6;
            FadeOutSound = MainBGM5;
        }

        while (!oper.isDone)
        {
            yield return new WaitForFixedUpdate();
            Debug.Log(oper.progress);
            float originVol = FadeInSound.volume;
            FadeInSound.volume = originVol * oper.progress;

            float originVol2 = FadeOutSound.volume;
            FadeOutSound.volume = originVol2 * (1 - oper.progress);
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
                if (AS == BallHitSounds[i] || AS == BubbleSFX)
                {
                    return;
                }
            }
        }

        AS.PlayOneShot(AS.clip);

    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    public GameObject VideoWindow;
    public GameObject btns;
    public GameObject menuBtn;

    public VideoClip[] videos;
    int tutorialCount, curCount,count;

    VideoPlayer vp;

    Coroutine delayTime;

    bool uiOpen, isPlay;

    void Start()
    {
        instance = this;
        uiOpen = false;
        isPlay = false;
        vp = transform.GetChild(0).GetComponent<VideoPlayer>();
        if (PlayerPrefs.HasKey("Tutorial"))
        {
            tutorialCount = PlayerPrefs.GetInt("Tutorial");
        }
        else
        {
            tutorialCount = 0;
            PlayerPrefs.SetInt("Tutorial", 0);
        }

        if (GameManager.stageLV+1 == 9 && tutorialCount == 0)
        {
            tutorialCount = 1;
            delayTime = StartCoroutine(DelayTime(videos[0], isFirst: true));
        }
        else if (GameManager.stageLV+1 == 17 && tutorialCount == 1)
        {
            tutorialCount = 2;
            delayTime = StartCoroutine(DelayTime(videos[1], isFirst:true));
        }
        else if (GameManager.stageLV + 1 == 23 && tutorialCount == 2)
        {
            tutorialCount = 4;
            delayTime = StartCoroutine(DelayTime(videos[2], videos[3], true));
        }
        else if (GameManager.stageLV + 1 == 27 && tutorialCount == 4)
        {
            tutorialCount = 6;
            delayTime = StartCoroutine(DelayTime(videos[4], videos[5], true));
        }
        else if (GameManager.stageLV + 1 == 32 && tutorialCount == 6)
        {
            tutorialCount = 8;
            delayTime = StartCoroutine(DelayTime(videos[6], videos[7], true));
        }
        else if (GameManager.stageLV + 1 == 45 && tutorialCount == 8)
        {
            tutorialCount = 9;
            delayTime = StartCoroutine(DelayTime(videos[8], isFirst: true));
        }
        else if (GameManager.stageLV + 1 == 51 && tutorialCount == 9)
        {
            tutorialCount = 10;
            delayTime = StartCoroutine(DelayTime(videos[9], isFirst: true));
        }
        PlayerPrefs.SetInt("Tutorial", tutorialCount);

        curCount = 0;
        switch (tutorialCount)
        {
            case 1:
                curCount = 1;
                break;
            case 2:
                curCount = 2;
                break;
            case 4:
                curCount = 3;
                break;
            case 6:
                curCount = 4;
                break;
            case 8:
                curCount = 5;
                break;
            case 9:
                curCount = 6;
                break;
            case 10:
                curCount = 7;
                break;
        }

        if (curCount == 0) menuBtn.SetActive(false);
    }
    IEnumerator DelayTime(VideoClip one, VideoClip two = null, bool isFirst = false)
    {
        isPlay = true;
        if (!uiOpen) yield return new WaitForSeconds(3f);
        vp.Stop();
        VideoWindow.SetActive(true);
        if (isFirst) yield return new WaitForSeconds(0.5f);

        if (two == null)
        {
            PlayVideo(one);
        }
        else
        {
            while (true)
            {
                PlayVideo(one);
                yield return new WaitForSeconds(5f);
                PlayVideo(two);
                yield return new WaitForSeconds(5f);
            }
        }
        isPlay = false;
    }

    void PlayVideo(VideoClip clip)
    {
        vp.clip = clip;
        vp.Play();
    }

    public void OpenTutorial()
    {
        uiOpen = true;
        count = 0;
        btns.SetActive(true);
        delayTime = StartCoroutine(DelayTime(videos[0], isFirst: true));
    }

    public void OnClick_Left()
    {
        count--;
        if (count < 0) count = curCount - 1;
        CoroutineStart();
    }
    public void OnClick_Right()
    {
        count++;
        if (count >= curCount) count = 0;
        CoroutineStart();
    }

    void CoroutineStart()
    {
        if(isPlay)StopCoroutine(delayTime);
        if(count == 0) delayTime = StartCoroutine(DelayTime(videos[0]));
        else if(count == 1) delayTime = StartCoroutine(DelayTime(videos[1]));
        else if(count == 2) delayTime = StartCoroutine(DelayTime(videos[2], videos[3]));
        else if(count == 3) delayTime = StartCoroutine(DelayTime(videos[4], videos[5]));
        else if(count == 4) delayTime = StartCoroutine(DelayTime(videos[6], videos[7]));
        else if(count == 5) delayTime = StartCoroutine(DelayTime(videos[8]));
        else if(count == 6) delayTime = StartCoroutine(DelayTime(videos[9]));
    }
}

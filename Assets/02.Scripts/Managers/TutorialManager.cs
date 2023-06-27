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
    public string[] nameList = new string[10];
    string Name;
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

        nameList[0] = Application.streamingAssetsPath + "/1.mp4";
        nameList[1] = Application.streamingAssetsPath + "/2.mp4";
        nameList[2] = Application.streamingAssetsPath + "/3-1.mp4";
        nameList[3] = Application.streamingAssetsPath + "/3-2.mp4";
        nameList[4] = Application.streamingAssetsPath + "/4-1.mp4";
        nameList[5] = Application.streamingAssetsPath + "/4-2.mp4";
        nameList[6] = Application.streamingAssetsPath + "/5-1.mp4";
        nameList[7] = Application.streamingAssetsPath + "/5-2.mp4";
        nameList[8] = Application.streamingAssetsPath + "/6.mp4";
        nameList[9] = Application.streamingAssetsPath + "/7.mp4";

        if (PlayerPrefs.HasKey("Tutorial"))
        {
            tutorialCount = PlayerPrefs.GetInt("Tutorial");
        }
        else
        {
            tutorialCount = 0;
            PlayerPrefs.SetInt("Tutorial", 0);
        }

        //if (GameManager.stageLV+1 == 9 && tutorialCount == 0)
        //{
        //    tutorialCount = 1;
        //    delayTime = StartCoroutine(DelayTime(videos[0], isFirst: true, index:0));
        //}
        //else 
        if (GameManager.stageLV+1 == 14 && tutorialCount == 1)
        {
            tutorialCount = 2;
            delayTime = StartCoroutine(DelayTime(videos[1], isFirst:true, index: 1));
        }
        else if (GameManager.stageLV + 1 == 17 && tutorialCount == 2)
        {
            tutorialCount = 4;
            delayTime = StartCoroutine(DelayTime(videos[2], videos[3], true, index: 2));
        }
        else if (GameManager.stageLV + 1 == 19 && tutorialCount == 4)
        {
            tutorialCount = 6;
            delayTime = StartCoroutine(DelayTime(videos[4], videos[5], true, index: 4));
        }
        else if (GameManager.stageLV + 1 == 24 && tutorialCount == 6)
        {
            tutorialCount = 8;
            delayTime = StartCoroutine(DelayTime(videos[6], videos[7], true, index: 6));
        }
        else if (GameManager.stageLV + 1 == 31 && tutorialCount == 8)
        {
            tutorialCount = 9;
            delayTime = StartCoroutine(DelayTime(videos[8], isFirst: true, index: 8));
        }
        else if (GameManager.stageLV + 1 == 32 && tutorialCount == 9)
        {
            tutorialCount = 10;
            delayTime = StartCoroutine(DelayTime(videos[9], isFirst: true, index: 9));
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
    IEnumerator DelayTime(VideoClip one, VideoClip two = null, bool isFirst = false, int index = 0)
    {
        isPlay = true;
        if (!uiOpen) yield return new WaitForSeconds(3f);
        vp.Stop();
        VideoWindow.SetActive(true);
        if (isFirst) yield return new WaitForSeconds(0.5f);

        if (two == null)
        {
            PlayVideo(index);
        }
        else
        {
            while (true)
            {
                PlayVideo(index);
                yield return new WaitForSeconds(5f);
                PlayVideo(index+1);
                yield return new WaitForSeconds(5f);
            }
        }
        isPlay = false;
    }

    void PlayVideo(int index)
    {
        vp.url = nameList[index];
        vp.Play();
    }

    public void OpenTutorial()
    {
        uiOpen = true;
        count = 0;
        btns.SetActive(true);
        delayTime = StartCoroutine(DelayTime(videos[0], isFirst: false, index : 0));
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

    [ContextMenu("RemoveTutoPrefs")]
    public void RemoveTutoPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

}

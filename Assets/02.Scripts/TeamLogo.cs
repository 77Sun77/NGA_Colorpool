using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
public class TeamLogo : MonoBehaviour
{
    public Image backGround;

    private void Start()
    {
        StartCoroutine(LoadScene_Cor());
    }
    public void DoSound()
    {
        transform.GetComponent<AudioSource>().Play();
    }

    public void DoFadeOut()
    {
        transform.GetComponent<Image>().DOFade(0, 2);
        backGround.GetComponent<Image>().DOFade(0, 2);
    }
    bool isOpen;
    public void DoSceneMove()
    {
        isOpen = true;
    }

    IEnumerator LoadScene_Cor()
    {
        AsyncOperation oper = SceneManager.LoadSceneAsync("StartScene");
        oper.allowSceneActivation = false;

        while (!oper.isDone)
        {
            yield return null;//제어권들 돌려줘서 화면 갱신
            if (oper.progress >= 0.9f)
            {
                while (!isOpen) yield return new WaitForFixedUpdate();
                oper.allowSceneActivation = true;
            }

        }
    }
}

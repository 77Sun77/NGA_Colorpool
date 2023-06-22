using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneLoad : MonoBehaviour
{
    public static string sceneName;
    public GameObject MovingBall;
    public GameObject TargetBall;
    public Vector3 OriginPos;
    public float MoveDistance;

    public float curTextChangeDelay;
    public int TextState;
    public TMPro.TextMeshPro LoadingText;

    bool isOpen;

    void Start()
    {
        isOpen = false;
        //LoadScene();
        StartCoroutine(Fade(true));
    }
    IEnumerator Fade(bool isFadeIn)
    { 

        Fade_InOut fade = GameObject.Find("Fade").GetComponent<Fade_InOut>();
        if (isFadeIn)
        {
            fade.ChangeFade(Fade_InOut.Fade.Fade_In);
            while (!fade.isFade) yield return new WaitForFixedUpdate();
            LoadScene();
        }
        else
        {
            fade.ChangeFade(Fade_InOut.Fade.Fade_Out);
            while (!fade.isFade) yield return new WaitForFixedUpdate();
            
            isOpen = true;
        }
    }
    public void LoadScene()
    {
        while (MovingBall == null)
        {
            MovingBall = GameObject.Find("Ball");
            TargetBall = GameObject.Find("Ball_1");

        }
        OriginPos = MovingBall.transform.position;
        StartCoroutine(nameof(LoadScene_Cor));

    }


    IEnumerator LoadScene_Cor()
    {
        AsyncOperation oper = SceneManager.LoadSceneAsync(sceneName);
        oper.allowSceneActivation = false;

        while (!oper.isDone)
        {
            yield return null;//제어권들 돌려줘서 화면 갱신
            curTextChangeDelay+=Time.deltaTime;
            if (curTextChangeDelay >= 0.3f)
            {
                ChangeText();
                curTextChangeDelay = 0;
            }

            if (oper.progress < 0.9f)
            {
                MoveDistance = oper.progress;
                MovingBall.transform.position = Vector3.Lerp(OriginPos, TargetBall.transform.position, MoveDistance);
            }
            else
            {
                MoveDistance += Time.deltaTime;
                MovingBall.transform.position = Vector3.Lerp(OriginPos, TargetBall.transform.position, MoveDistance);
                if (MoveDistance >= 1f)
                {
                    StartCoroutine(Fade(false));
                    while (!isOpen) yield return new WaitForFixedUpdate();
                    yield return new WaitForSeconds(0.5f);
                    oper.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }

    void ChangeText()
    {
        TextState++;
        if (TextState == 3)
            TextState = 0;

        switch (TextState)
        {
            case 0:
                LoadingText.text = "Loading.";
                break;
            case 1:
                LoadingText.text = "Loading..";
                break;
            case 2:
                LoadingText.text = "Loading...";
                break;
        }

    }

}



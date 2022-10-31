using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneLoad : MonoBehaviour
{
    public GameObject MovingBall;
    public GameObject TargetBall;
    public Vector3 OriginPos;
    public float MoveDistance;

    public float curTextChangeDelay;
    public int TextState;
    public TMPro.TextMeshPro LoadingText;
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
        AsyncOperation oper = SceneManager.LoadSceneAsync("PlayScene");
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



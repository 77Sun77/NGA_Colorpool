using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DebugManager : MonoBehaviour
{
    public static DebugManager instance;
    public static bool IsDebugStart;
    public int stageIndex_ForDebugging;

    public TextMeshProUGUI debugMessage; 

    private void Awake()
    {
        instance = this;
        StartCoroutine(Initialize_DebugMan());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            stageIndex_ForDebugging++;
            StartStage();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            stageIndex_ForDebugging--;
            StartStage();
        }

        debugMessage.text = stageIndex_ForDebugging.ToString();
    }

    IEnumerator Initialize_DebugMan()
    {
        yield return GameManager.instance;
        stageIndex_ForDebugging = GameManager.stageLV + 1;
    }

    public void StartStage()
    {
        GameManager.stageLV = stageIndex_ForDebugging - 1;
        IsDebugStart = true;
        //GameManager.instance.PlayStage();
        SceneManager.LoadScene("PlayScene");
    }

    [ContextMenu("SetBallColorVisually")]
    public void SetBallColorVisually()
    {
        Ball[] balls = FindObjectsOfType<Ball>(true);
        foreach (var ball in balls)
        {
            ball.SetColorVisually();
        }
    }



}



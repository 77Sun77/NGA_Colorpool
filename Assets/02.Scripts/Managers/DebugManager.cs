using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour
{
    public static DebugManager instance;
    public static bool IsDebugStart;
    public int stageIndex_ForDebugging;

    public TextMeshProUGUI debugMessage;
    public TMP_InputField StageSkipper;
    public Button Button_SkipEnter;
    public Button Button_SkipNext;
    public Button Button_SkipPast;

    private void Awake()
    {
        instance = this;

        Button_SkipEnter.onClick.AddListener(SkipEnter);
        Button_SkipNext.onClick.AddListener(SkipNext);
        Button_SkipPast.onClick.AddListener(SkipPast);

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
        SceneManager.LoadScene("PlayScene_New");
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

    public void SkipEnter()
    {
         if(int.TryParse(StageSkipper.text,out stageIndex_ForDebugging)) StartStage();
    }

    public void SkipNext()
    {
        stageIndex_ForDebugging++;
        StartStage();
    }

    public void SkipPast()
    {
        stageIndex_ForDebugging--;
        StartStage();
    }
}



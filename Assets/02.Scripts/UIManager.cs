using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject UI_TargetPlace;
    public GameObject[] UI_ballImagePrefabs;
    public GameObject UI_CheckImagePrefab;

    public GameObject UI_ScoreBoard;
    public GameObject[] UI_Stars;

    public TextMeshProUGUI targetText;
    public TextMeshProUGUI shotText;

    [HideInInspector]
    public List<GameObject> InstantiatedObj;

    public List<GameObject> UI_BallImg;
    /// <summary>
    /// BallImg에 체크표시 여부담은 List
    /// </summary>
    public List<bool> BallChecked;


    public int imgCount;
    public int ballIndex;

    public int h_interval = 90;
    public int v_interval;

    public bool isOnScoreBoard;


    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        ShowTargetsOnUI();
    }

    [ContextMenu("Test")]
    void ShowTargetsOnUI()
    {
        //Debug.Log("Test");

        foreach (GameObject go in InstantiatedObj)
        {
            Destroy(go);
        }
        InstantiatedObj.Clear();
        BallChecked.Clear();
        UI_BallImg.Clear();

        SetImgCount();

        for (int i = 0; i < imgCount; i++)
        {
            BallChecked.Add(false);
        }

        CheckSuccessBall();

        SetBallUI();
        SetCheckUI();

    }
    void SetImgCount()
    {
        imgCount = GameManager.instance.targetList.Count;
        //Debug.Log($"타겟리스트의 길이는{imgCount}입니다");
    }

    int SetBallColorToIndex(string _colorName)
    {
        switch (_colorName)
        {
            case "Red":
                return 0;

            case "Orange":
                return 1;

            case "Yellow":
                return 2;

            case "Green":
                return 3;

            case "Blue":
                return 4;

            case "Purple":
                return 5;

            case "Black":
                return 6;

        }
        return -1;
    }


    void CheckSuccessBall()
    {
        for (int i = 0; i < GameManager.instance.targetList.Count; i++)
        {
            if (GameManager.instance.curTargetDic[GameManager.instance.targetList[i]] <= 0)
            {
                BallChecked[i] = true;
            }


        }


    }

    void SetBallUI()
    {

        for (int i = 0; i < imgCount; i++)
        {
            string colorName = GameManager.instance.targetList[i];

            GameObject go = Instantiate(UI_ballImagePrefabs[SetBallColorToIndex(colorName)], UI_TargetPlace.transform);
            go.transform.localPosition = GetPositon(i);



            //프리펩 관리
            InstantiatedObj.Add(go);
            UI_BallImg.Add(go);
        }
    }

    void SetCheckUI()
    {
        //BallChecked.Reverse();
        for (int i = 0; i < BallChecked.Count; i++)
        {
            if (BallChecked[i] == true)
            {
                GameObject go = Instantiate(UI_CheckImagePrefab, UI_TargetPlace.transform);
                go.transform.localPosition = UI_BallImg[i].transform.localPosition;
                InstantiatedObj.Add(go);
            }
        }
    }


    public void EnableScoreBoard(int score)
    {
        Debug.Log("스코어보드 실행");
        isOnScoreBoard = true;
        UI_ScoreBoard.SetActive(true);

        foreach (GameObject UI_Star in UI_Stars)
        {
            UI_Star.SetActive(false);
        }
        for (int i = 0; i < score; i++)
        {
            UI_Stars[i].SetActive(true);
        }

        targetText.text = $"Goal Shot Count:{GameManager.instance.shotRule}";
        shotText.text = $"Shot Count:{GameManager.instance.shotCount}";
    }

    public void InitializeStage()
    {
        GameManager.instance.shotCount = 0;
        GameManager.instance.targetList.Clear();
        GameObject go = GameObject.Find($"Stage_{GameManager.instance.stageLV}(Clone)");
        Destroy(go);
        isOnScoreBoard = false;
        UI_ScoreBoard.SetActive(false);

    }

    public void RestartStage()
    {
        InitializeStage();
        GameManager.instance.PlayStage();

    }

    public void NextStage()
    {
        InitializeStage();
        GameManager.instance.stageLV++;
        GameManager.instance.PlayStage();

    }

    Vector3 GetPositon(int i)
    {
        if (imgCount % 2 == 0)
        {

            //짝수일때
            //Debug.Log(new Vector3((-(imgCount / 2 - 0.5f) + i) * h_interval, v_interval, 0));
            return new Vector3((-(imgCount / 2 - 0.5f) + i) * h_interval, v_interval, 0);

        }
        else
        {
            //홀수일때
            //Debug.Log(new Vector3((-(imgCount / 2) + i) * h_interval, v_interval, 0));

            return new Vector3((-(imgCount / 2) + i) * h_interval, v_interval, 0);
        }


    }

}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 딕셔너리 인스펙터창에서 보여주는 에셋
/// </summary>
[System.Serializable]
public class StringInt : SerializableDictionary<string, int> { }


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isClear, isStart;

    public static string moveScene;

    [Header("StageOption")]
    public static int stageLV = 0;
    public Transform Stages_Parent;
    public List<StageOption> stageOptions = new List<StageOption>();
    int star_Count;

    [Header("BallOption")]
    public bool isAllBallShot, isValid;
    public List<Ball> balls;

    //Dictionary<string, int> colorRule = new Dictionary<string, int>();//스테이지에서 요구하는 공 
    //Dictionary<string, int> colorCount = new Dictionary<string, int>();//현재 보유중인 공

    public StringInt colorRule = new();
    public StringInt colorCount = new();

    public StringInt curTargetDic = new();

    public List<string> targetList = new();//스테이지에서 요구하는 공(Rule의 List형태)
    public List<string> curTargetList = new();//현재 목표로 하는 공



    public int shotRule;
    public int shotCount;
    public bool firstAnim;

    public GameObject paintBrush_Prefab;
    public Transform movingWall1;
    public Transform movingWall2;

    public GameObject ClearAnim_Prefab;

    public int HitSoundIndex;

    public static GameObject static_SoundManager;
    public GameObject soundManager;

    public ClickMove2 clickMove2;
    void Awake()
    {
        instance = this;

        isAllBallShot = false;
        isValid = false;

        shotCount = 0;
        star_Count = 0;

        //if (static_SoundManager == null)
        //{
        //    if (GameObject.Find("SoundManager"))
        //        return;

        //    soundManager.SetActive(true);
        //    static_SoundManager = soundManager;
        //    DontDestroyOnLoad(soundManager);
        //}


        if (!GameObject.Find("SoundManager"))
        {
            soundManager.SetActive(true);
            SoundManager.instance = soundManager.GetComponent<SoundManager>();
            DontDestroyOnLoad(soundManager);
        }
    }
    public GameObject text, text2, ColorBook;
    void Start()
    {
        stageOptions.AddRange(Stages_Parent.GetComponentsInChildren<StageOption>(true));

        for (int i = 0; i < stageOptions.Count; i++)
        {
            if (stageOptions[i].gameObject.activeSelf)
            {
                stageLV = i;
                stageOptions[i].gameObject.SetActive(false);
            }
        }

        PlayStage();
        //1스테이지 실행
    }
    public void PlayStage()
    {
        if(SoundManager.instance)
        {
            SoundManager.instance.InitializeBubble();
            SoundManager.instance.isEnable_BallHitSound = true;
            SoundManager.instance.ManagerState = SoundManager.State.InGame;
        }

        //SetBalls();
        //ValidColor();
        stageOptions[stageLV].SetStageRule();

        if(UIManager.instance)
        UIManager.instance.Set_Target_Img();
      
        StageOption stage = Instantiate(stageOptions[stageLV]);
        stage.gameObject.SetActive(true);
        if (stage.TryGetComponent(out MapAnim mapanim)) clickMove2.mapAnim = mapanim;

    }


    void Update()
    {

        SetBalls();
        //if (firstAnim)
        //{
        //    foreach (Ball ball in balls)
        //    {
        //        if (ball.GetComponent<Animator>().enabled) return;
        //        else firstAnim = false;
        //    }
        //}

        isAllBallShot = Set_IsAllBallShot();

        if (isAllBallShot == false && shotCount != 0)
        {
            if (!isValid) ValidColor();
        }
    }

    void SetBalls()
    {
        balls.Clear();//초기화

        Ball[] curBalls = FindObjectsOfType<Ball>();//현재 존재하는 Ball들 찾기
        //Ball 종류는 등록
        foreach (Ball curball in curBalls)
        {
            if (curball.kind == Ball.ObjectKind.Ball)
                balls.Add(curball);
        }
    }

    /// <summary>
    /// 모든 공이 멈추면 false를 반환
    /// </summary>
    /// <returns></returns>
    bool Set_IsAllBallShot()
    {
        foreach (Ball ball in balls)
        {
            if (ball.isShot)
                return true;
        }
        //HitSoundIndex = 0;
        return false;
    }

    void ValidColor()
    {
        isValid = true;
        Debug.Log("실행중");
        Reset_ColorCount();
        curTargetList.Clear();
        curTargetDic.Clear();


        foreach (Ball ball in balls)
        {
            //공색깔을 colorCount에 넣음
            Add_ColorCount(ball.color_Name.ToString());
        }
        bool isClear = false;
        bool isBreak = false;
        List<string> curColorCount = new List<string>();
        foreach (KeyValuePair<string, int> rule in colorRule)
        {
            foreach (KeyValuePair<string, int> color in colorCount)
            {
                if (rule.Value == 0) continue;
                if (rule.Key == color.Key)
                {
                    for (int i = 0; i < color.Value; i++) curColorCount.Add(color.Key);

                    if (rule.Value == color.Value) isClear = true;
                    else
                    {
                        isClear = false;
                        isBreak = true;
                        break;
                    }

                }

            }
        }
        if (isBreak) isClear = false;
        this.isClear = isClear;
        UIManager.instance.Set_Check(curColorCount.ToArray());
        if (UIManager.instance.isOnScoreBoard == false)
        {
            if (isClear)
            {
                Star_Calculate();
                if (PlayerPrefs.GetInt("STAGE") < stageLV + 1) PlayerPrefs.SetInt("STAGE", stageLV + 1);
                if (PlayerPrefs.GetInt((stageLV + 1) + "_STAR") < star_Count) PlayerPrefs.SetInt((stageLV + 1) + "_STAR", star_Count);
                foreach (Ball ball in balls) ball.GetComponent<Collider>().isTrigger = true;
                FindObjectOfType<MapAnim>().EndMapAnim();
            }
        }



        /*
        foreach (KeyValuePair<string, int> rule in colorRule)
        {
            foreach (KeyValuePair<string, int> color in colorCount)
            {
                if (rule.Key == color.Key)
                {
                    curTargetDic.Add(rule.Key, rule.Value - color.Value);
                }

            }
        }

        

        foreach (KeyValuePair<string, int> targetDic in curTargetDic)
        {
            if (targetDic.Value > 0)
            {
                for (int i = 0; i < targetDic.Value; i++)
                {
                    curTargetList.Add(targetDic.Key);
                }
            }

        }
        */

    }
    /// <summary>
    /// colorCount에 값 추가
    /// </summary>
    /// <param name="keyName"></param>
    void Reset_ColorCount()
    {
        colorCount.Clear();

        colorCount.Add("Red", 0);
        colorCount.Add("Orange", 0);
        colorCount.Add("Yellow", 0);
        colorCount.Add("Green", 0);
        colorCount.Add("Blue", 0);
        colorCount.Add("Purple", 0);
        colorCount.Add("Black", 0);
    }

    void Add_ColorCount(string keyName)
    {
        colorCount[keyName] += 1;
    }

    public void Set_Rule(int shotRule, int red = 0, int orange = 0, int yellow = 0, int green = 0, int blue = 0, int purple = 0, int black = 0)
    {
        colorRule.Clear();
        colorRule.Add("Red", red);
        colorRule.Add("Orange", orange);
        colorRule.Add("Yellow", yellow);
        colorRule.Add("Green", green);
        colorRule.Add("Blue", blue);
        colorRule.Add("Purple", purple);
        colorRule.Add("Black", black);

        this.shotRule = shotRule;
    }

    //점수 정산
    void Star_Calculate()
    {
        if (shotCount <= shotRule)
        {
            //3점
            star_Count = 3;
        }
        else if (shotCount > shotRule)
        {
            if (shotCount == shotRule + 1)
            {
                //2점
                star_Count = 2;
            }
            else
            {
                //1점
                star_Count = 1;
            }

        }
    }

    public void ColorEnable()
    {
        StartCoroutine(ColorAnim_Enable());
    }
    IEnumerator ColorAnim_Enable()
    {
        foreach (Ball ball in balls)
        {
            Vector3 ballPos = ball.transform.position;
            GameObject go = Instantiate(ClearAnim_Prefab, new Vector3(ballPos.x, 1.1f, ballPos.z), Quaternion.Euler(Vector3.right * 90));
            go.GetComponent<SpriteRenderer>().color = ball.myMaterial.color;
        }
        SoundManager.instance.PlayTargetSound(SoundManager.instance.BallWaveSFX);
        yield return new WaitForSeconds(1);
        UIManager.instance.EnableScoreBoard(star_Count);
    }

    /// <summary>
    /// 스테이지의 총 목표를 설정하는 함수
    /// </summary>
    public void SetTargetCount()
    {
        foreach (KeyValuePair<string, int> rule in colorRule)
        {
            for (int i = 0; i < rule.Value; i++)
            {
                targetList.Add(rule.Key);
            }
        }

        UIManager.instance.SetBallUi_BG(targetList.ToArray());
    }



}

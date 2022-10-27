using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// ��ųʸ� �ν�����â���� �����ִ� ����
/// </summary>
[System.Serializable]
public class StringInt : SerializableDictionary<string, int> { }


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

 


    [Header("StageOption")]
    public static int stageLV;
    public List<StageOption> stageOptions = new List<StageOption>();

    [Header("BallOption")]
    public bool isAllBallShot, isValid;
    public List<Ball> balls;

    //Dictionary<string, int> colorRule = new Dictionary<string, int>();//������������ �䱸�ϴ� �� 
    //Dictionary<string, int> colorCount = new Dictionary<string, int>();//���� �������� ��

    public StringInt colorRule = new();
    public StringInt colorCount = new();

    public StringInt curTargetDic=new();

    public List<string> targetList = new();//������������ �䱸�ϴ� ��(Rule�� List����)
    public List<string> curTargetList = new();//���� ��ǥ�� �ϴ� ��
    


    public int shotRule;
    public int shotCount;
    public bool firstAnim;

    public GameObject paintBrush_Prefab;
    public Transform movingWall1;
    public Transform movingWall2;

    public GameObject ClearAnim_Prefab;

    void Awake()
    {
        instance = this;

        isAllBallShot = false;
        isValid = false;

        shotCount = 0;

        
    }

    void Start()
    {
        //1�������� ����
        PlayStage();
    }
   public void PlayStage()
    {
        SoundManager.instance.InitializeBubble();
        Instantiate(stageOptions[stageLV]);
        stageOptions[stageLV].SetStageRule();
        SetBalls();
        UIManager.instance.Set_Target_Img();
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
            if(!isValid) ValidColor();

        }

    }

    void SetBalls()
    {
        balls.Clear();//�ʱ�ȭ

        Ball[] curBalls = FindObjectsOfType<Ball>();//���� �����ϴ� Ball�� ã��
        //Ball ������ ���
        foreach (Ball curball in curBalls)
        {
            if (curball.kind == Ball.ObjectKind.Ball)
                balls.Add(curball);
        }
    }

    /// <summary>
    /// ��� ���� ���߸� false�� ��ȯ
    /// </summary>
    /// <returns></returns>
    bool Set_IsAllBallShot()
    {
        foreach (Ball ball in balls)
        {
            if (ball.isShot)
                return true;
        }
        return false;
    }

    void ValidColor()
    {
        isValid = true;
        Debug.Log("������");
        Reset_ColorCount();
        curTargetList.Clear();
        curTargetDic.Clear();


        foreach (Ball ball in balls)
        {
            //�������� colorCount�� ����
            Add_ColorCount(ball.color_Name.ToString());
        }
        bool isClear = false;
        List<string> curColorCount = new List<string>();
        foreach(KeyValuePair<string, int> rule in colorRule)
        {
            foreach (KeyValuePair<string, int> color in colorCount)
            {
                if (rule.Value == 0) continue;
                if (rule.Key == color.Key )
                {
                    for(int i=0; i<color.Value; i++) curColorCount.Add(color.Key);

                    if (rule.Value == color.Value) isClear = true;
                    else
                    {
                        isClear = false;
                        break;
                    }

                }
                
            }
        }
        UIManager.instance.Set_Check(curColorCount.ToArray());
        if (UIManager.instance.isOnScoreBoard == false)
        {
            if (isClear)
            {
                FindObjectOfType<MapAnim>().EndMapAnim();
            }
            else
            {

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
    /// colorCount�� �� �߰�
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

    //���� ����
    void OnStageEnd()
    {
        if (shotCount <= shotRule)
        {
            //3��
            UIManager.instance.EnableScoreBoard(3);
        }
        else if (shotCount > shotRule)
        {
            if (shotCount == shotRule + 1)
            {
                //2��
                UIManager.instance.EnableScoreBoard(2);
            }
            else
            {
                //1��
                UIManager.instance.EnableScoreBoard(1);
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
            GameObject go = Instantiate(ClearAnim_Prefab, new Vector3(ballPos.x, 1.1f, ballPos.z), Quaternion.Euler(Vector3.right*90));
            go.GetComponent<SpriteRenderer>().color = ball.myMaterial.color;
            SoundManager.instance.PlayTargetSound(SoundManager.instance.BallWaveSFX);
        }
        yield return new WaitForSeconds(1);
        OnStageEnd();
    }

    /// <summary>
    /// ���������� �� ��ǥ�� �����ϴ� �Լ�
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
    }


   
}

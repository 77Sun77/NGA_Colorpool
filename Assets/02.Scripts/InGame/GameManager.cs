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
    public int stageLV;
    public List<StageOption> stageOptions = new List<StageOption>();

    [Header("BallOption")]
    public bool isAllBallShot;
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

    void Awake()
    {
        instance = this;

        isAllBallShot = false;

        shotCount = 0;

        //1�������� ����
        PlayStage();
    }

   public void PlayStage()
    {
        Instantiate(stageOptions[stageLV]);
        stageOptions[stageLV].SetStageRule();
    }


    void Update()
    {
        
        SetBalls();
        isAllBallShot = Set_IsAllBallShot();

        if (isAllBallShot == false)
        {
            ValidColor();
        }

        if(shotCount!=0)
        if (curTargetList.Count==0)
        {
            if (UIManager.instance.isOnScoreBoard == false)
                OnStageEnd();
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
        Debug.Log("������");
        Reset_ColorCount();
        curTargetList.Clear();
        curTargetDic.Clear();


        foreach (Ball ball in balls)
        {
            //�������� colorCount�� ����
            Add_ColorCount(ball.color_Name.ToString());
        }


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

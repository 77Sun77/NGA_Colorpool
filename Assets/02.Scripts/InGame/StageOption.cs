using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageOption : MonoBehaviour
{

    public int shotCount;
    [Header("Ball")]
    public int RedBallCount;
    public int OrangeBallCount;
    public int YellowBallCount;
    public int GreenBallCount;
    public int BlueBallCount;
    public int PurpleBallCount;
    public int BlackBallCount;

  public void SetStageRule()
  {
        GameManager.instance.Set_Rule(shotCount, RedBallCount, OrangeBallCount, YellowBallCount, GreenBallCount, BlueBallCount, PurpleBallCount, BlackBallCount);
        GameManager.instance.SetTargetCount();
  }

}

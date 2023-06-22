using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DebugManager : MonoBehaviour
{
    public static DebugManager instance;
   
    public int stageIndex_ForDebugging;
    private void Awake()
    {
        instance = this;
        stageIndex_ForDebugging = GameManager.stageLV + 1;
    }
   
    public void StartStage()
    {
        GameManager.stageLV = stageIndex_ForDebugging-1;
        //GameManager.instance.PlayStage();
        SceneManager.LoadScene("PlayScene");
    }

}



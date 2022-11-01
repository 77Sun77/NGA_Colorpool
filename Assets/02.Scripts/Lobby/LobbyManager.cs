using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LobbyManager : MonoBehaviour
{
    public static LobbyManager instance;
    public GameObject[] Stages;
    public GameObject[] Circles;
    public int pageNum;
    Transform canvas;

    public GameObject UI;
    public SceneLoad Obj_SL;

    void Start()
    {
        instance = this;
        pageNum = 1;

        canvas = GameObject.Find("Canvas").transform;

        Fade_InOut fade = GameObject.Find("Fade").GetComponent<Fade_InOut>();
        fade.ChangeFade(Fade_InOut.Fade.Fade_In);
    }

    void Update()
    {
        /*
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.farClipPlane;
            Vector3 vec = Camera.main.ScreenToWorldPoint(mousePos);
            if (vec.x <= 0) OnClick_Left();
            else OnClick_Right();
        }
        */
    }

    public void OnClick_Left()
    {
        if (pageNum == 1) return;
        pageNum--;
        foreach (GameObject stage in Stages) stage.SetActive(false);
        foreach (GameObject circle in Circles) circle.SetActive(false);
        Stages[pageNum - 1].SetActive(true);
        Circles[pageNum - 1].SetActive(true);
    }
    public void OnClick_Right()
    {
        if (pageNum == 5) return;
        pageNum++;
        foreach (GameObject stage in Stages) stage.SetActive(false);
        foreach (GameObject circle in Circles) circle.SetActive(false);
        Stages[pageNum - 1].SetActive(true);
        Circles[pageNum - 1].SetActive(true);
    }

    
   public void Initialize_Lobby()
    {
        Camera.main.gameObject.SetActive(true);
        instance.Obj_SL.gameObject.SetActive(false);
        instance.UI.transform.localScale = Vector3.one;
    }


}

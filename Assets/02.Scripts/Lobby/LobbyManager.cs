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
    void Start()
    {
        instance = this;
        pageNum = 1;
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.farClipPlane;
            print(Camera.main.ScreenToWorldPoint(mousePos).x);
            if (Camera.main.ScreenToWorldPoint(mousePos).x <= 0) OnClick_Left();
            else OnClick_Right();
        }

    }

    void OnClick_Left()
    {
        if (pageNum == 1) return;
        pageNum--;
        foreach (GameObject stage in Stages) stage.SetActive(false);
        foreach (GameObject circle in Circles) circle.SetActive(false);
        Stages[pageNum - 1].SetActive(true);
        Circles[pageNum - 1].SetActive(true);
    }
    void OnClick_Right()
    {
        if (pageNum == 5) return;
        pageNum++;
        foreach (GameObject stage in Stages) stage.SetActive(false);
        foreach (GameObject circle in Circles) circle.SetActive(false);
        Stages[pageNum - 1].SetActive(true);
        Circles[pageNum - 1].SetActive(true);
    }
}

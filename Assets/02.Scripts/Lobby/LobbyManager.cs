using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    GameObject canvas;
    public GameObject left, right;

    public Transform Content;
    public Transform endPoint;
    public Transform[] contentsPos;
    Vector3 start_Pos;
    Vector3 end_Pos;
    Vector3 direction;
    int pageNum;
    bool isMove;
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        pageNum = 1;
        isMove = false;
        end_Pos = canvas.transform.InverseTransformPoint(endPoint.position);

        Application.targetFrameRate = 60;
    }

    void Update()
    {
        if (isMove)
        {
            Vector3 pos = canvas.transform.InverseTransformPoint(contentsPos[pageNum - 1].position);
            bool distance = false;
            if (direction == Vector3.right) distance = pos.x > end_Pos.x;
            else distance = pos.x < end_Pos.x;
            if (distance)
            {
                Content.position += Vector3.right * Vector3.Distance(pos, end_Pos);
                left.SetActive(true);
                right.SetActive(true);
                isMove = false;
            }
            else
            {
                Content.Translate(direction * 2000 * Time.deltaTime);
            }
        }
        print(Content.position);
    }

    public void OnClick_Left()
    {
        if (pageNum == 1) return;
        pageNum--;
        isMove = true;
        left.SetActive(false);
        right.SetActive(false);
        start_Pos = canvas.transform.InverseTransformPoint(contentsPos[pageNum - 1].position);
        direction = Vector3.right;
    }
    public void OnClick_Right()
    {
        if (pageNum == 4) return;
        pageNum++;
        isMove = true;
        left.SetActive(false);
        right.SetActive(false);
        start_Pos = canvas.transform.InverseTransformPoint(contentsPos[pageNum - 1].position);
        direction = Vector3.left;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    GameObject canvas;
    
    public Transform stage_ScrollView;
    public Transform[] stagesTr;
    Vector3[] stagesVec = new Vector3[3];
    Vector3 target;
    bool autoScroll,onclickScroll;
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        autoScroll = false;
        onclickScroll = false;
        for (int i = 0; i < 3; i++)
        {
            stagesVec[i] = canvas.transform.TransformPoint(stagesTr[i].position);
            stagesVec[i].x -= 1920;
            stagesVec[i].x *= -1;
        }
    }

    void Update()
    {

        ScrollView();
    }

    void ScrollView()
    {
        stage_ScrollView.parent.parent.GetComponent<ScrollRect>().enabled = !autoScroll;
        if (autoScroll)
        {
            Vector3 pos = stage_ScrollView.position;
            pos = Vector3.Lerp(pos, target, 20 * Time.deltaTime);
            stage_ScrollView.position = pos;
            if (Vector3.Distance(pos, target) <= 0.1f)
            {
                stage_ScrollView.position = target;
                autoScroll = false;
                onclickScroll = false;
            }
        }

        if (stage_ScrollView.position.x >= 500)
        {
            autoScroll = true;
            target = new Vector3(0, 1080, 0);
        }

        if (Input.GetMouseButtonDown(0)) onclickScroll = true;
        if (Input.GetMouseButtonUp(0) && onclickScroll)
        {
            float distance = Vector3.Distance(Vector3.right * 960, stagesTr[0].position);
            for(int i=0; i<3; i++)
            {
                if(distance >= Vector3.Distance(Vector3.right * 960, stagesTr[i].position))
                {
                    distance = Vector3.Distance(Vector3.right * 960, stagesTr[i].position);
                    target = stagesVec[i];
                }
            }
            target.y = 1080;
            onclickScroll = false;
            autoScroll = true;
            
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : MonoBehaviour
{

    public Transform target;
    Vector3 rayPos = Vector3.zero;
    Vector3 vec = Vector3.zero;


   public bool isShot;

    void Update()
    {
        if (!GameManager.instance.isAllBallShot && GameManager.instance.isStart && !GameManager.instance.isClear)
        {
            Mouse_Fun();
        }
        
    }

    void Mouse_Fun()
    {
        //처음 마우스를 클릭할때 타겟을 정함
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layerMask = (1 << LayerMask.NameToLayer("Ball_Red")) | (1 << LayerMask.NameToLayer("Ball_Orange")) | (1 << LayerMask.NameToLayer("Ball_Yellow")) | (1 << LayerMask.NameToLayer("Ball_Green")) | (1 << LayerMask.NameToLayer("Ball_Blue")) | (1 << LayerMask.NameToLayer("Ball_Purple") | (1 << LayerMask.NameToLayer("Ball_CurTargetting")));
            Physics.Raycast(ray, out hit, 100, layerMask);
            if (hit.collider != null)
            {
                Ball ball = hit.transform.GetComponent<Ball>();
                if (ball.color_Name == Ball.Ball_Color.Black || ball.kind == Ball.ObjectKind.Wall)
                    return;
                target = hit.transform;

            }
        }

        
        if (Input.GetMouseButton(0) && target != null)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layerMask = 1 << LayerMask.NameToLayer("Ground");
            Physics.Raycast(ray,out hit, 100, layerMask);
            if (hit.collider != null)
            {
                rayPos = hit.point;
                vec = rayPos - target.position;
                //vec = target.position - rayPos;
                Ball targetBall = target.GetComponent<Ball>();
                targetBall.isTargetting = true;
                float distance = 0;
                if(targetBall.ballKind == Ball.BallKind.Small)
                {
                    distance = Vector3.Distance(rayPos, target.position) * 3f;
                    distance = Mathf.Clamp(distance, 0.2f, 13f);
                }
                else
                {
                    distance = Vector3.Distance(rayPos, target.position) * 1.85f;
                    distance = Mathf.Clamp(distance, 0.2f, 8f);
                }
                Vector3 vec2 = target.position - (vec.normalized* distance);
                //Vector3 vec2 = (vec.normalized* distance);
                vec2.y = 0.5f;
                //Debug.Log(distance);
                isShot = target.GetComponent<Ball>().Set_Line(vec2);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
          
            if(target != null) target.GetComponent<Ball>().isTargetting = false;
            if (target != null && isShot)
            {
                target.GetComponent<Ball>().isTargetting = false;

                float power = Vector3.Distance(target.position, rayPos);
                Ball targetBall = target.GetComponent<Ball>();
                targetBall.Set_Line(vec, 1);
                targetBall.Shot(-vec.normalized * SpeedCalculator(power, targetBall.ballKind));

                GameManager.instance.isAllBallShot = true;

            }
            target = null;
        }
    }

    float SpeedCalculator(float power, Ball.BallKind kind)
    {
        float speed = 0;
        if (kind == Ball.BallKind.Big)
        {
            power = Mathf.Clamp(power, 0.8f, 4.3f);
            speed = (power - 0.8f) * 10 + 2;
        }
        else
        {
            power = Mathf.Clamp(power, 0.8f, 4.3f);
            speed = (power - 0.8f) * 10 + 2;
        }

        return speed;
    }

}

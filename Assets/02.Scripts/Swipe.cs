using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : MonoBehaviour
{

    Transform target;
    Vector3 rayPos = Vector3.zero;
    Vector3 vec = Vector3.zero;

    bool isShot;

    void Update()
    {
        if (!GameManager.instance.isAllBallShot)
        {
            Mouse_Fun();
        }
        
    }

    void Mouse_Fun()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit);
            if (hit.collider != null && hit.transform.CompareTag("BALL"))
            {
                Ball ball = hit.transform.GetComponent<Ball>();
                if (ball.color_Name == Ball.Ball_Color.Black || ball.kind == Ball.ObjectKind.Wall)
                    return;
                target = hit.transform;

            }
        }

        if (Input.GetMouseButton(0) && target != null)
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out float rayDistance))
            {
                rayPos = ray.GetPoint(rayDistance);
                vec = rayPos - target.position;
                Ball targetBall = target.GetComponent<Ball>();
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
                vec2.y = 0.5f;
                isShot = target.GetComponent<Ball>().Set_Line(vec2);
                
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (target != null && isShot)
            {
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

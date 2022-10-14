using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    new Camera camera;

    bool isActive, isMove;
    float size, speed;
    Vector3 target;

    enum direction { none, down, up };
    direction dir;
    void Start()
    {
        camera = GetComponent<Camera>();

        size = 10;
        speed = 3.5f;
        isActive = false; 
        isMove = false;
    }

    void Update()
    {
        if (isActive)
        {
            Camera_Move();

        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Start_Move(Vector3.left);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Start_Move(Vector3.right);
        }

        transform.position = new Vector3(transform.position.x, 13, 0);
    }

    public void Start_Move(Vector3 direction)
    {
        if (dir == CameraMove.direction.up ||(target.x <= 0 && direction.x < 0)) return;
        if (isMove && dir == CameraMove.direction.none && Mathf.Abs(target.x - transform.position.x) <= 1.5f)
        {
            Vector3 vec = new Vector3(0, 10, 0);
            target += (direction * 30);
            target += vec;
            return;
        }
        if (isMove && dir == CameraMove.direction.down)
        {
            isMove = false;
            Vector3 vec = new Vector3(0, 10, 0);
            target += (direction * 30);
            target += vec;
            return;
        }


        if(transform.position.x >= 0)
        {
            isActive = true;
            Vector3 vec = new Vector3(0, 10, 0);
            target += (direction * 30);
            target += vec;
        }

        if(target.x < 0)
        {
            target = new Vector3(0, 10, 0);
        }
    }

    void Camera_Move()
    {
        if (isMove)
        {
            //transform.Translate(target.normalized * Time.deltaTime * 20f);
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime*speed);
            if (Mathf.Abs(target.x - transform.position.x) <= 1.5f) speed = 6f;
            if (Mathf.Abs(target.x - transform.position.x) <= 0.1f)
            {
                dir = direction.down;
                size = Mathf.Lerp(size, 10, Time.deltaTime*4);
                camera.orthographicSize = size;
                if(size <= 10.01f)
                {
                    transform.position = target;
                    size = 10;
                    camera.orthographicSize = size;
                    isMove = false;
                    isActive = false;
                    speed = 3.5f;
                }
            }

        }
        else
        {
            if (size >= 14.9f)
            {
                dir = direction.none;
                size = 15;
                camera.orthographicSize = size;
                isMove = true;
            }
            else
            {
                dir = direction.up;
                size = Mathf.Lerp(size, 15, Time.deltaTime*4);
                camera.orthographicSize = size;
                return;
            }
        }

    }
}

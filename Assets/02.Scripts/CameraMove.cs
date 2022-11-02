using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public static Quaternion parentRotation;
    public static float xRotate = 0;
    GameManager gm;
    Camera camera;
    float size;

    Swipe swipe;
    Fade_InOut fade;

    
    void Start()
    {
        transform.parent.rotation = parentRotation;
        gm = GameManager.instance;
        camera = GetComponent<Camera>();
        size = 12;

        swipe = GetComponent<Swipe>();
        fade = GameObject.Find("Fade").GetComponent<Fade_InOut>();
    }

    void Update()
    {
        //transform.parent.Rotate(Vector3.up * 50 * Time.deltaTime);
        if (GameManager.instance.isStart && !GameManager.instance.isClear)
        {
            parentRotation = transform.parent.rotation;
            if (Input.GetMouseButton(0))
            {
                if (swipe.target == null)
                {
                    float y = Input.GetAxis("Mouse X") * Time.deltaTime * 100;
                    xRotate = xRotate + y;
                    transform.parent.eulerAngles = new Vector3(0, xRotate, 0);
                }
                
            }

            if (!gm.isAllBallShot && !gm.isClear)
            {
                if (size <= 9f)
                {
                    camera.orthographicSize = 9;
                    return;
                }
                size = Mathf.Lerp(size, 9, 4 * Time.deltaTime);
                camera.orthographicSize = size;
            }
            else
            {
                if (size >= 12f)
                {
                    camera.orthographicSize = 12;
                    return;
                }
                size = Mathf.Lerp(size, 12, 4 * Time.deltaTime);
                camera.orthographicSize = size;
            }

            
        }
        
    }

}

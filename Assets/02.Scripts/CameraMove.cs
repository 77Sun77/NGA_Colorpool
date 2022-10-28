using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    GameManager gm;
    Camera camera;
    float size;

    Fade_InOut fade;
    void Start()
    {
        gm = GameManager.instance;
        camera = GetComponent<Camera>();
        size = 12;

        fade = GameObject.Find("Fade").GetComponent<Fade_InOut>();
    }

    void Update()
    {
        //transform.parent.Rotate(Vector3.up * 50 * Time.deltaTime);
        if (fade.isFade)
        {
            if (!gm.isAllBallShot && !gm.isClear)
            {
                if (size >= 11.9f)
                {
                    camera.orthographicSize = 12;
                    return;
                }
                size = Mathf.Lerp(size, 12, 4 * Time.deltaTime);
                camera.orthographicSize = size;
            }
            else
            {
                if (size <= 9.1f)
                {
                    camera.orthographicSize = 9;
                    return;
                }
                size = Mathf.Lerp(size, 9, 4 * Time.deltaTime);
                camera.orthographicSize = size;
            }
        }
        
    }

}

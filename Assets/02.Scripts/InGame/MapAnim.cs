using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAnim : MonoBehaviour
{
    bool isStart, anim;
    List<Transform> objects = new List<Transform>();
    List<Transform> walls = new List<Transform>();

    Transform[] moveWalls = new Transform[2];

    void Start()
    {
        StartMapAnim();
    }

    public void StartMapAnim()
    {
        isStart = true;

        foreach (Transform tr in transform)
        {

            if (tr.TryGetComponent(out AnimType_Mono AM))
            {
                objects.Add(tr);

                //맵 오브젝트들 위치 초기화 및 할당
                AM.Initialize();
            }
        }

        foreach (Transform _tr in transform.Find("Walls"))
        {
            Vector3 vec = _tr.position;
            _tr.position = new Vector3(vec.x, -2f, vec.z);

            walls.Add(_tr);
        }

        //디버그하기 쉽게 만든 코드
        if (GameManager.instance.movingWall1==true)
        {
            //맵 열기
            StartCoroutine(MoveMovingWalls());
        }
       else StartCoroutine(LoadingWalls());

    }

    IEnumerator MoveMovingWalls()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            GameManager.instance.movingWall1.Translate(Vector3.back * 7 * Time.deltaTime);
            GameManager.instance.movingWall2.Translate(Vector3.forward * 7 * Time.deltaTime);
            if (GameManager.instance.movingWall1.position.z <= -24.5 && GameManager.instance.movingWall2.position.z >= 24.5)
            {
                GameManager.instance.movingWall1.gameObject.SetActive(false);
                GameManager.instance.movingWall2.gameObject.SetActive(false);
                break;
            }
        }
        StartCoroutine(LoadingWalls());
    }

    IEnumerator LoadingWalls()
    {

        for (int i = 0; i < walls.Count; i++)
        {
            //Debug.Log($"{walls[i].name} 실행");
            StartCoroutine(MoveUpWall(walls[i]));
            yield return new WaitForSeconds(0.12f);
        }
        StartCoroutine(LoadingMap());

    }

    IEnumerator MoveUpWall(Transform tf)
    {
        while (tf.position.y < 0.5f)
        {
            yield return new WaitForFixedUpdate();
            tf.position += (Vector3.up * 2 * Time.deltaTime);
        }

        if (tf.TryGetComponent(out AnimType_Mono AM))
        {
            if (AM.animType == AnimType_Mono.AnimType.Key)
            {
                AM.TriggerAnimBool();
            }
        }
    }



    IEnumerator LoadingMap()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            //Debug.Log($"{objects[i].name} 실행");

            objects[i].GetComponent<AnimType_Mono>().TriggerAnimBool();
            yield return new WaitForSeconds(0.17f);
        }
    }

    //private void Update()
    //{
    //    if (isStart)
    //    {
    //        foreach (Transform tr in objects)
    //        {
               
    //            ////벽 움직임
    //            //if(tr.gameObject.CompareTag("WALL")) tr.Translate(Vector3.right * 2 * Time.deltaTime);
    //            ////기믹 움직임
    //            //else tr.Translate(Vector3.up * 2 * Time.deltaTime);

    //            switch (tr.GetComponent<AnimType_Mono>().animType)
    //            {
    //                case AnimType_Mono.AnimType.Wall:
    //                    tr.Translate(Vector3.right * 2 * Time.deltaTime);
    //                    break;
    //                case AnimType_Mono.AnimType.Ball:
    //                    break;
    //                case AnimType_Mono.AnimType.Key:
    //                    break;
    //                case AnimType_Mono.AnimType.Cage:
    //                    break;
    //            }

    //        }
    //    }
       
    
}

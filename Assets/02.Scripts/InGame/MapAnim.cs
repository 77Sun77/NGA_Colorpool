using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAnim : MonoBehaviour
{
    bool isStart, anim;
    List<Transform> objects = new List<Transform>();

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
            Vector3 vec = tr.position;

            if (tr.TryGetComponent(out AnimType_Mono AM))
            {
                objects.Add(tr);

                //맵 오브젝트들 위치 초기화 및 할당
                AM.Initialize();
            }
        }
        

        //맵 열기
        for (int i = 0; i < 2; i++)
        {
            moveWalls[i] = GameObject.Find("MoveWall" + (i + 1)).transform;
        }
        StartCoroutine(MoveWalls());
       

    }

    IEnumerator MoveWalls()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            moveWalls[0].Translate(Vector3.back * 7 * Time.deltaTime);
            moveWalls[1].Translate(Vector3.forward * 7 * Time.deltaTime);
            if (moveWalls[0].position.z <= -24.5 && moveWalls[1].position.z >= 24.5)
            {
                moveWalls[0].gameObject.SetActive(false);
                moveWalls[1].gameObject.SetActive(false);
                break;
            }
        }
        StartCoroutine(LoadingMap());
    }

    IEnumerator LoadingMap()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].GetComponent<AnimType_Mono>().DoAnim();
            yield return new WaitForSeconds(0.12f);
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

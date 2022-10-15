using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAnimation : MonoBehaviour
{
    bool isStart, anim;
    List<Transform> objects = new List<Transform>();
    List<Transform> children = new List<Transform>();

    Transform[] moveWalls = new Transform[2];
    void Start()
    {
        isStart = true;
        anim = false;
        foreach (Transform tr in transform)
        {
            Vector3 vec = tr.position;
            tr.position = new Vector3(vec.x, -2f, vec.z);

            children.Add(tr);
        }
        for(int i=0; i<2; i++)
        {
            moveWalls[i] = GameObject.Find("MoveWall"+(i+1)).transform;
        }


        GameManager.instance.firstAnim = true;
        StartCoroutine(MoveWalls());
    }
    
    IEnumerator MoveWalls()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            moveWalls[0].Translate(Vector3.back * 7 * Time.deltaTime);
            moveWalls[1].Translate(Vector3.forward * 7 * Time.deltaTime);
            if (moveWalls[0].position.z <= -12 || moveWalls[1].position.z >= 12)
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
        for(int i=0; i< transform.childCount; i++)
        {
            objects.Add(transform.GetChild(i));
            yield return new WaitForSeconds(0.12f);
        }
        
    }

    private void Update()
    {
        if (isStart)
        {
            foreach (Transform tr in objects)
            {
                Vector3 vec = tr.position;
                if (objects.Count == children.Count)
                {
                    bool isMove = false;
                    foreach (Transform child in objects)
                    {
                        if (child.position.y >= 0.5f) isMove = false;
                        else
                        {
                            isMove = true;
                            break;
                        }
                    }
                    isStart = isMove;
                }
                
                if (vec.y >= 0.5f)
                {
                    tr.position = new Vector3(vec.x, 0.5f, vec.z);
                    continue;
                }
                if(tr.gameObject.CompareTag("WALL")) tr.Translate(Vector3.right * 2 * Time.deltaTime);
                else tr.Translate(Vector3.up * 2 * Time.deltaTime);
            }
        }
        else
        {
            if (!anim)
            {
                foreach(Ball ball in GameManager.instance.balls)
                {
                    ball.anim.SetTrigger("SizeAnim");
                    print("??");
                }
                anim = true;
            }
            
        }
        print(isStart);
    }
}

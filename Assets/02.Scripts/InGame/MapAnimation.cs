using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAnimation : MonoBehaviour
{
    bool isStart, anim;
    List<Transform> objects = new List<Transform>();
    List<Transform> children = new List<Transform>();
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
        print(children.Count);
        StartCoroutine(LoadingMap());
    }
    
    IEnumerator LoadingMap()
    {
        for(int i=0; i< transform.childCount; i++)
        {
            yield return new WaitForSeconds(0.12f);
            objects.Add(transform.GetChild(i));
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
                tr.Translate(Vector3.up * 2 * Time.deltaTime);
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

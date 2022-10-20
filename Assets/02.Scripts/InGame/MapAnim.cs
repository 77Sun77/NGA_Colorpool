using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAnim : MonoBehaviour
{
    bool isStart, anim;
    public List<Transform> objects = new List<Transform>();
    public List<Transform> walls = new List<Transform>();
    public List<Transform> paints=new List<Transform>();

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

                //�� ������Ʈ�� ��ġ �ʱ�ȭ �� �Ҵ�
                AM.Initialize();
            }
        }

        foreach (Transform _tr in transform.Find("Walls"))
        {
            //Walls���� ����Ʈ�� ã�� paints �迭�� ����
            if (_tr.TryGetComponent(out AnimType_Mono AM))
            {
                if (AM.animType == AnimType_Mono.AnimType.Paint)
                {
                    paints.Add(_tr);
                    AM.Initialize();
                    continue;
                }
            }

            Vector3 vec = _tr.position;
            _tr.position = new Vector3(vec.x, -2f, vec.z);
            walls.Add(_tr);
        }

        //������ϱ� ���� ���� �ڵ�
        if (GameManager.instance.movingWall1 == true)
        {
            //�� ����
            StartCoroutine(OpenMovingWalls());
        }
        else StartCoroutine(LoadingWalls());

    }

    IEnumerator OpenMovingWalls()
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

    //���������� ���� �ø�
    IEnumerator LoadingWalls()
    {

        for (int i = 0; i < walls.Count; i++)
        {
            //Debug.Log($"{walls[i].name} ����");
            StartCoroutine(MoveUpWall(walls[i]));
            yield return new WaitForSeconds(0.12f);
        }
        StartCoroutine(LoadingMap());
    }

    //���� �ø��� ����� �ϴ� �ڵ�
    IEnumerator MoveUpWall(Transform tf)
    {
        while (tf.position.y < 0.5f)
        {
            yield return new WaitForFixedUpdate();
            tf.position += (Vector3.up * 2 * Time.deltaTime);
        }

        //Ű �ִϸ��̼� ����
        if (tf.TryGetComponent(out AnimType_Mono AM))
        {
            if (AM.animType == AnimType_Mono.AnimType.Key)
            {
                AM.Initialize();
                AM.TriggerAnimBool();
            }
        }
    }


    //���� ������ �ٸ� ������Ʈ���� ������
    IEnumerator LoadingMap()
    {
        for (int i = 0; i < paints.Count; i++)
        {
            paints[i].GetComponent<AnimType_Mono>().TriggerAnimBool();
            yield return new WaitForSeconds(0.17f);
        }

        for (int i = 0; i < objects.Count; i++)
        {
            //Debug.Log($"{objects[i].name} ����");
            objects[i].GetComponent<AnimType_Mono>().TriggerAnimBool();
            yield return new WaitForSeconds(0.17f);
        }
    }



    [ContextMenu("EndAnim")]
    void EndMapAnim()
    {
        StartCoroutine(EndMapLoad());
    }

    IEnumerator EndMapLoad()
    {
        List<Transform> tempList = new();
        foreach (Transform obj in objects)
        {
            if (obj.TryGetComponent(out AnimType_Mono AM))
            {
                if (AM.animType == AnimType_Mono.AnimType.Ball)
                {
                    Debug.Log("BallAnim");
                    // obj.GetComponent<Animator>().Set;
                    obj.GetComponent<Animator>().SetTrigger("SizeAnim_Re");
                }
                else
                {
                    tempList.Add(obj);
                }
            }
            else
            {
                tempList.Add(obj);
            }
        }

        foreach (Transform obj in tempList)
        {
            StartCoroutine(MoveDownWall(obj));
            yield return new WaitForSeconds(0.12f);
        }

        foreach (Transform paint in paints)
        {
            StartCoroutine(MoveDownWall(paint));
            yield return new WaitForSeconds(0.12f);
        }


            foreach (Transform wall in walls)
            {
            //Ű �ִϸ��̼��� ��Ȱ��ȭ
            if (wall.TryGetComponent(out AnimType_Mono AM))
            {
                if (AM.animType == AnimType_Mono.AnimType.Key)
                {
                    AM.UnTriggerAnimBool();
                }
            }
            StartCoroutine(MoveDownWall(wall));
            yield return new WaitForSeconds(0.12f);
            }

    }

    IEnumerator MoveDownWall(Transform tf)
    {
        while (tf.position.y > -2f)
        {
            yield return new WaitForFixedUpdate();
            Debug.Log($"{tf.name} moveDown ����");
            tf.position -= (Vector3.up * 3 * Time.deltaTime);
        }
        yield return new WaitForSeconds(2f);

        StartCoroutine(CloseMovingWalls());
    }

    IEnumerator CloseMovingWalls()
    {
        GameManager.instance.movingWall1.gameObject.SetActive(true);
        GameManager.instance.movingWall2.gameObject.SetActive(true);
        while (GameManager.instance.movingWall1.position.z <= -20 && GameManager.instance.movingWall2.position.z >= 20)
        {
            yield return new WaitForFixedUpdate();
            GameManager.instance.movingWall2.Translate(Vector3.back * 7 * Time.deltaTime);
            GameManager.instance.movingWall1.Translate(Vector3.forward * 7 * Time.deltaTime);
            
        }
    }
}

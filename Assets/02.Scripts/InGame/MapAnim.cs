using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAnim : MonoBehaviour
{
    bool isStart;
    public bool isAnim;
    public List<Transform> objects = new List<Transform>();
    public List<Transform> walls = new List<Transform>();
    public List<Transform> paints=new List<Transform>();

    Transform[] moveWalls = new Transform[2];

    float fastAnimIndex=1;
    void Start()
    {
        StartMapAnim();
        //Time.timeScale = 2.0f;
    }

    private void Update()
    {
        if (!isAnim)
            Time.timeScale = 1.0f;
    }

    public void StartMapAnim()
    {
        isStart = true;
        isAnim = true;

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
            //Walls에서 페인트를 찾아 paints 배열에 넣음
            if (_tr.TryGetComponent(out AnimType_Mono AM))
            {
                if (AM.animType == AnimType_Mono.AnimType.Paint)
                {
                    paints.Add(_tr);
                    AM.Initialize();
                    continue;
                }
                else if(AM.animType == AnimType_Mono.AnimType.Ball) _tr.localScale = new Vector3(_tr.localScale.x, 0.99f, _tr.localScale.z);
            }
            if(_tr.GetComponent<Wall_ColorReflect>() != null) _tr.localScale = new Vector3(_tr.localScale.x, 0.99f, _tr.localScale.z);


            Vector3 vec = _tr.position;
            _tr.position = new Vector3(vec.x, -1f, vec.z);
            walls.Add(_tr);
        }

        //디버그하기 쉽게 만든 코드
        if (GameManager.instance.movingWall1 == true)
        {
            //맵 열기
            StartCoroutine(OpenMovingWalls());
        }
        else StartCoroutine(LoadingWalls());

    }

    IEnumerator OpenMovingWalls()
    {
        if(GameManager.moveScene == "Lobby" || GameManager.moveScene == null)
        {
            Fade_InOut fade = GameObject.Find("Fade").GetComponent<Fade_InOut>();
            fade.ChangeFade(Fade_InOut.Fade.Fade_In);
            while (!fade.isFade) yield return new WaitForFixedUpdate();

        }

        Transform wall1 = GameManager.instance.movingWall1;
        Transform wall2 = GameManager.instance.movingWall2;
        while (true)
        {
            yield return new WaitForFixedUpdate();
            wall1.Translate(Vector3.left * 3.5f * Time.deltaTime);
            wall2.Translate(Vector3.right * 3.5f * Time.deltaTime);
            wall1.localScale -= new Vector3(1f, 0, 0) * 7 * Time.deltaTime;
            wall2.localScale -= new Vector3(1f, 0, 0) * 7 * Time.deltaTime;

            if (wall1.localScale.x <= 0 && wall2.localScale.x <= 0)
            {
                wall1.gameObject.SetActive(false);
                wall2.gameObject.SetActive(false);
                break;
            }
        }

        UIManager.instance.DoBallUIAnim();

        StartCoroutine(LoadingWalls());
    }

    IEnumerator OpenMovingWalls_Fast()
    {
        if (GameManager.moveScene == "Lobby" || GameManager.moveScene == null)
        {
            Fade_InOut fade = GameObject.Find("Fade").GetComponent<Fade_InOut>();
            fade.ChangeFade(Fade_InOut.Fade.Fade_In);
            while (!fade.isFade) yield return new WaitForFixedUpdate();

        }

        Transform wall1 = GameManager.instance.movingWall1;
        Transform wall2 = GameManager.instance.movingWall2;
        while (true)
         {
            wall1.Translate(Vector3.left * 3.5f);
            wall2.Translate(Vector3.right * 3.5f);
            wall1.localScale -= new Vector3(1f, 0, 0) * 7;
            wall2.localScale -= new Vector3(1f, 0, 0) * 7;

            if (wall1.localScale.x <= 0 && wall2.localScale.x <= 0)
            {
                wall1.gameObject.SetActive(false);
                wall2.gameObject.SetActive(false);
                break;
            }
         }

        UIManager.instance.DoBallUIAnim();

        StartCoroutine(LoadingWalls());
    }

    //순차적으로 벽을 올림
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

    IEnumerator LoadingWalls_Fast()
    {

        for (int i = 0; i < walls.Count; i++)
        {
            //Debug.Log($"{walls[i].name} 실행");
            StartCoroutine(MoveUpWall_Fast(walls[i]));
        }
        StartCoroutine(LoadingMap());
        yield return null;
    }

    //벽을 올리는 기능을 하는 코드
    IEnumerator MoveUpWall(Transform tf)
    {
        while (tf.position.y < 0.5f)
        {
            yield return new WaitForFixedUpdate();
            tf.position += (Vector3.up * 2 * Time.deltaTime);
        }

        tf.position = new Vector3(tf.position.x, 0.5f, tf.position.z);

        //키 애니메이션 실행
        if (tf.TryGetComponent(out AnimType_Mono AM))
        {
            if (AM.animType == AnimType_Mono.AnimType.Key)
            {
                AM.Initialize();
                AM.TriggerAnimBool();
            }
        }
    }
   

    //벽을 올리는 기능을 하는 코드
    IEnumerator MoveUpWall_Fast(Transform tf)
    {
        //while (tf.position.y < 0.5f)
        //{
        //    yield return new WaitForFixedUpdate();
        //    tf.position += (Vector3.up * 2 * Time.deltaTime);
        //}

        tf.position = new Vector3(tf.position.x, 0.5f, tf.position.z);

        //키 애니메이션 실행
        if (tf.TryGetComponent(out AnimType_Mono AM))
        {
            if (AM.animType == AnimType_Mono.AnimType.Key)
            {
                AM.Initialize();
                AM.TriggerAnimBool();
            }
        }

        yield return null;
    }

    //벽을 제외한 다른 오브젝트들을 실행함
    IEnumerator LoadingMap()
    {
        for (int i = 0; i < paints.Count; i++)
        {
            paints[i].GetComponent<AnimType_Mono>().TriggerAnimBool();
            yield return new WaitForSeconds(0.17f);
        }

        for (int i = 0; i < objects.Count; i++)
        {
            //Debug.Log($"{objects[i].name} 실행");
            objects[i].GetComponent<AnimType_Mono>().TriggerAnimBool();
            yield return new WaitForSeconds(0.17f);
        }
        yield return new WaitForSeconds(0.5f);
        GameManager.instance.isStart = true;
        isAnim = false;
    }



    [ContextMenu("EndAnim")]
    public void EndMapAnim()
    {
        StartCoroutine(EndMapLoad());
        isAnim = true;
    }

    IEnumerator EndMapLoad()
    {
        List<Transform> tempList = new();
        objects.Clear();
        foreach (Transform tr in transform)
        {

            if (tr.TryGetComponent(out AnimType_Mono AM))
            {
                objects.Add(tr);

                //맵 오브젝트들 위치 초기화 및 할당
                AM.Initialize();
            }
        }
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

        walls.Clear();
        foreach (Transform _tr in transform.Find("Walls"))
        {
            walls.Add(_tr);
        }
        foreach (Transform wall in walls)
        {
                //키 애니메이션을 비활성화
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
        Transform wall1 = GameManager.instance.movingWall1;
        Transform wall2 = GameManager.instance.movingWall2;
        wall1.gameObject.SetActive(true);
        wall2.gameObject.SetActive(true);
        while (true)
        {
            yield return new WaitForFixedUpdate();
            wall1.Translate(Vector3.right * 5f * Time.deltaTime);
            wall2.Translate(Vector3.left * 5f * Time.deltaTime);
            wall1.localScale += new Vector3(1f, 0, 0) * 10 * Time.deltaTime;
            wall2.localScale += new Vector3(1f, 0, 0) * 10 * Time.deltaTime;

            if (wall1.localScale.x >= 10 && wall2.localScale.x >= 10)
            {
                break;
            }
        }
        GameManager.instance.ColorEnable();
    }

    IEnumerator MoveDownWall(Transform tf)
    {
        while (tf.position.y > -2f)
        {
            yield return new WaitForFixedUpdate();
            Debug.Log($"{tf.name} moveDown 실행");
            tf.position -= (Vector3.up * 3 * Time.deltaTime);
        }
        yield return new WaitForSeconds(2f);

        isAnim = false;
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

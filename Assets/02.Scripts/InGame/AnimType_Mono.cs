using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimType_Mono : MonoBehaviour
{
    public Material[] materials;


    public float seconds;

    public enum AnimType { Wall, Ball, Key, Cage, Paint }
    public AnimType animType;

    public bool isAnimWall;
    public bool isAnimBall;
    public bool isAnimKey;
    public bool isAnimCage;
    public bool isAnimPaint;

  

    GameObject brushPrefab_Ins;
    public bool isBrushSpawned;
    public bool isBrushMoving;
   public float curTime;

    Transform CCW_TF;

    Vector3 keyPos;
    public bool isDownCount;
    public bool isUpCount;

    private void Awake()
    {
        if (animType == AnimType.Cage)
        {
            Initialize();
        }

    }



    [ContextMenu("초기화")]
    public void Initialize()
    {
        switch (animType)
        {
            //case AnimType.Wall:
            //    transform.position = new Vector3(transform.position.x, -2f, transform.position.z);
            //    break;
            case AnimType.Ball:
                //transform.position.Scale(Vector3.one / 10);
                break;
            case AnimType.Key:
                keyPos = transform.position;
                isUpCount = true;
                break;
            case AnimType.Cage:
                switch (transform.GetChild(0).gameObject.name)
                {
                    case "NewCage_Circle_C":
                        CageMaterial(0);
                        break;
                    case "NewCage_Heart_C":
                        CageMaterial(1);
                        break;
                    case "NewCage_Square_C":
                        CageMaterial(2);
                        break;
                    case "NewCage_Star_C":
                        CageMaterial(3);
                        break;
                    case "NewCage_Triangle_C":
                        CageMaterial(4);
                        break;
                }

                


                break;
            case AnimType.Paint:
                CCW_TF = transform.Find("Square_Offset");
                //Vector3 colorScale = CCW_TF.localScale;
                //CCW_TF.localScale = new Vector3(0, colorScale.y, colorScale.z);
                Debug.Log("페인트 초기화");
                break;
        }

    }

    [ContextMenu("TriggerAnimBool")]
    public void TriggerAnimBool()
    {
        //AnimType에 따라 bool 값을 활성화시키는 함수
        this.GetType().GetField($"isAnim{animType}").SetValue(this, true);

    }

    public void UnTriggerAnimBool()
    {
        //AnimType에 따라 bool 값을 활성화시키는 함수
        this.GetType().GetField($"isAnim{animType}").SetValue(this, false);
        Debug.Log("언트리거");

    }

    public void DoAnim()
    {
 
        if (isAnimBall)
        {
            //transform.localScale = Vector3.Lerp(Vector3.one * 0.1f, Vector3.one, curTime/3);
            //curTime += Time.deltaTime;
            //transform.GetComponent<Animator>().SetBool("SizeAnim2",true);
            //transform.GetComponent<Animator>().SetTrigger("SizeAnim2");
            transform.GetComponent<Animator>().SetTrigger("SizeAnim2");


            isAnimBall = false;
        }

        if (isAnimKey)
        {

            if (curTime <= 0)
            {
                MoveUp();
            }
            else if (curTime >= 1f)
            {
                MoveDown();
            }
          
                if (isDownCount)
                {
                    curTime -= Time.deltaTime;
                }
                else if (isUpCount)
                {
                    curTime += Time.deltaTime;
                }
            
            transform.position = Vector3.Lerp(keyPos, keyPos + Vector3.up/3, curTime);
            transform.Rotate(Vector3.up*20 * Time.deltaTime);
        }

        if (isAnimPaint)
        {
            StartCoroutine(nameof(DoAnim_Paint));

            if (isBrushMoving)
            {
                brushPrefab_Ins.transform.position = Vector3.Lerp(transform.Find("StartPoint").position, transform.Find("EndPoint").position, curTime*1.5f);
                Vector3 colorScale = CCW_TF.localScale;
                CCW_TF.localScale = Vector3.Lerp(Vector3.one, new Vector3(0, colorScale.y, colorScale.z), curTime * 1.5f); 

                curTime += Time.deltaTime;
            }

        }

    }

 void MoveDown()
    {
        isUpCount = false;
        isDownCount = true;
    }

    void MoveUp()
    {
        isUpCount = true;
        isDownCount = false;
    }


    IEnumerator DoAnim_Paint()
    {
        if (!isBrushSpawned)
        {
            brushPrefab_Ins = Instantiate(GameManager.instance.paintBrush_Prefab, transform.Find("StartPoint").position, Quaternion.identity);
            brushPrefab_Ins.GetComponent<Animator>().SetBool("isFadeIn", true);
            Debug.Log("페이드인");
            isBrushSpawned = true;
            yield return new WaitForSeconds(0.1f);
            isBrushMoving = true;
        }


        if (brushPrefab_Ins.transform.position == transform.Find("EndPoint").position)
        {
            if (brushPrefab_Ins.GetComponent<Animator>().GetBool("isFadeOut")==false)
            {
                Debug.Log("페이드아웃");
                brushPrefab_Ins.GetComponent<Animator>().SetBool("isFadeOut", true);
                yield return new WaitForSeconds(0.2f);
                isAnimPaint =false;
                Destroy(brushPrefab_Ins);
            }
        }
    }

    void ReUse()
    {
        brushPrefab_Ins.GetComponent<Animator>().SetBool("isReset", true);
        brushPrefab_Ins.GetComponent<Animator>().SetBool("isFadeOut", false);
        brushPrefab_Ins.GetComponent<Animator>().SetBool("isFadeIn", false);

    }



    private void Update()
    {
        DoAnim();
    }


    void CageMaterial(int num)
    {
        foreach (Transform child in transform)
        {
            foreach (Transform tf in child)
            {
                tf.GetComponent<MeshRenderer>().material = materials[num];
            }
        }
    }


   
}

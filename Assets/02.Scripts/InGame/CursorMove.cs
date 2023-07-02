using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CursorMove : MonoBehaviour
{
    public Transform EndPos;
    Vector3 startPos, endPos;

    Image Img;
    void Start()
    {
        Img = GetComponent<Image>();
        startPos = transform.position;
        endPos = EndPos.position;

        StartCoroutine(DoCursorAnim());
    }
    IEnumerator MoveCursor()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();

            if(transform.position.z >= endPos.z+0.1f)
            {
                transform.position= Vector3.Lerp(transform.position, endPos, 7 * Time.deltaTime);
            }
            else
            {
                yield return new WaitForSeconds(1.5f);
                Vector3 pos = transform.position;
                transform.position = startPos;
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    IEnumerator MoveCursor2()
    {
        while (true)
        {
            transform.DOMove(endPos, 1.4f);
            
            yield return new WaitForSeconds(1.5f);

            transform.DOMove(startPos, 0.4f);
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator DoCursorAnim()
    {
        float moveDuration = 0.85f;

        while(true)
        {
            float fadeOutTime = 0f;
            transform.DOMove(endPos, moveDuration);
            Color imgColor = default;

            while (fadeOutTime < moveDuration)
            {
                fadeOutTime += Time.deltaTime;
                imgColor = Img.color;
                imgColor = new Color(imgColor.r, imgColor.g, imgColor.b,1 - fadeOutTime/2);
                Img.color = imgColor;
                yield return new WaitForFixedUpdate();
            }
            //yield return new WaitForSeconds(0.3f);

            transform.DOMove(startPos, 0.4f);
            Img.color = new Color(imgColor.r, imgColor.g, imgColor.b, 1);
            yield return new WaitForSeconds(0.5f);
        }

    }



}

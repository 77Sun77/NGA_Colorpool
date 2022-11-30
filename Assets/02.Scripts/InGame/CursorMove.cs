using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorMove : MonoBehaviour
{
    public Transform EndPos;
    Vector3 startPos, endPos;
    void Start()
    {
        StartCoroutine(MoveCursor());
        startPos = transform.position;
        endPos = EndPos.position;
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
}

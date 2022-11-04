using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LogoAnim : MonoBehaviour
{
    public Transform Objects;

    public void MoveUpObjs()
    {
        StartCoroutine(nameof(MoveUpObjs_Cor));
    }

    IEnumerator MoveUpObjs_Cor()
    {
        for (int i = 0; i < Objects.childCount; i++)
        {
            //Objects.GetChild(i).DOMoveZ();
            yield return new WaitForSeconds(0.1f);
        }
    }

}

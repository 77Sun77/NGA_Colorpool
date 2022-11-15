using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFade : MonoBehaviour
{
    Text text;
    float alpha;
    bool isUp;
    void Start()
    {
        text = GetComponent<Text>();
        alpha = 0;
        isUp = true;
        StartCoroutine(TextFadeInOut());
    }

    IEnumerator TextFadeInOut()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            Color c = text.color;
            if (isUp)
            {
                alpha += Time.deltaTime * 1.7f;
                if(alpha >= 1)
                {
                    alpha = 1;
                    isUp = false;
                }
            }
            else
            {
                alpha -= Time.deltaTime * 1.7f;
                if (alpha <= 0)
                {
                    alpha = 0;
                    isUp = true;
                }
            }
            c.a = alpha;
            text.color = c;
        }
    }
}

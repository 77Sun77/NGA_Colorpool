using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade_InOut : MonoBehaviour
{
    public Image FadeIn_Img, FadeOut_Img;
    public enum Fade{ None, Fade_In, Fade_Out };
    public Fade fade_InOut;

    public bool isFade; // False일때 실행 중 / True일때 실행 완료

    void Update()
    {
        Fade_In();
        Fade_Out();
    }

    void Fade_In()
    {
        if(fade_InOut == Fade.Fade_In)
        {
            Color color = FadeIn_Img.color;
            color.a -= 1 * Time.deltaTime;
            FadeIn_Img.color = color;
            if (FadeIn_Img.color.a <= 0)
            {
                color.a = 0;
                FadeIn_Img.color = color;
                fade_InOut = Fade.None;
                isFade = true;
                FadeIn_Img.gameObject.SetActive(false);
            }
        }
    }
    
    void Fade_Out()
    {
        if (fade_InOut == Fade.Fade_Out)
        {
            Color color = FadeOut_Img.color;
            color.a += 1 * Time.deltaTime;
            FadeOut_Img.color = color;
            if(FadeOut_Img.color.a >= 1)
            {
                color.a = 1;
                FadeOut_Img.color = color;
                fade_InOut = Fade.None;
                isFade = true;
            }
        }
    }

    public void ChangeFade(Fade fade)
    {
        isFade = false;
        Color color = FadeOut_Img.color;
        if (fade == Fade.Fade_In)
        {
            color.a = 1;
            FadeIn_Img.color = color;
            FadeIn_Img.gameObject.SetActive(true);
        }
        else if(fade == Fade.Fade_Out)
        {
            color.a = 0;
            FadeOut_Img.color = color;
            FadeOut_Img.gameObject.SetActive(true);
        }
        fade_InOut = fade;
    }
}

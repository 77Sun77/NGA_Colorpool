using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class TeamLogo : MonoBehaviour
{
    public Image backGround;

   public void DoSound()
    {
        transform.GetComponent<AudioSource>().Play();
    }

    public void DoFadeOut()
    {
        transform.GetComponent<Image>().DOFade(0, 2);
        backGround.GetComponent<Image>().DOFade(0, 2);
    }

}

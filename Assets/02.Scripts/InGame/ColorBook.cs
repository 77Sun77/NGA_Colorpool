using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBook : MonoBehaviour
{
    public GameObject[] parents;
    public int index;
    public GameObject[] text;
    void Start()
    {
        index = 0;
        //if (GameManager.stageLV + 1 == 1)
        //{
        //    text[0].SetActive(false);
        //    text[1].SetActive(true);
        //}
    }

    public void OnClick_Left()
    {
        parents[index].SetActive(false);
        if(index == 0)
        {
            index = 3;
        }
        else
        {
            index--;
        }
        parents[index].SetActive(true);
    }
    public void OnClick_Right()
    {
        parents[index].SetActive(false);
        if (index == 3)
        {
            index = 0;
        }
        else
        {
            index++;
        }
        parents[index].SetActive(true);
    }

    public void Quit()
    {
        //if (GameManager.stageLV + 1 == 1)
        //{
        //    text[1].SetActive(false);
        //    text[2].SetActive(true);
        //}
        gameObject.SetActive(false);
        
    }
    
}

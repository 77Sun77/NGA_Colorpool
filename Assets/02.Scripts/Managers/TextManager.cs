using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    public GameObject TextBox_Top;
    public GameObject TextBox_Bottom;
    public ColorBook colorBook;
    public string[] texts;

    public Text Text_Top;
    public Text Text_Bottom;

    bool TutoClear;
  
    private void Awake()
    {
        Text_Top = TextBox_Top.GetComponentInChildren<Text>();
        Text_Bottom = TextBox_Bottom.GetComponentInChildren<Text>();

        TutoClear = false;
       
        if (GameManager.stageLV == 0)
        {
            TextBox_Top.SetActive(true);
            Text_Top.text = texts[0];
        }
        else if (GameManager.stageLV == 1)
        {
            TextBox_Top.SetActive(true);
            Text_Top.text = texts[3];
        }
        else if (GameManager.stageLV == 2)
        {
            TextBox_Top.SetActive(true);
            Text_Top.text = texts[4];
        }
    }

    private void Update()
    {
        if (GameManager.stageLV == 0)
        {
            if (colorBook.gameObject.activeSelf)
            {
                Text_Top.text = texts[1];
                TutoClear = true;
            }

            if (TutoClear && !colorBook.gameObject.activeSelf)
            {
                TextBox_Top.SetActive(false);
                TextBox_Bottom.SetActive(true);
                Text_Bottom.text = texts[2];
            }

            GameManager.instance.isAllBallShot = !TutoClear;

            if (GameManager.instance.isClear) TextBox_Bottom.SetActive(false);
        }
        else if (GameManager.stageLV == 1)
        {
            if (colorBook.gameObject.activeSelf && !TutoClear)
            {
                TutoClear = true;
                colorBook.OnClick_Right();
            }

            GameManager.instance.isAllBallShot = !TutoClear;
            if (GameManager.instance.isClear) TextBox_Top.SetActive(false);
        }
        else if (GameManager.stageLV == 2)
        {
            if (colorBook.gameObject.activeSelf && !TutoClear)
            {
                TutoClear = true;
                for (int i = 0; i < 2; i++) colorBook.OnClick_Right();
            }
                
            GameManager.instance.isAllBallShot = !TutoClear;
            if (GameManager.instance.isClear) TextBox_Top.SetActive(false);
        }
    }


}

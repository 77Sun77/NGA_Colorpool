using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    public TextMeshProUGUI Text_Top;
    public TextMeshProUGUI Text_Bottom;
    public ColorBook colorBook;
    public string[] texts;

    bool TutoClear;

    private void Start()
    {
        TutoClear = false;


        StartCoroutine(DoPopUp(0, Text_Top, texts[0]));
        StartCoroutine(DoPopUp(1, Text_Top, texts[3]));
        StartCoroutine(DoPopUp(2, Text_Top, texts[4]));
        StartCoroutine(DoPopUp(4, Text_Top, texts[5]));
        StartCoroutine(DoPopUp(5, Text_Top, texts[6]));
    }

    IEnumerator DoPopUp(int targetStageLV, TextMeshProUGUI textBox, string showingText)
    {
        if (GameManager.stageLV != targetStageLV)
            yield break;

        Debug.Log("DoPopUp" + GameManager.stageLV);

        textBox.transform.parent.gameObject.SetActive(true);
        textBox.text = showingText;


        yield return new WaitUntil(() => { return GameManager.instance.isClear; });

        textBox.transform.parent.gameObject.SetActive(false);
        Debug.Log("DoPopUpEnd");
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
                Text_Top.gameObject.SetActive(false);
                Text_Bottom.gameObject.SetActive(true);
                Text_Bottom.text = texts[2];
            }

            GameManager.instance.isAllBallShot = !TutoClear;

            if (GameManager.instance.isClear) Text_Bottom.gameObject.SetActive(false);
        }
        else if (GameManager.stageLV == 1)
        {
            if (colorBook.gameObject.activeSelf && !TutoClear)
            {
                TutoClear = true;
                colorBook.OnClick_Right();
            }

            GameManager.instance.isAllBallShot = !TutoClear;
        }
        else if (GameManager.stageLV == 2)
        {
            if (colorBook.gameObject.activeSelf && !TutoClear)
            {
                TutoClear = true;
                for (int i = 0; i < 2; i++) colorBook.OnClick_Right();
            }

            GameManager.instance.isAllBallShot = !TutoClear;
        }
        else if (GameManager.stageLV == 5)
        {
            if (colorBook.gameObject.activeSelf && !TutoClear)
            {
                TutoClear = true;
                for (int i = 0; i < 3; i++) colorBook.OnClick_Right();
            }
            GameManager.instance.isAllBallShot = !TutoClear;
        }
    }


}

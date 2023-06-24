using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TextManager : MonoBehaviour
{
    public static TextManager instance;

    public TextMeshProUGUI Text_Top;
    public TextMeshProUGUI Text_Bottom;
    public ColorBook colorBook;
    public string[] texts;

    
    public bool HasTuto,TutoClear, TutoClear2;

    public bool detectCondition = false;
    
    public unsafe bool* BoolMemoryPos;

    Coroutine coroutine;
    private void Awake()
    {
        instance = this;
    }

    private unsafe void Start()
    {
        TutoClear = false;
       
        DoPopUp(0, Text_Top, texts[0], ref TutoClear2);
        DoPopUp(1, Text_Top, texts[3], ref GameManager.instance.isClear);
        DoPopUp(2, Text_Top, texts[4], ref GameManager.instance.isClear);
        DoPopUp(4, Text_Top, texts[5], ref GameManager.instance.isClear);
        DoPopUp(5, Text_Top, texts[6], ref TutoClear);

        if(!HasTuto) TutoClear = true;
    }

    unsafe void DoPopUp(int targetStageLV, TextMeshProUGUI textBox, string showingText, ref bool _colorCloseCondition)
    {
        if (GameManager.stageLV != targetStageLV|| _colorCloseCondition==true)
            return;

        fixed (bool* b = &_colorCloseCondition) BoolMemoryPos = b;

        coroutine = StartCoroutine(DoPopUp_Cor(targetStageLV, textBox, showingText));

        HasTuto = true;
    }

  
    IEnumerator DoPopUp_Cor(int targetStageLV, TextMeshProUGUI textBox, string showingText)
    {
        Debug.Log("DoPopUp" + GameManager.stageLV + textBox.name);
        
        textBox.transform.parent.gameObject.SetActive(true);
        textBox.transform.parent.DOScale(Vector3.zero, 0f);
        textBox.transform.parent.DOScale(Vector3.one, 0.3f);

        textBox.text = showingText;

        yield return new WaitUntil(() => { return detectCondition; });
        coroutine = null;
        detectCondition = false;

        float animDur = 0.3f;
        textBox.transform.parent.DOScale(Vector3.zero, animDur);
        
        yield return new WaitForSeconds(animDur);
        textBox.transform.parent.gameObject.SetActive(false);
       
        Debug.Log("DoPopUpEnd");
    }

    private unsafe void Update()
    {
        if (GameManager.stageLV == 0)
        {
            if (!TutoClear) GameManager.instance.CanMoveBall = false;

            if (colorBook.gameObject.activeSelf)
            {
                Text_Top.text = texts[1];
                TutoClear = true;
            }

            if (TutoClear && !colorBook.gameObject.activeSelf)
            {
                TutoClear2 = true;
                if (coroutine==null) DoPopUp(0, Text_Bottom, texts[2], ref GameManager.instance.isClear);               
            }
        }
        else if (GameManager.stageLV == 1)
        {
            if (!TutoClear) GameManager.instance.CanMoveBall = false;

            if (colorBook.gameObject.activeSelf && !TutoClear)
            {
                TutoClear = true;
                colorBook.OnClick_Right();
            }  
        }
        else if (GameManager.stageLV == 2)
        {
            if (!TutoClear) GameManager.instance.CanMoveBall = false;

            if (colorBook.gameObject.activeSelf && !TutoClear)
            {
                TutoClear = true;
                for (int i = 0; i < 2; i++) colorBook.OnClick_Right();
            }
        }
        else if (GameManager.stageLV == 5)
        {
            if (!TutoClear) GameManager.instance.CanMoveBall = false;

            if (GameManager.instance.clickMove2.isClicking) Text_Top.text = texts[7];

            if (colorBook.gameObject.activeSelf && !TutoClear)
            {
                TutoClear = true;
                for (int i = 0; i < 3; i++) colorBook.OnClick_Right();
            }
        }

        if (BoolMemoryPos != null) detectCondition = *BoolMemoryPos;
    }

  
}

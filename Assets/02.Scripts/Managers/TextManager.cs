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
    public Button ColorBookButton;
    public ColorBook colorBook;
    public GameObject FingerImg;
    public CameraMove cameraMove;
    public string[] texts;
    [Header("Mask")]
    public GameObject UI_TutorialBG;
    public GameObject MenuMask;
    public GameObject RotLockMask;
    public GameObject ReStartMask;

    public Button Button_Lobby;
    public Button Button_Replay;
    public Button Button_RotLock;

    public static bool IsRestarted;
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

        if (GameManager.stageLV == 0 && !IsRestarted)
        {
            HasTuto = true;
            ColorBookButton.interactable = false;
            StartCoroutine(Start_Cor());
        }
        else if(IsRestarted) DoPopUp(0, Text_Top, texts[0], ref TutoClear2);
      
        DoPopUp(1, Text_Top, texts[3], ref TutoClear);
        DoPopUp(2, Text_Top, texts[4], ref TutoClear);
        DoPopUp(4, Text_Top, texts[5], ref TutoClear);
        DoPopUp(5, Text_Top, texts[6], ref TutoClear);
        DoPopUp(6, Text_Top, texts[8], ref TutoClear);

        if (!HasTuto) TutoClear = true;
    }

    IEnumerator Do_Menu_Tuto()
    {
        yield return new WaitUntil(() => { return UIManager.instance; });
        UIManager.instance.OpenMenu();
        
        yield return new WaitUntil(() => UIManager.instance.Camera.GetComponent<CameraMove>().isLocked);
        Text_Top.text = texts[10];
    }

    IEnumerator Start_Cor()
    {
        yield return new WaitUntil(() => { return GameManager.instance.clickMove2.mapAnim; });
        yield return new WaitUntil(() => { return GameManager.instance.clickMove2.mapAnim.isAnim == false; });
        FingerImg.SetActive(true);
        DoPopUp(0, Text_Top, texts[11], ref TutoClear2);
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

        textBox.transform.parent.localScale = Vector3.zero;
        textBox.transform.parent.gameObject.SetActive(true);
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

            if(!IsRestarted)
            {
                if (cameraMove.IsRotated && GameManager.instance.clickMove2.isClicking == false)
                {
                    FingerImg.SetActive(false);
                    UI_TutorialBG.SetActive(true);
                    Text_Top.text = texts[9];
                    MenuMask.SetActive(true);
                }

                if (UIManager.instance.isMenuOpen)
                {
                    MenuMask.SetActive(false);
                    RotLockMask.SetActive(true);

                    Button_Lobby.interactable = false;
                    Button_Replay.interactable = false;
                    Button_RotLock.interactable = true;
                }

                if (UIManager.instance.Camera.GetComponent<CameraMove>().isLocked)
                {
                    RotLockMask.SetActive(false);
                    ReStartMask.SetActive(true);

                    Text_Top.text = texts[10];
                    Button_Replay.interactable = true;
                }
            }

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
        else if (GameManager.stageLV == 4)
        {
            if (!TutoClear) GameManager.instance.CanMoveBall = false;
            if (GameManager.instance.clickMove2.isClicking) TutoClear = true;
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
        else if (GameManager.stageLV == 6)
        {
            if (GameManager.instance.clickMove2.isClicking) TutoClear = true;
        }

        if (BoolMemoryPos != null) detectCondition = *BoolMemoryPos;
    }

  
}

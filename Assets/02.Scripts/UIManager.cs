using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject UI_TargetPlace;
    public GameObject[] UI_ballImagePrefabs;
    public GameObject UI_CheckImagePrefab;

    public GameObject UI_ScoreBoard;
    public GameObject[] UI_Stars;

    public TextMeshProUGUI targetText;
    public TextMeshProUGUI shotText;

    [HideInInspector]
    public List<GameObject> InstantiatedObj;

    public List<GameObject> UI_BallImg;
    /// <summary>
    /// BallImg에 체크표시 여부담은 List
    /// </summary>
    public List<bool> BallChecked;


    public int imgCount;
    public int ballIndex;

    public int h_interval = 90;
    public int v_interval;

    public bool isOnScoreBoard, isMenuOpen, isClick;
    public DropDown Menu;

    Dictionary<GameObject, string> colorImages = new Dictionary<GameObject, string>();
    List<GameObject> check_Object = new List<GameObject>();

    public Transform targetBG;
    public GameObject leftCircle, rightCircle;
    public GameObject image;

    public GameObject Camera;
    public GameObject[] UI_CamLockImgs;

    float SceneLoadOper_F;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        isMenuOpen = false;
        isClick = false;
        UI_CamLockImgs[0].SetActive(SoundManager.instance.isCamLocked);
        UI_CamLockImgs[1].SetActive(!SoundManager.instance.isCamLocked);
    }
    private void Update()
    {
        //ShowTargetsOnUI();
    }

    [ContextMenu("Test")]
    void ShowTargetsOnUI()
    {
        //Debug.Log("Test");

        foreach (GameObject go in InstantiatedObj)
        {
            Destroy(go);
        }
        InstantiatedObj.Clear();
        BallChecked.Clear();
        UI_BallImg.Clear();

        SetImgCount();

        for (int i = 0; i < imgCount; i++)
        {
            BallChecked.Add(false);
        }

        CheckSuccessBall();

        SetBallUI();
        SetCheckUI();

    }

    public void Set_Target_Img()
    {
        int count = GameManager.instance.targetList.Count;
        for (int i = 0; i < count; i++)
        {
            string colorName = GameManager.instance.targetList[i];
            GameObject go = Instantiate(UI_ballImagePrefabs[SetBallColorToIndex(colorName)], UI_TargetPlace.transform);
            go.transform.localScale = Vector3.zero;
            colorImages.Add(go, colorName);
        }

    }
    public void Set_Check(string[] colors)
    {
        List<GameObject> checkTemp = new List<GameObject>(check_Object);
        check_Object.Clear();
        foreach (string color in colors)
        {
            foreach (KeyValuePair<GameObject, string> image in colorImages)
            {
                if (image.Value == color)
                {
                    if(image.Key.transform.childCount == 1)
                    {
                        GameObject child = image.Key.transform.GetChild(0).gameObject;
                        if (check_Object.Contains(child)) continue;
                        else
                        {
                            checkTemp.Remove(child);
                            check_Object.Add(child);
                            break;
                        }
                    }
                    else
                    {
                        GameObject go = Instantiate(UI_CheckImagePrefab, image.Key.transform);
                        go.transform.localPosition = new Vector3(0, 0, 0);
                        checkTemp.Remove(go);
                        check_Object.Add(go);
                        break;
                    }
                    
                }

            }
        }
        
        foreach (GameObject checkObj in checkTemp)
        {
            Destroy(checkObj);
        }

    }

    void SetImgCount()
    {
        imgCount = GameManager.instance.targetList.Count;
        //Debug.Log($"타겟리스트의 길이는{imgCount}입니다");
    }

    int SetBallColorToIndex(string _colorName)
    {
        switch (_colorName)
        {
            case "Red":
                return 0;

            case "Orange":
                return 1;

            case "Yellow":
                return 2;

            case "Green":
                return 3;

            case "Blue":
                return 4;

            case "Purple":
                return 5;

            case "Black":
                return 6;

        }
        return -1;
    }


    void CheckSuccessBall()
    {
        for (int i = 0; i < GameManager.instance.targetList.Count; i++)
        {
            if (GameManager.instance.curTargetDic[GameManager.instance.targetList[i]] <= 0)
            {
                BallChecked[i] = true;
            }


        }


    }

    public void SetBallUi_BG(string[] array)
    {
        Instantiate(leftCircle, targetBG);
        for (int i = 0; i < array.Length; i++)
        {
            Instantiate(image, targetBG);
        }
        Instantiate(rightCircle, targetBG);
    }

    public void DoBallUIAnim()
    {
        StartCoroutine(nameof(DoUIBallAnim_Cor));
    }

    IEnumerator DoUIBallAnim_Cor()
    {
        Vector3 _pos = GameObject.Find("BG_Pos").transform.position;
        Debug.Log("UIAnim");
        targetBG.DOMove(_pos, 0.3f);

        yield return new WaitForSeconds(0.3f);

        foreach (var image in colorImages)
        {
            Transform imgTF = image.Key.transform;
            imgTF.DOScale(Vector3.one, 0.1f).SetEase(Ease.OutBack, 1.1f);
            yield return new WaitForSeconds(0.15f);
        }
    }
    [ContextMenu("Exit")]
    public void DoBallUIAnim_Exit()
    {
        StartCoroutine(nameof(DoUIBallAnim_Exit_Cor));
    }
    IEnumerator DoUIBallAnim_Exit_Cor()
    {
        foreach (var image in colorImages)
        {
            Transform imgTF = image.Key.transform;
            imgTF.DOLocalMoveY(170, 0.6f).SetEase(Ease.InBack, 1.2f);
            yield return new WaitForSeconds(0.03f);
        }

        targetBG.DOLocalMoveY(170, 0.6f).SetEase(Ease.InBack, 1.2f);
        yield return new WaitForSeconds(0.03f);
    }

    void SetBallUI()
    {

        for (int i = 0; i < imgCount; i++)
        {
            string colorName = GameManager.instance.targetList[i];

            GameObject go = Instantiate(UI_ballImagePrefabs[SetBallColorToIndex(colorName)], UI_TargetPlace.transform);
            go.transform.localPosition = GetPositon(i);



            //프리펩 관리
            InstantiatedObj.Add(go);
            UI_BallImg.Add(go);
        }
    }

    void SetCheckUI()
    {
        //BallChecked.Reverse();
        for (int i = 0; i < BallChecked.Count; i++)
        {
            if (BallChecked[i] == true)
            {
                GameObject go = Instantiate(UI_CheckImagePrefab, UI_TargetPlace.transform);
                go.transform.localPosition = UI_BallImg[i].transform.localPosition;
                InstantiatedObj.Add(go);
            }
        }
    }


    public void EnableScoreBoard(int score)
    {
        Debug.Log("스코어보드 실행");
        isOnScoreBoard = true;

        StartCoroutine(EnableStar(score));
        targetText.text = $"Goal Shot Count:{GameManager.instance.shotRule}";
        shotText.text = $"Shot Count:{GameManager.instance.shotCount}";
    }
    IEnumerator EnableStar(int score)
    {
        UI_ScoreBoard.SetActive(true);
        yield return new WaitForSeconds(1.7f);
        for (int i = 0; i < score; i++)
        {
            UI_Stars[i].SetActive(true);
            UI_Stars[i].transform.parent.GetComponent<RectTransform>().SetAsLastSibling();
            //SoundManager.instance.UI_StarSFX.pitch += 0.35f;
            SoundManager.instance.PlayTargetSound(SoundManager.instance.BallHitSounds[GameManager.instance.HitSoundIndex]);
            GameManager.instance.HitSoundIndex += 2;
            print("?");
            yield return new WaitForSeconds(0.35f);

        }
        //if (score == 3)
        //{
        //    SoundManager.instance.PlayTargetSound(SoundManager.instance.BallHitSounds[7]);
        //    SoundManager.instance.PlayTargetSound(SoundManager.instance.BallHitSounds[9]);
        //    SoundManager.instance.PlayTargetSound(SoundManager.instance.BallHitSounds[11]);
        //}
    }
    public void InitializeStage()
    {
        GameManager.instance.shotCount = 0;
        GameManager.instance.targetList.Clear();
        //GameObject go = GameObject.Find($"Stage_{GameManager.stageLV}(Clone)");
        //Destroy(go);
        isOnScoreBoard = false;
        UI_ScoreBoard.SetActive(false);

    }


    public void OpenMenu()
    {
        isMenuOpen = !isMenuOpen;
        SoundManager.instance.PlayTargetSound(SoundManager.instance.ButtonClickSFX);

        if (isMenuOpen)
        {
            Menu.Change_State(DropDown.State.MoveIn);
        }
        else
        {
            Menu.Change_State(DropDown.State.MoveOut);
        }
    }
    public void OpenTutorial()
    {
        SoundManager.instance.PlayTargetSound(SoundManager.instance.ButtonClickSFX);

        TutorialManager.instance.OpenTutorial();
    }
    public void RestartStage()
    {
        if (!isClick)
        {
            StartCoroutine(DelayStage("PlayScene"));
            //StartCoroutine(DelayStage("LoadingScene"));
            SoundManager.instance.PlayTargetSound(SoundManager.instance.ButtonClickSFX);
            isClick = true;
        }

    }

    public void LockCamRot()
    {
        SoundManager.instance.PlayTargetSound(SoundManager.instance.ButtonClickSFX);

        if (Camera.GetComponent<CameraMove>().isLocked == true)
        {
            //Camera.GetComponent<CameraMove>().isLocked = false;
            SoundManager.instance.isCamLocked = false;
            UI_CamLockImgs[0].SetActive(false);
            UI_CamLockImgs[1].SetActive(true);
        }
        else
        {
            //Camera.GetComponent<CameraMove>().isLocked = true;
            SoundManager.instance.isCamLocked = true;
            UI_CamLockImgs[0].SetActive(true);
            UI_CamLockImgs[1].SetActive(false);
        }
    }


    public void NextStage()
    {
        if (!isClick)
        {
            GameManager.stageLV++;
            StartCoroutine(DelayStage("PlayScene"));
            //StartCoroutine(DelayStage("LoadingScene"));
            SoundManager.instance.PlayTargetSound(SoundManager.instance.ButtonClickSFX);

            isClick = true;
        }
        

    }

    IEnumerator DelayStage(string SceneName)
    {
        Time.timeScale = 1;
        if (GameManager.instance.isClear)
        {
            GameManager.moveScene = SceneName;
            UI_ScoreBoard.GetComponent<Animator>().enabled = true;
            UI_ScoreBoard.GetComponent<Animator>().SetTrigger("DoSlideDown");
            yield return new WaitForSeconds(1);
            DoBallUIAnim_Exit();
            yield return new WaitForSeconds(1);

        }
        if (SceneName == "Lobby")
        {
            GameManager.moveScene = "Lobby";
            Fade_InOut fade = GameObject.Find("Fade").GetComponent<Fade_InOut>();
            fade.ChangeFade(Fade_InOut.Fade.Fade_Out);
            while (!fade.isFade) yield return new WaitForFixedUpdate();
            SceneLoad.sceneName = "Lobby";
            AsyncOperation oper=SceneManager.LoadSceneAsync("LoadingScene");
            SoundManager.instance.DoSoundFade(oper);
            

            GameManager.instance.isStart = false;
            CameraMove.xRotate = 0;
            CameraMove.parentRotation = Quaternion.identity;

            //Destroy(GameManager.static_SoundManager);
            //GameManager.static_SoundManager = null;
            //SoundManager.instance = null;
        }
        else
        {
            GameManager.moveScene = SceneName;
            AsyncOperation oper = SceneManager.LoadSceneAsync(SceneName);
            SoundManager.instance.DoSoundFade(oper);
        }

    }

    public void OpenLobby()
    {
        if (!isClick)
        {
            CameraMove.parentRotation = Quaternion.Euler(Vector3.zero);
            CameraMove.xRotate = 0;
            StartCoroutine(DelayStage("Lobby"));
            SoundManager.instance.PlayTargetSound(SoundManager.instance.ButtonClickSFX);
            SoundManager.instance.isEnable_BallHitSound = false;
            isClick = true;
        }
        
    }

    Vector3 GetPositon(int i)
    {
        if (imgCount % 2 == 0)
        {

            //짝수일때
            //Debug.Log(new Vector3((-(imgCount / 2 - 0.5f) + i) * h_interval, v_interval, 0));
            return new Vector3((-(imgCount / 2 - 0.5f) + i) * h_interval, v_interval, 0);

        }
        else
        {
            //홀수일때
            //Debug.Log(new Vector3((-(imgCount / 2) + i) * h_interval, v_interval, 0));

            return new Vector3((-(imgCount / 2) + i) * h_interval, v_interval, 0);
        }


    }

}

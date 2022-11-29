using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class StageBtn : MonoBehaviour
{
    public static bool isPlay;

    Button btn;
    Text text;
    string mapName;

    GameObject stars_Prefab, stars, blackCircle;

    public Sprite[] starSprite;
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick_Btn);
        text = transform.GetChild(1).GetComponent<Text>();

        mapName = gameObject.name.Substring(6);

        text.fontSize = 70;
        text.text = mapName;

       
        if (!GetComponent<Button>().interactable)
        {
            blackCircle = (GameObject)Resources.Load("Black_Circle");
            Instantiate(blackCircle, transform);
            return;
        }
        stars_Prefab = (GameObject)Resources.Load("Stars");
        
        stars = Instantiate(stars_Prefab, transform);
        if(PlayerPrefs.HasKey(mapName + "_STAR"))
        {
            int star_Num = PlayerPrefs.GetInt(mapName + "_STAR");
            stars.transform.GetChild(0).GetComponent<Image>().sprite = starSprite[star_Num];
        }
    }


    void OnClick_Btn()
    {
        if (isPlay) return;
        SoundManager.instance.PlayTargetSound(SoundManager.instance.ButtonClickSFX);

        isPlay = true;
        print(mapName + " stage open");
        GameManager.stageLV = int.Parse(mapName) - 1;
        StartCoroutine(LoadGameScene());
    }

    IEnumerator LoadGameScene()
    {
        Fade_InOut fade = GameObject.Find("Fade").GetComponent<Fade_InOut>();
        fade.ChangeFade(Fade_InOut.Fade.Fade_Out);
        while (!fade.isFade) yield return new WaitForFixedUpdate();

        SceneLoad.sceneName = "PlayScene";
        Camera.main.gameObject.SetActive(false);
        LobbyManager.instance.Obj_SL.gameObject.SetActive(true);
        LobbyManager.instance.UI.transform.localScale = Vector3.zero;
        
    }




    IEnumerator DelayTime()
    {
        Fade_InOut fade = GameObject.Find("Fade").GetComponent<Fade_InOut>();
        //while (!fade.isFade) yield return new WaitForFixedUpdate();
        yield return null;
       
        SceneManager.LoadScene("PlayScene");
    }
}

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

    GameObject stars_Prefab, stars;
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick_Btn);
        text = transform.GetChild(0).GetComponent<Text>();

        mapName = gameObject.name.Substring(6);

        text.fontSize = 70;
        text.text = mapName;

       
        if (!GetComponent<Button>().interactable) return; 
        stars_Prefab = (GameObject)Resources.Load("Stars");
        stars = Instantiate(stars_Prefab, transform);
        if(PlayerPrefs.HasKey(mapName + "_STAR"))
        {
            int star_Num = PlayerPrefs.GetInt(mapName + "_STAR");
            for(int i = 0; i < star_Num; i++)
            {
                stars.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
            }
        }
    }


    void OnClick_Btn()
    {
        if (isPlay) return;
        isPlay = true;
        print(mapName + " stage open");
        GameManager.stageLV = int.Parse(mapName) - 1;
        //Fade_InOut fade = GameObject.Find("Fade").GetComponent<Fade_InOut>();
        //fade.ChangeFade(Fade_InOut.Fade.Fade_Out);
        //StartCoroutine(DelayTime());
        StartCoroutine(LoadGameScene());
    }

    IEnumerator LoadGameScene()
    {
        Fade_InOut fade = GameObject.Find("Fade").GetComponent<Fade_InOut>();
        fade.ChangeFade(Fade_InOut.Fade.Fade_Out);
        while (!fade.isFade) yield return new WaitForFixedUpdate();
        //Debug.Log("±× ¹¹³Ä");

        SceneLoad.sceneName = "PlayScene";
        Camera.main.gameObject.SetActive(false);
        LobbyManager.instance.Obj_SL.gameObject.SetActive(true);
        LobbyManager.instance.UI.transform.localScale = Vector3.zero;
        //fade.ChangeFade(Fade_InOut.Fade.Fade_In);
        //while (!fade.isFade) yield return new WaitForFixedUpdate();
        //Debug.Log("±× ¹¹³Ä2");
        
    }




    IEnumerator DelayTime()
    {
        Fade_InOut fade = GameObject.Find("Fade").GetComponent<Fade_InOut>();
        //while (!fade.isFade) yield return new WaitForFixedUpdate();
        yield return null;
       
        SceneManager.LoadScene("PlayScene");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageBtn : MonoBehaviour
{
    Button btn;
    Text text;
    string mapName;
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick_Btn);
        text = transform.GetChild(0).GetComponent<Text>();

        mapName = gameObject.name.Substring(6);

        text.fontSize = 55;
        text.text = mapName + " Stage";
    }

    
    void Update()
    {
        
    }

    void OnClick_Btn()
    {
        print(mapName + " stage open");
        GameManager.stageLV = int.Parse(mapName) - 1;
        Fade_InOut fade = GameObject.Find("Fade").GetComponent<Fade_InOut>();
        fade.ChangeFade(Fade_InOut.Fade.Fade_Out);
        StartCoroutine(DelayTime());
        
    }

    IEnumerator DelayTime()
    {
        Fade_InOut fade = GameObject.Find("Fade").GetComponent<Fade_InOut>();
        while (!fade.isFade) yield return new WaitForFixedUpdate();
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("PlayScene");
    }
}

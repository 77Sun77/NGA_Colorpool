using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    public Slider loadingSlider;
    public Text guide, startText;
    public GameObject image;

    public SoundManager SoundManager;
    bool isOpen;
    void Start()
    {
        DontDestroyOnLoad(SoundManager);

        isOpen = false;
        loadingSlider.value = 0;
        StartCoroutine(LoadScene_Cor());
        StartCoroutine(DelayImage());
    }

    IEnumerator Fade()
    {

        Fade_InOut fade = GameObject.Find("Fade").GetComponent<Fade_InOut>();
        
        fade.ChangeFade(Fade_InOut.Fade.Fade_Out);
        while (!fade.isFade) yield return new WaitForFixedUpdate();

        isOpen = true;
        
    }
    IEnumerator LoadScene_Cor()
    {
        AsyncOperation oper = SceneManager.LoadSceneAsync("Lobby");
        oper.allowSceneActivation = false;

        while (!oper.isDone)
        {
            yield return null;//제어권들 돌려줘서 화면 갱신
            loadingSlider.value = oper.progress;
            if(oper.progress < 0.3f)
            {
                guide.text = "맵을 불러오는 중";
            }
            else if(oper.progress < 0.6f)
            {
                guide.text = "공을 생성하는 중";
            }
            else if (oper.progress < 0.9f)
            {
                guide.text = "색깔을 입히는 중";
            }
            else
            {
                loadingSlider.gameObject.SetActive(false);
                guide.gameObject.SetActive(false);
                startText.gameObject.SetActive(true);

                while (true)
                {
                    yield return new WaitForFixedUpdate();
                    if (Input.GetMouseButtonDown(0))
                    {
                        StartCoroutine(Fade());
                        break;
                    }
                }
                
                while (!isOpen) yield return new WaitForFixedUpdate();
                oper.allowSceneActivation = true;
                break;
            }
        }
    }

    IEnumerator DelayImage()
    {
        yield return new WaitForSeconds(0.5f);
        image.SetActive(true);
    }
}

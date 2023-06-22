using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class LogoAnim : MonoBehaviour
{
    public Transform Objects;
    public GameObject[] balls;

    public Material purple;
    int SoundSFXIndex;
    bool isTriggered;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTriggered)
            {
                SceneManager.LoadScene("LogoAnimation");
            }
            else
            {
                isTriggered = true;
                MoveUpObjs();
            }
        }
    }

    public void MoveUpObjs()
    {
        balls[0].transform.gameObject.SetActive(true);
        balls[1].transform.gameObject.SetActive(true);

        balls[0].transform.localScale = Vector3.zero;
        balls[1].transform.localScale = Vector3.zero;
        StartCoroutine(nameof(MoveUpObjs_Cor));
    }

    IEnumerator MoveUpObjs_Cor()
    {
        for (int i = 0; i < Objects.childCount; i++)
        {
            Objects.GetChild(i).DOLocalMoveZ(-3f,2);
            SoundManager.instance.PlayTargetSound(SoundManager.instance.BallHitSounds[SoundSFXIndex++]);

            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.3f);


        balls[0].transform.DOScale(Vector3.one*12.4f, 0.5f);
        yield return new WaitForSeconds(0.1f);
        balls[1].transform.DOScale(Vector3.one* 12.4f, 0.5f);
        yield return new WaitForSeconds(0.5f);

        balls[0].transform.DOMove(balls[0].transform.position+Vector3.forward/4, 0.2f);
        balls[1].transform.DOMove(balls[1].transform.position - Vector3.forward/4, 0.2f);

        yield return new WaitForSeconds(0.2f);

        balls[0].GetComponent<Renderer>().material = purple;
        balls[1].GetComponent<Renderer>().material = purple;

        balls[1].transform.DOMove(balls[1].transform.position + Vector3.forward / 4, 0.3f);
        balls[0].transform.DOMove(balls[0].transform.position - Vector3.forward / 4, 0.3f);

        SoundManager.instance.PlayTargetSound(SoundManager.instance.BallHitSounds[SoundSFXIndex]);

    }

}

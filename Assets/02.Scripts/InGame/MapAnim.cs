using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAnim : MonoBehaviour
{
    public Animator Wall_T;
    public Animator Wall_B;

    public MapAnimation MA;

    public GameObject[] Balls;

    private void Start()
    {
        Wall_T.SetTrigger("doAnim");
        Wall_B.SetTrigger("doAnim");

        StartCoroutine(nameof(SpawnBall));
    }

    IEnumerator SpawnBall()
    {
        yield return new WaitForSeconds(0.7f);

        MA.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.5f);
        foreach (var b in Balls)
        {
            b.SetActive(true);
            yield return new WaitForSeconds(0.15f);
        }

    }

}

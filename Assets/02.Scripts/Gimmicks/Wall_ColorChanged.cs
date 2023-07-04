using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_ColorChanged : MonoBehaviour
{
    public Ball.Ball_Color color;

    public GameObject StartPos;
    public GameObject EndPos;

    public GameObject[] Colors;
    public Transform Square_Offset;

    private void Awake()
    {
        Colors[(int)color].gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BALL"))
        {
            Ball ball = other.GetComponent<Ball>();
            ball.ChangeColor(color, ball.color_Name, "Wall");

            SoundManager.instance.PlayTargetSound(SoundManager.instance.PaintSound);
        }
    }
}

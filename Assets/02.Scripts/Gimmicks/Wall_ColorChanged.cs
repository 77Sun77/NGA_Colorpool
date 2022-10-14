using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_ColorChanged : MonoBehaviour
{
    public Ball.Ball_Color color;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BALL"))
        {
            Ball ball = other.GetComponent<Ball>();
            ball.ChangeColor(color, ball.color_Name, "Wall");
        }
    }
}

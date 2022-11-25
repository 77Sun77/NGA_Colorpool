using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameQuit : MonoBehaviour
{
    public GameObject gameQuit;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameQuit.SetActive(true);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}

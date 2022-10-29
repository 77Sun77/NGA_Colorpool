using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ClickMove : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        print("Down : " + eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.position.x <= Display.main.systemWidth / 2)
        {
            LobbyManager.instance.OnClick_Left();
        }
        else
        {
            LobbyManager.instance.OnClick_Right();
        }
        print(eventData.position);
    }
}

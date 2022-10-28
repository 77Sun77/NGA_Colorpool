using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ClickMove : MonoBehaviour, IPointerDownHandler
{
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (eventData.position.x <= Display.main.systemWidth/2)
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

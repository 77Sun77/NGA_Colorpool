using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ClickMove2 : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
   public MapAnim mapAnim;

    [SerializeField]
    public float a => Time.timeScale;
    void Start()
    {
        //mapAnim=FindObjectOfType<MapAnim>();
    }

    

    public void OnPointerDown(PointerEventData eventData)
    {
        //print("Down : " + eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Input");
        if (mapAnim.isAnim)
        {
            Time.timeScale = 4.2f;
        }

    }
}

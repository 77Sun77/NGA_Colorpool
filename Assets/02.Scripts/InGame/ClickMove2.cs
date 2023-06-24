using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ClickMove2 : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public MapAnim mapAnim;
    public bool isClicking;

    [SerializeField]
    public float a => Time.timeScale;
    void Start()
    {
        //mapAnim=FindObjectOfType<MapAnim>();
    }



    public void OnPointerDown(PointerEventData eventData)
    {
        //print("Down : " + eventData.position);
        isClicking = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isClicking = false;
        Debug.Log("Input");
        if (mapAnim.isAnim)
        {
            Time.timeScale = 4.2f;
        }
        
    }
}

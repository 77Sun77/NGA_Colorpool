using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropDown : MonoBehaviour
{
    public enum State{ None, MoveIn, MoveOut };
    public State WindowState;

    public Transform StartPoint, EndPoint;

    RectTransform tr;

    void Start()
    {
        tr = GetComponent<RectTransform>();
    }

    void Update()
    {
        if(WindowState == State.MoveIn)
        {
            Vector3 vec = tr.position;
            tr.position = Vector3.Lerp(vec, EndPoint.position, 15 * Time.deltaTime);
            if(Vector3.Distance(transform.position, EndPoint.position) <= 0.1f)
            {
                WindowState = State.None;
                tr.position = EndPoint.position;
            }
        }
        else if(WindowState == State.MoveOut)
        {
            Vector3 vec = tr.position;
            tr.position = Vector3.Lerp(vec, StartPoint.position, 15 * Time.deltaTime);
            if (Vector3.Distance(transform.position, StartPoint.position) <= 0.1f)
            {
                WindowState = State.None;
                tr.position = StartPoint.position;
            }
        }
    }

    public void Change_State(State state)
    {
        WindowState = state;
    }
}

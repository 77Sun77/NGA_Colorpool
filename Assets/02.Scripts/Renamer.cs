using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Renamer : MonoBehaviour
{
    [ContextMenu("ReName")]
    public void ReName()
    {
        int index = 0;

        foreach (Transform t in transform)
        {
            if (t.TryGetComponent(out StageOption so))
            {
                index++;
                t.transform.name = $"Stage_{index}";
            }    
        }
    }
}

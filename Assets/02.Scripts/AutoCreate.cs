using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCreate : MonoBehaviour
{
    List<Transform> children = new List<Transform>();
    void Start()
    {
        Transform parent = transform.Find("Walls");
        foreach(Transform child in parent)
        {
            children.Add(child);
        }
        foreach(Transform child in children)
        {
            if (child.gameObject.CompareTag("WALL"))
            {
                child.rotation = Quaternion.Euler(new Vector3(child.rotation.x, child.rotation.y, 90));
                child.localScale = new Vector3(1, child.localScale.x, child.localScale.z);
            }
        }
        Destroy(this);
    }

    void Update()
    {
        
    }
}

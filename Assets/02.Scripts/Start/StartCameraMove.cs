using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCameraMove : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.up * 60 * Time.deltaTime);
    }
}

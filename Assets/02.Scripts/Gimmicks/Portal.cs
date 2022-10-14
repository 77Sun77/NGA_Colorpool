using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal[] portals = new Portal[2];
    public Transform teleportPoint;

    public bool isTrigger;

    void Start()
    {
        portals[0] = this;
        portals[1] = teleportPoint.GetComponent<Portal>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BALL"))
        {
            foreach(Portal portal in portals)
            {
                if (portal.isTrigger && portal != this) return;
            }
            Teleport(other.transform);
            isTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (Portal portal in portals)
        {
            if (portal.isTrigger && portal == this) return;
            else portal.isTrigger = false;
        }
        isTrigger = false;
    }

    void Teleport(Transform ball)
    {
        ball.position = teleportPoint.position;
    }
}

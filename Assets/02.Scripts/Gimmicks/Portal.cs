using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal[] portals = new Portal[2];
    public Transform teleportPoint;

    public bool isTrigger;
    public List<Transform> balls = new List<Transform>();

    [SerializeField] Color[] colors;
    ParticleSystem particle;
    void Start()
    {
        portals[0] = this;
        portals[1] = teleportPoint.GetComponent<Portal>();

        particle = transform.Find("VortexGlow").GetComponent<ParticleSystem>();
        if (transform.GetChild(0).gameObject.activeInHierarchy) particle.startColor = colors[0];
        else if (transform.GetChild(1).gameObject.activeInHierarchy) particle.startColor = colors[1];
        else if (transform.GetChild(2).gameObject.activeInHierarchy) particle.startColor = colors[2];
        else if (transform.GetChild(3).gameObject.activeInHierarchy) particle.startColor = colors[3];
        else if (transform.GetChild(4).gameObject.activeInHierarchy) particle.startColor = colors[4];
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BALL"))
        {
            SoundManager.instance.PlayTargetSound(SoundManager.instance.PortalSFX);

            foreach (Portal portal in portals)
            {
                if (portal.balls.Contains(other.transform) && portal != this) return;
            }
            Teleport(other.transform);
            balls.Add(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (Portal portal in portals)
        {
            if (portal.balls.Contains(other.transform) && portal == this) return;
            else portal.balls.Remove(other.transform);
        }
        balls.Remove(other.transform);
    }

    void Teleport(Transform ball)
    {
        ball.position = teleportPoint.position;
    }
}

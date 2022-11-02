using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallReflect : MonoBehaviour
{
    private void Start()
    {
        gameObject.tag = "WALL";
    }
    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.CompareTag("BALL"))
        {
            Rigidbody ballRigid = coll.gameObject.GetComponent<Rigidbody>();
            Vector3 velocity = coll.gameObject.GetComponent<Ball>().velocity;
            ballRigid.velocity = Vector3.Reflect(velocity, -coll.GetContact(0).normal);
        }
    }
}

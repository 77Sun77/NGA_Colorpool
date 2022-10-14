using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_ColorReflect : MonoBehaviour
{
    public  Ball.Ball_Color ball_Color;
    [SerializeField]
    Material[] materials;
    private void Start()
    {
        gameObject.layer = 20 + (int)ball_Color;
        Material material = materials[((int)ball_Color)];
        GetComponent<Renderer>().material = material;


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

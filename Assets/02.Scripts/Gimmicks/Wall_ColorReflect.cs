using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_ColorReflect : MonoBehaviour
{
    public  Ball.Ball_Color ball_Color;

    public enum WallKind { Other, ColorReflect };
    public WallKind kind;

    [SerializeField]
    Material[] materials;
    private void Start()
    {
        gameObject.layer = 20 + (int)ball_Color;
        Material material = materials[((int)ball_Color)];
        GetComponent<Renderer>().material = material;
        gameObject.tag = "WALL";

    }



    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.CompareTag("BALL"))
        {
            if (kind == WallKind.ColorReflect && gameObject.layer - coll.gameObject.layer == 9)
            {
                Physics.IgnoreLayerCollision(gameObject.layer, coll.gameObject.layer, true);
                return;
            }
            Rigidbody ballRigid = coll.gameObject.GetComponent<Rigidbody>();
            Vector3 velocity = coll.gameObject.GetComponent<Ball>().velocity;
            ballRigid.velocity = Vector3.Reflect(velocity, -coll.GetContact(0).normal);
        }


    }
    
}

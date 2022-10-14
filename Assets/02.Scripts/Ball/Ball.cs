using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public void Shot(Vector3 vec)
    {
        myRIgid.velocity = vec;
        isShot = true;
        foreach(LineRenderer line in lines) line.positionCount = 1;

        GameManager.instance.shotCount++;
    }

    public enum ObjectKind { Ball, Wall, Line};
    public ObjectKind kind;
    public enum BallKind { Small, Big};
    public BallKind ballKind;

    // Components
    Material myMaterial;
    Rigidbody myRIgid;
    public Animator anim;
    List<LineRenderer> lines = new List<LineRenderer>();

    // Color
    public enum Ball_Color { Red, Orange, Yellow, Green, Blue, Purple, Black };
    public Ball_Color color_Name;
    [SerializeField]
    Color[] colors;


    
    public Vector3 velocity;
    public bool isShot;

    int num_ballIndex = 0;

    public GameObject hitParticle;
    

    void Start()
    {

        myRIgid = GetComponent<Rigidbody>();

        if (kind == ObjectKind.Ball)
        {
            anim = GetComponent<Animator>();
            foreach (Transform children in transform.GetChild(0))
            {
                lines.Add(children.GetComponent<LineRenderer>());
            }
        }

        Set_Ball();
        foreach (LineRenderer line in lines) line.SetColors(myMaterial.color, myMaterial.color);
    }

    void Update()
    {
        velocity = myRIgid.velocity;

        if(myRIgid.velocity.magnitude <= 0.01f && isShot)
        {
            isShot = false;
        }

     


        if(kind != ObjectKind.Wall)gameObject.layer = 10 + num_ballIndex;
    }

    public void Set_Ball(string call = "Ball", Ball_Color line_Color = Ball_Color.Red)
    {
        myMaterial = GetComponent<Renderer>().material;

        Ball_Color color;
        if (call == "Line") color = line_Color;
        else color = color_Name;

        if (color == Ball_Color.Red) num_ballIndex = 0;
        if (color == Ball_Color.Orange) num_ballIndex = 1;
        if (color == Ball_Color.Yellow) num_ballIndex = 2;
        if (color == Ball_Color.Green) num_ballIndex = 3;
        if (color == Ball_Color.Blue) num_ballIndex = 4;
        if (color == Ball_Color.Purple) num_ballIndex = 5;
        if (color == Ball_Color.Black) num_ballIndex = 6;

        //myMaterial = GetComponent<Renderer>().sharedMaterial;
        //var tempMat = new Material(myMaterial);
        //tempMat.color=colors[num_ballIndex];
        //myMaterial = tempMat;

       // Debug.Log("SetBall");

        if (call == "Line")
        {
            Color mat = colors[num_ballIndex];
            mat.a = 0.7f;
            foreach (LineRenderer line in lines) line.SetColors(mat, mat);

            return;
        }


        myMaterial.color = colors[num_ballIndex];// 이미지 만들어지면 색깔이 아니라 이미지 변경




    }

    public bool Set_Line(Vector3 vec, int count = 2)
    {
        float distance = Vector3.Distance(transform.position, vec);
        float newDistance = distance; // 반사될때 길이
        RaycastHit wallHit;
        Vector3 curPos = vec;
        Vector3 newPos = Vector3.zero;
        int layerMask = (-1) - (1 << LayerMask.NameToLayer("ColorChangeWall"));
        if (Physics.Raycast(transform.position, (curPos - transform.position).normalized, out wallHit, newDistance, layerMask))
        {
            if (wallHit.collider.CompareTag("WALL") || wallHit.collider.gameObject.layer == 3)
            {

                curPos = wallHit.point;
                newDistance = distance - Vector3.Distance(transform.position, curPos);
                Vector3 reflect = Vector3.Reflect((vec - curPos).normalized, wallHit.normal);
                newPos = reflect * newDistance + curPos;
                count = 3;

                bool firstBool = wallHit.collider.gameObject.layer == 3;
                if (firstBool)
                {
                    ChangeColor(color_Name, wallHit.collider.GetComponent<Ball>().color_Name, "Line");
                }
                else ResetColor();

                if (Physics.Raycast(curPos, (newPos - curPos).normalized, out wallHit, newDistance, layerMask))
                {
                    if (wallHit.collider != null)
                    {
                        newPos = wallHit.point;
                        if (wallHit.collider.CompareTag("BALL"))
                        {
                            ChangeColor(color_Name, wallHit.collider.GetComponent<Ball>().color_Name, "Line");
                        }
                    }

                }
            }
            else if (wallHit.collider.CompareTag("BALL"))
            {
                if (wallHit.transform != transform)
                {
                    curPos = wallHit.point;
                    ChangeColor(color_Name, wallHit.collider.GetComponent<Ball>().color_Name, "Line");
                }
                else
                {
                    ResetColor();
                }
            }
            else ResetColor();
        }
        else ResetColor();

        foreach (LineRenderer line in lines)
        {
            line.positionCount = 2;
        }
        curPos = new Vector3(curPos.x, 0.5f, curPos.z);
        newPos = new Vector3(newPos.x, 0.5f, newPos.z);
        if (count == 1 || distance < 1f)
        {
            foreach (LineRenderer line in lines)
            {
                line.positionCount = 0;
            }
            return false;
        }
        if(count == 2)
        {
            lines[0].SetPosition(0, transform.position);
            lines[0].SetPosition(1, curPos);
            lines[1].positionCount = 0;
        }
        if (count == 3)
        {
            lines[0].SetPosition(0, transform.position);
            lines[0].SetPosition(1, curPos);
            lines[1].SetPosition(0, curPos);
            lines[1].SetPosition(1, newPos);
        }

        return true;

    }


    void ResetColor()
    {
        Color mat = myMaterial.color;
        mat.a = 0.7f;
        //foreach (LineRenderer line in lines) line.SetColors(myMaterial.color, myMaterial.color);
        foreach (LineRenderer line in lines) line.SetColors(mat, mat);
    }



    // Ball Collision
    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.CompareTag("BALL"))
        {
            if(kind == ObjectKind.Ball)
            {
                Ball hit_Ball = coll.gameObject.GetComponent<Ball>();
                GameObject particle = null;
                if (hit_Ball.kind == ObjectKind.Ball && color_Name != hit_Ball.color_Name)
                {
                    particle = Instantiate(hitParticle, coll.transform.position, Quaternion.identity);
                    particle.transform.localScale = coll.transform.localScale;
                    particle.transform.rotation = Quaternion.Euler(new Vector3(60, -30, 0));
                    Destroy(particle, 1.5f);
                }

                ChangeColor(color_Name, hit_Ball.color_Name);
                if(particle != null)
                {
                    particle.GetComponent<SpriteRenderer>().color = myMaterial.color;
                }

                hit_Ball.isShot = true;
            }
            else
            {
                Rigidbody ballRigid = coll.gameObject.GetComponent<Rigidbody>();
                Vector3 velocity = coll.gameObject.GetComponent<Ball>().velocity;
                ballRigid.velocity = Vector3.Reflect(velocity, -coll.GetContact(0).normal);
            }
        }
    }

    public void ChangeColor(Ball_Color c1, Ball_Color c2, string call = "Ball")
    {
        if (c1 == c2) return;

        if(call == "Ball")
        {
            if (Orange(c1, c2))
            {
                color_Name = Ball_Color.Orange;

            }
            else if (Green(c1, c2))
            {
                color_Name = Ball_Color.Green;

            }
            else if (Purple(c1, c2))
            {
                color_Name = Ball_Color.Purple;
            }
            else
            {
                color_Name = Ball_Color.Black;
            }
            Set_Ball();
        }
        else if (call == "Line")
        {
            if (c1 == c2) Set_Ball("Line", color_Name);
            Ball_Color color;
            if (Orange(c1, c2))
            {
                color = Ball_Color.Orange;
            }
            else if (Green(c1, c2))
            {
                color = Ball_Color.Green;

            }
            else if (Purple(c1, c2))
            {
                color = Ball_Color.Purple;
            }
            else
            {
                color = Ball_Color.Black;
            }
            Set_Ball("Line", color);
        }
        else
        {
            color_Name = c1;
            Set_Ball();
        }
        
    }

    bool Orange(Ball_Color c1, Ball_Color c2)
    {

        bool o1 = (c1, c2) == (Ball_Color.Red, Ball_Color.Yellow);
        bool o2 = (c1, c2) == (Ball_Color.Yellow, Ball_Color.Red);
        bool o3 = (c1, c2) == (Ball_Color.Red, Ball_Color.Orange);
        bool o4 = (c1, c2) == (Ball_Color.Orange, Ball_Color.Red);
        bool o5 = (c1, c2) == (Ball_Color.Orange, Ball_Color.Orange);
        bool o6 = (c1, c2) == (Ball_Color.Orange, Ball_Color.Yellow);
        bool o7 = (c1, c2) == (Ball_Color.Yellow, Ball_Color.Orange);
        if (o1 || o2 || o3 || o4 || o5 || o6 || o7)
        {
            return true;
        }
        return false;
    }
    bool Green(Ball_Color c1, Ball_Color c2)
    {

        bool g1 = (c1, c2) == (Ball_Color.Green, Ball_Color.Yellow);
        bool g2 = (c1, c2) == (Ball_Color.Yellow, Ball_Color.Green);
        bool g3 = (c1, c2) == (Ball_Color.Blue, Ball_Color.Yellow);
        bool g4 = (c1, c2) == (Ball_Color.Yellow, Ball_Color.Blue);
        bool g5 = (c1, c2) == (Ball_Color.Blue, Ball_Color.Green);
        bool g6 = (c1, c2) == (Ball_Color.Green, Ball_Color.Blue);
        if (g1 || g2 || g3 || g4 || g5 || g6)
        {
            return true;
        }
        return false;
    }
    bool Purple(Ball_Color c1, Ball_Color c2)
    {

        bool p1 = (c1, c2) == (Ball_Color.Red, Ball_Color.Purple);
        bool p2 = (c1, c2) == (Ball_Color.Purple, Ball_Color.Red);
        bool p3 = (c1, c2) == (Ball_Color.Blue, Ball_Color.Purple);
        bool p4 = (c1, c2) == (Ball_Color.Purple, Ball_Color.Blue);
        bool p5 = (c1, c2) == (Ball_Color.Blue, Ball_Color.Red);
        bool p6 = (c1, c2) == (Ball_Color.Red, Ball_Color.Blue);
        if (p1 || p2 || p3 || p4 || p5 || p6)
        {
            return true;
        }
        return false;
    }

    public void Stop_Anim()
    {
        anim.enabled = false;
        if(ballKind == BallKind.Small) transform.localScale = new Vector3(1, 1, 1);
        else transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

    }

}

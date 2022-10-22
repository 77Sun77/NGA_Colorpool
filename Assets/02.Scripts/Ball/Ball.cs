using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public void Shot(Vector3 vec)
    {
        myRIgid.velocity = vec;
        isShot = true;
        foreach (LineRenderer line in lines) line.positionCount = 1;

        GameManager.instance.isValid = false;
        GameManager.instance.shotCount++;
    }

    public enum ObjectKind { Ball, Wall, Line };
    public ObjectKind kind;
    public enum BallKind { Small, Big };
    public BallKind ballKind;

    // Components
    Material myMaterial;
    Rigidbody myRIgid;
    public Animator anim;
    List<LineRenderer> lines = new List<LineRenderer>();

    // Color
    public enum Ball_Color { Red, Orange, Yellow, Green, Blue, Purple, Black };
    public Ball_Color color_Name;

    public Ball_Color color_Line1;
    public Ball_Color color_Line2;
    [SerializeField]
    Color[] colors;



    public Vector3 velocity;
    public bool isShot;
    public bool isTargetting;

    int num_ballIndex = 0;

    public GameObject hitParticle;

    int changeCount;

    public bool isPassingPaint;
    public bool isPassingPaint2;
    Ball_Color paintColor;


    void Awake()
    {
        changeCount = 0;
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

        if (GetComponent<AnimType_Mono>() == null)
        {
            AnimType_Mono am = gameObject.AddComponent<AnimType_Mono>();
            am.animType = AnimType_Mono.AnimType.Ball;
        }

    }

    void Update()
    {
        velocity = myRIgid.velocity;

        if (myRIgid.velocity.magnitude <= 0.01f && isShot)
        {
            isShot = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && kind == ObjectKind.Ball) anim.SetTrigger("SizeAnim");

        if (isTargetting)
            gameObject.layer = LayerMask.NameToLayer("Ball_CurTargetting");//자기자신한테 예측선이 막히는 거 방지
        else if (kind != ObjectKind.Wall)
        {
            gameObject.layer = 10 + num_ballIndex;
        }
    }

    public void Set_Ball(string call = "Ball", Ball_Color line_Color = Ball_Color.Red)
    {
        myMaterial = GetComponent<Renderer>().material;

        Ball_Color color;
        if (call == "Line"|| call == "Line2") color = line_Color;
        else color = color_Name;

        if (color == Ball_Color.Red) num_ballIndex = 0;
        if (color == Ball_Color.Orange) num_ballIndex = 1;
        if (color == Ball_Color.Yellow) num_ballIndex = 2;
        if (color == Ball_Color.Green) num_ballIndex = 3;
        if (color == Ball_Color.Blue) num_ballIndex = 4;
        if (color == Ball_Color.Purple) num_ballIndex = 5;
        if (color == Ball_Color.Black) num_ballIndex = 6;

        if (call == "Line")
        {
            Color mat = colors[num_ballIndex];
            mat.a = 0.7f;
            //foreach (LineRenderer line in lines) line.SetColors(mat, mat);
            lines[0].SetColors(mat, mat);
            return;
        }
        else if (call == "Line2")
        {
            Color mat = colors[num_ballIndex];
            mat.a = 0.7f;
            lines[1].SetColors(mat, mat);
            return;
        }


        myMaterial.color = colors[num_ballIndex];// 이미지 만들어지면 색깔이 아니라 이미지 변경
        if (call == "Ball" && changeCount > 0)
        {
            GameObject particle = Instantiate(hitParticle, transform.position + new Vector3(-0.5f, 0.5f, 0.25f), Quaternion.Euler(new Vector3(50, -50, 0)));
            particle.transform.localScale = transform.localScale;
            particle.GetComponent<SpriteRenderer>().color = myMaterial.color;
            Destroy(particle, 1.5f);
        }
        changeCount++;
    }

    public bool Set_Line(Vector3 vec, int count = 2)
    {
        float distance = Vector3.Distance(transform.position, vec);
        //float newDistance = distance; // 반사될때 길이
        float newDistance;
        RaycastHit wallHit;
        Vector3 curPos = vec;
        Vector3 newPos = Vector3.zero;

        isPassingPaint = false;//값 초기화
        isPassingPaint2 = false;//값 초기화

        //여기부터 WallColorChange를 위한 레이캐스트
        if (Physics.Raycast(transform.position, (curPos - transform.position).normalized, out wallHit, distance))
        {
            curPos = wallHit.point;
            newDistance = distance - Vector3.Distance(transform.position, curPos);
            Vector3 reflect = Vector3.Reflect((vec - curPos).normalized, wallHit.normal);
            newPos = reflect * newDistance + curPos;
           
            if (wallHit.transform.TryGetComponent(out Wall_ColorChanged WC))
            {
                isPassingPaint = true;
                paintColor = WC.color;
                //Debug.Log("찾음");
            }

            if (wallHit.collider.CompareTag("WALL") || wallHit.collider.gameObject.layer == 3)//벽일때
            {
                if (Physics.Raycast(curPos, (newPos - curPos).normalized, out wallHit, newDistance))//튕기는거 계산
                {
                    //newPos = wallHit.point;
                    if (wallHit.collider.TryGetComponent(out Wall_ColorChanged _WC))
                    {
                        isPassingPaint2 = true;
                        paintColor = _WC.color;
                    }
                }
            }

        }
        //여기까지 WallColorChange를 위한 레이캐스트


        string ReflectWall_LayerName = LayerMask.LayerToName(num_ballIndex + 20);//ColorRefect에 적합한 색깔이라면
        Debug.Log(ReflectWall_LayerName);
        color_Line1 = color_Name;//Line1의 컬러를 저장해둠
        int layerMask = (-1) - (1 << LayerMask.NameToLayer("ColorChangeWall") |1<< LayerMask.NameToLayer(ReflectWall_LayerName)|1<<2);//ColorChangeWall,ColorReflect,Key 레이어 제외

        if (Physics.Raycast(transform.position, (curPos - transform.position).normalized, out wallHit, distance, layerMask))
        {
            if (wallHit.collider.CompareTag("WALL") || wallHit.collider.gameObject.layer == 3)//벽에 부딪혔을때
            {
                curPos = wallHit.point;
                newDistance = distance - Vector3.Distance(transform.position, curPos);
                Vector3 reflect = Vector3.Reflect((vec - curPos).normalized, wallHit.normal);
                newPos = reflect * newDistance + curPos;
                count = 3;

                bool firstBool = wallHit.collider.gameObject.layer == 3;//ColorWall일때 색 바꾸기
                if (firstBool)
                {
                    Debug.Log("ColorWall");
                    ChangeColor(color_Name, wallHit.collider.GetComponent<Ball>().color_Name, "Line");
                    ChangeColor(color_Name, wallHit.collider.GetComponent<Ball>().color_Name, "Line2");
                }
                else
                {
                    Debug.Log("Wall");
                    ChangeColor(color_Name, color_Name, "Line");
                    ChangeColor(color_Line1, color_Line1, "Line2");
                }

                int layerMask2 = (-1) - (1 << LayerMask.NameToLayer("ColorChangeWall") | 1 << LayerMask.NameToLayer(ReflectWall_LayerName) | 1 << 2);//ColorChangeWall,ColorReflect,Key 레이어 제외
                //한번 튕길때 두번째 직선 레이 쏘기
                if (Physics.Raycast(curPos, (newPos - curPos).normalized, out wallHit, newDistance, layerMask2))
                {
                    if (wallHit.collider != null)
                    {
                        newPos = reflect * newDistance+curPos;
                        Debug.DrawRay(curPos, newPos,Color.green);
                        if (wallHit.collider.CompareTag("BALL"))
                        {
                            Debug.Log("Ball By SeconLine");
                            newPos = wallHit.point;
                            ChangeColor(color_Line1, wallHit.collider.GetComponent<Ball>().color_Name, "Line2");
                        }
                        else
                        {
                            Debug.Log("Wall And Wall");
                            newPos = wallHit.point;
                            ChangeColor(color_Line1, color_Line1, "Line2");
                        }
                    }
                }
            }
            else if (wallHit.collider.CompareTag("BALL"))//공에 부딪혔다면
            {
                if (wallHit.transform != transform)//본인이 아닐때
                {
                    Debug.Log("Ball");
                    curPos = wallHit.point;
                    ChangeColor(color_Name, wallHit.collider.GetComponent<Ball>().color_Name, "Line");
                    ChangeColor(color_Name, wallHit.collider.GetComponent<Ball>().color_Name, "Line2");
                }
            }
   
        }
        else //아무에게도 부딪히지 않았을때
        {
            Debug.Log("None");
            ChangeColor(color_Name, color_Name, "Line");
            ChangeColor(color_Name, color_Name, "Line2");
            //ResetColor();
        }

        foreach (LineRenderer line in lines)
        {
            line.positionCount = 2;
        }
        curPos = new Vector3(curPos.x, 0.5f, curPos.z);
        newPos = new Vector3(newPos.x, 0.5f, newPos.z);
        if (count == 1 || distance < 1f)//일정 거리에 도달하지 못하면
        {
            foreach (LineRenderer line in lines)
            {
                line.positionCount = 0;//라인을 생성하지 않음
            }
            return false;
        }
        if (count == 2)//선이 1개일때
        {
            lines[0].SetPosition(0, transform.position);
            lines[0].SetPosition(1, curPos);
            lines[1].positionCount = 0;
        }
        if (count == 3)//선이 2개일때
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
        if (isPassingPaint)//만약 페인트를 지나간다면 페인트색으로 변경
        {
            Set_Ball("Line", paintColor);
            return;
        }

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
            if (kind == ObjectKind.Ball)
            {
                Ball hit_Ball = coll.gameObject.GetComponent<Ball>();
                ChangeColor(color_Name, hit_Ball.color_Name);
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

        Ball_Color originColor = color_Name;
        if (call == "Ball")
        {
            if (c1 == c2) return;
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
            if (originColor == color_Name) return;
            Set_Ball();
        }
        else if (call == "Line")
        {
           

            //Ball_Color color;
            if (Orange(c1, c2))
            {
                color_Line1 = Ball_Color.Orange;
            }
            else if (Green(c1, c2))
            {
                color_Line1 = Ball_Color.Green;

            }
            else if (Purple(c1, c2))
            {
                color_Line1 = Ball_Color.Purple;
            }
            else if (c1 == c2)
            {
                color_Line1 = c1;
                //Set_Ball("Line", c1);//자기색으로 지정
            }
            else
            {
                color_Line1 = Ball_Color.Black;
            }
          
            Set_Ball("Line", color_Line1);//섞인 색으로 지정

            if (isPassingPaint)//만약 페인트를 지나간다면 페인트색으로 변경
            {
                color_Line1 = paintColor;
                Set_Ball("Line", color_Line1);
            }
        }
        else if (call == "Line2")
        {
            

            //Ball_Color color;
            if (Orange(c1, c2))
            {
                color_Line2 = Ball_Color.Orange;
            }
            else if (Green(c1, c2))
            {
                color_Line2 = Ball_Color.Green;

            }
            else if (Purple(c1, c2))
            {
                color_Line2 = Ball_Color.Purple;
            }
            else if (c1 == c2)
            {
                color_Line2 = c1;
                //Set_Ball("Line2", c1);//자기색으로 지정
            }
            else
            {
                color_Line2 = Ball_Color.Black;
            }
            Set_Ball("Line2", color_Line2);//섞인 색으로 지정

            if (isPassingPaint2)//만약 페인트를 지나간다면 페인트색으로 변경
            {
                color_Line2 = paintColor;
                Set_Ball("Line2", color_Line2);
            }
        }
        else//ColorChange에 부딪혔을때
        {
            color_Name = c1;
            if (originColor == color_Name) return;
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
        if (ballKind == BallKind.Small) transform.localScale = new Vector3(1, 1, 1);
        else transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

    }

}

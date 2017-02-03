using UnityEngine;
using System.Collections;

[RequireComponent(typeof (Animator))]
[RequireComponent(typeof (CharacterController))]
public class ActionController : MonoBehaviour 
{
    private Animator m_animator;
    private CharacterController m_character;
    public float m_moveSpeed = 1f;
    private bool m_moving = false;

    private int m_cornerIndex = 0;
    private UnityEngine.AI.NavMeshPath m_navPath;

    private Quaternion m_destRotation = Quaternion.identity;
    public float m_rotateSpeed = 100f;
    //private Vector2 m_touchStartPos = Vector2.zero;

    public Animator[] m_monsterAnim = new Animator[4];

    void Start () 
    {
        m_animator = GetComponent<Animator>();
        m_character = GetComponent<CharacterController>();
        m_navPath = new UnityEngine.AI.NavMeshPath();
	}
	
	void Update ()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 destPoint = hit.point;
                if (UnityEngine.AI.NavMesh.CalculatePath(transform.position, destPoint, -1, m_navPath))
                {
                    m_moving = true;
                    m_cornerIndex = -1;
                    Utility.Log("auto pathing begin,dest x:" + destPoint.x + ",z:" + destPoint.z);
                }
            }
        }

        if (m_navPath.corners.Length > 0)
        {
            bool changeCorner = false;
            if (m_cornerIndex < 0)
            {
                m_cornerIndex = 0;
                changeCorner = true;
            }

            if (0.5f > Vector3.Distance(transform.position, m_navPath.corners[m_cornerIndex]))
            {
                m_cornerIndex++;
                if (m_navPath.corners.Length <= m_cornerIndex)
                {
                    m_moving = false;
                    m_navPath.ClearCorners();
                    Utility.Log("auto pathing end,current pos,x:" + transform.position.x + ",z:" + transform.position.z);
                }
                else
                {
                    changeCorner = true;
                }
            }

            if (changeCorner)
            {
                m_destRotation = xInputManager.GetWorldRotation(
                    m_navPath.corners[m_cornerIndex].x - transform.position.x,
                    m_navPath.corners[m_cornerIndex].z - transform.position.z);

                Utility.Log("next nav corner:" + m_navPath.corners[m_cornerIndex].x + "," + m_navPath.corners[m_cornerIndex].z);
            }
        }
                  
        float h = xInputManager.GetHorizontalValue();
        float v = xInputManager.GetVerticalValue();
        if (h != 0f || v != 0f)
        {
            m_moving = true;
            m_destRotation = xInputManager.GetWorldRotation(h, v);          
        }
        else
        {
            if (m_navPath.corners.Length <= 0)
            {
                m_moving = false;
            }
        }

        if (m_moving)
        {
            m_character.SimpleMove(transform.forward * m_moveSpeed);
            //m_animator.SetFloat("Forward", 1f);
            m_animator.SetBool("Run", true);
        }
        else
        {
            //m_animator.SetFloat("Forward", 0f);
            m_animator.SetBool("Run", false);
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, m_destRotation, m_rotateSpeed);

        /*
        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                m_touchStartPos = t.position;
            }
            else if (t.phase == TouchPhase.Ended)
            {
                Vector2 deltaPos = t.position - m_touchStartPos;
                if (deltaPos.x > 0f)
                {
                    if (deltaPos.y > 0f && deltaPos.y > deltaPos.x)
                    {
                        Utility.Log("touch slide to up");
                    }
                    else if (deltaPos.y < 0f && Mathf.Abs(deltaPos.y) > deltaPos.x)
                    {
                        Utility.Log("touch slide bottom");
                    }
                    else
                    {
                        Utility.Log("touch slide right");
                    }
                }
                else if (deltaPos.x < 0f)
                {
                    if (deltaPos.y > 0f && deltaPos.y > Mathf.Abs(deltaPos.x))
                    {
                        Utility.Log("touch slide to up");
                    }
                    else if (deltaPos.y < 0f && Mathf.Abs(deltaPos.y) >  Mathf.Abs(deltaPos.x))
                    {
                        Utility.Log("touch slide bottom");
                    }
                    else
                    {
                        Utility.Log("touch slide left");
                    }
                }

                m_touchStartPos = Vector2.zero;
            }
        }
        */
    }
        
    public void OnAttack()
    {
        for (int i = 0; i < 4; i++)
        {
            Animator monsterAnim = m_monsterAnim[i];
            Vector3 direction = monsterAnim.transform.position - transform.position;
            if (Vector3.Angle(direction, transform.forward) < 60f)
            {
                if (Vector3.Distance(monsterAnim.transform.position, transform.position) < 2f)
                {
                    Animator theAnimtor = monsterAnim.gameObject.GetComponent<Animator>();
                    if (theAnimtor)
                    {
                        AnimatorStateInfo theState = theAnimtor.GetCurrentAnimatorStateInfo(0);
                        if (theState.IsName("Idle") || theState.IsName("Walk"))
                        {
                            theAnimtor.SetBool("Fall", true);
                            Utility.Log("Monster was attacked:" + monsterAnim.name);
                        }                
                    }
                }
            }

        }
        /*
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);
        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; ++i)
            {
                GameObject theGo = colliders[i].gameObject;
                Animator theAnimtor = theGo.GetComponent<Animator>();
                if (theAnimtor)
                {
                    AnimatorStateInfo theState = theAnimtor.GetCurrentAnimatorStateInfo(0);
                    if (theState.IsName("Idle") || theState.IsName("Walk"))
                    {
                        theAnimtor.SetBool("Fall", true);
                        Utility.Log("object attacked:" + theGo.name);
                    }                
                }
            }
        }
        */
    }
}

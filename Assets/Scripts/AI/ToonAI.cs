using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Animator))]
[RequireComponent(typeof (CharacterController))]
public class ToonAI : MonoBehaviour {

    private Animator m_animator;
    private CharacterController m_character;
    public float m_moveSpeed = 30f;
    private bool m_moving = false;

    private int m_cornerIndex = 0;
    private UnityEngine.AI.NavMeshPath m_navPath;

    private Quaternion m_destRotation = Quaternion.identity;
    public float m_rotateSpeed = 100f;

    private float m_lastTime = 0f;
    public Transform m_target;

	// Use this for initialization
	void Start () {
        m_animator = GetComponent<Animator>();
        m_character = GetComponent<CharacterController>();

        m_navPath = new UnityEngine.AI.NavMeshPath();

	}
	
	// Update is called once per frame
	void Update () {
		
        if (Time.time - m_lastTime > 3f)
        {            
            int rand = Random.Range(0, 3);  
            if (0 == rand)
            {
                m_animator.SetBool("Attack", false);
                m_animator.SetFloat("Speed", 0.9f);
                Vector3 lookPosition = new Vector3(m_target.position.x, transform.position.y, m_target.position.z);
                transform.LookAt(lookPosition);
            }
            else if (1 == rand)
            {
                m_animator.SetBool("Attack", true);
                m_animator.SetFloat("Speed", 0f);
            }
            else if (2 == rand)
            {
                m_animator.SetBool("Attack", false);
                m_animator.SetFloat("Speed", 0.3f);
                Vector3 lookPosition = new Vector3(m_target.position.x, transform.position.y, m_target.position.z);
                transform.LookAt(lookPosition);
            }
            m_lastTime = Time.time;
        }

        AnimatorStateInfo stateInfo = m_animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Walk") || stateInfo.IsName("Charge"))
        {
           // transform.Translate(Vector3.forward*Time.deltaTime, Space.Self);
            m_character.SimpleMove(transform.forward * m_moveSpeed * Time.deltaTime);

        }
	}

    public void OnAttack()
    {
        Vector3 direction = m_target.position - transform.position;
        if (Vector3.Angle(direction, transform.forward) < 60f)
        {
            if (Vector3.Distance(m_target.position, transform.position) < 3f)
            {
                Animator theAnimtor = m_target.gameObject.GetComponent<Animator>();
                if (theAnimtor)
                {
                    AnimatorStateInfo theState = theAnimtor.GetCurrentAnimatorStateInfo(0);
                    if (theState.IsName("Idle") || theState.IsName("Walk"))
                    {
                        theAnimtor.SetTrigger("DamageDown");
                        Utility.Log("MainCharacter was attacked:" + m_target.name);
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
                if (theGo.layer == 8)
                    continue;
                
                Animator theAnimtor = theGo.GetComponent<Animator>();
                if (theAnimtor)
                {
                    AnimatorStateInfo theState = theAnimtor.GetCurrentAnimatorStateInfo(0);
                    if (theState.IsName("Idle") || theState.IsName("Run"))
                    {
                        theAnimtor.SetTrigger("DamageDown");
                        Utility.Log("object attacked:" + theGo.name);
                    }                
                }
            }
        }
        */
    }
}

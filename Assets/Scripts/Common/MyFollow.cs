using UnityEngine;
using System.Collections;

public class MyFollow : MonoBehaviour 
{
	public Transform m_target = null;

    private float m_xDistance;
    private float m_yDistance;

	private bool m_follow;
	public bool Follow
	{
		get { return m_follow; }
		set {
            m_follow = value; 
            if(m_follow)
            {
                m_xDistance = m_target.position.x - this.transform.position.x;
                m_yDistance = m_target.position.y - this.transform.position.y;
                Debug.Log("MyFollow:xdist:"+m_xDistance+",ydist:"+m_yDistance);
            }
        }
	}
    	
	// Use this for initialization
	void Start () 
	{
        m_xDistance = m_target.position.x - this.transform.position.x;
        m_yDistance = m_target.position.y - this.transform.position.y;
        m_follow = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!m_follow)
        {
            return;
        }

		if (m_target == null) 
		{
			Debug.LogWarning("Follow target is null");
			return;
		}

       // //float targetY = Mathf.Lerp(transform.position.y, m_Player.position.y, ySmooth*Time.deltaTime);

        float newX = m_target.position.x - m_xDistance;
        float newY = (m_target.position.y - m_yDistance) > 0 ? (m_target.position.y - m_yDistance) : 0f;
        this.transform.position = new Vector3(newX, newY, this.transform.position.z);
        //Debug.Log("MyFollow,now pos:x:"+transform.position.x+",y:"+transform.position.y+",z:"+transform.position.z);
	}
}

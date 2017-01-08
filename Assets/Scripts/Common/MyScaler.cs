using UnityEngine;
using System.Collections;

public class MyScaler : MonoBehaviour 
{
	public Camera m_camera = null;
	
	void Start () 
	{
        if (null == m_camera)
        {
            Debug.LogError("MyScaler: the camera is null");
            return;
        }

		float cameraHeight = m_camera.orthographicSize * 2.0f;
		float cameraWidth = m_camera.aspect * cameraHeight;

		if (cameraHeight/cameraWidth >= 1136.0f / 640.0f) 
        {
			transform.localScale = new Vector3 (cameraHeight / 1136.0f, cameraHeight / 1136.0f, 1.0f);
		} 
        else 
        {
			transform.localScale = new Vector3 (cameraWidth / 640.0f, cameraWidth / 640.0f, 1.0f);
		}
	}
}

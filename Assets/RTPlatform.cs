using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTPlatform : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("Runtime platform: " + PathSystem.GetPlatformStr());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

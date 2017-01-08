using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TestBtn : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	Button btn = (Button)gameObject.GetComponent("Button");		

        float v1 = Input.GetAxis("Horizontal");
        float raw1 = Input.GetAxisRaw("Horizontal");

        float v2 = Input.GetAxis("Vertical");
        float raw2 = Input.GetAxisRaw("Vertical");

        float v3 = Input.GetAxis("Fire1");
        float raw3 = Input.GetAxisRaw("Fire1");

        float v4 = Input.GetAxis("Fire2");
        float raw4 = Input.GetAxisRaw("Fire2");

        float v5 = Input.GetAxis("Fire3");
        float raw5 = Input.GetAxisRaw("Fire3");

        float v6 = Input.GetAxis("Mouse X");
        float raw6 = Input.GetAxisRaw("Mouse X");

        float v7 = Input.GetAxis("Mouse Y");
        float raw7 = Input.GetAxisRaw("Mouse Y");

        Debug.Log("h,hraw,v,vraw,f1,fraw1,f2,fraw2,f3,fraw3,mousex,mxraw,mousey,myraw");
        Debug.Log(v1+","+raw1+","+v2+","+raw2+","+v3+","+raw3+","+v4+","+raw4+","+v5+","+raw5+","+v6+","+raw6+","+v7+","+raw7);

	}


	public void btnClick(GameObject go_)
	{
		Debug.Log("btn clicked:" + go_.name);
	}
}

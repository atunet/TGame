using UnityEngine;
using System.Collections;

public class AddTorque : MonoBehaviour
{
    bool set = true;

    void Start()
    {
        Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D>();
        
        //rigidbody.AddTorque(10f);
        //rigidbody.MoveRotation(10f*Time.deltaTime);
    }

	void FixedUpdate ()
    {
        Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D>();

        //rigidbody.AddTorque(10f);
        //rigidbody.MoveRotation(100f*Time.deltaTime);
        //rigidbody.AddForceAtPosition(Vector2.right * 1f, new Vector2(1f, 1f));
        //rigidbody.AddTorque(5f);
       if (set)
        {
            rigidbody.AddForceAtPosition(Vector2.up * 5f, new Vector2(-1f, 0f));
            set = false;
        }
    }

    void OnPress()
    {
        //Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D>();
        
        //rigidbody.AddTorque(10f);
    }
}

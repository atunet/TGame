using UnityEngine;
using System.Collections;

public class AddForce : MonoBehaviour 
{
    public float currentSpeed = 200f;
    public float maxVelocity = 4f;

    void Start ()
    {
    }

    void FixedUpdate ()
    {
        Rigidbody2D rigidbody2d = gameObject.GetComponent<Rigidbody2D>();
        if (rigidbody2d.velocity.y < maxVelocity)
        {
            rigidbody2d.AddForce(Vector2.up * currentSpeed);
        } else
        {
            rigidbody2d.gravityScale = 0;
        }

        Debug.Log("velocity:" + rigidbody2d.velocity);
        /*
        Rigidbody2D rigidbody2d = gameObject.GetComponent<Rigidbody2D>();

        if (rigidbody2d.velocity.y < 2f)
        {
            rigidbody2d.AddForce(Vector2.up * speed);
        }
        */
       // Debug.Log("velocity:" + rigidbody2d.velocity);
    }
}

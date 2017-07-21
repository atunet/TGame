using UnityEngine;
using System.Collections;

public class AnimEvent : MonoBehaviour 
{
    public void OnTimetickEnd()
    {
        Destroy(gameObject);
    }
}

using UnityEngine;
using System.Collections;

public class InitBattleController : MonoBehaviour 
{
    void Start () 
    {
        if (!GlobalRef.Init())
            return;
    }
}

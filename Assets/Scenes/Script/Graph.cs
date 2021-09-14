using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    protected static Vector3 zero;
    protected virtual void OnDrawGizmos ()
    {
        GraphHelp.zero = zero = transform.position;
        GraphHelp.DrawGraph ();
    }
}

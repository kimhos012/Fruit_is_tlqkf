using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gizmoss : MonoBehaviour
{
    public Color c0l = Color.white;
    public float s1ze = 1f;


    private void OnDrawGizmos()
    {
        Gizmos.color = c0l;

        Gizmos.DrawSphere(transform.position, s1ze);
    }

}

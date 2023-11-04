using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScore : MonoBehaviour
{
    GameObject a;
    private void Start()
    {
        a = GameObject.Find("");
        if (a != null)
            return;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("AddScore"))
        {
            collision.gameObject.GetComponent<GetScore>();
        }
    }
}

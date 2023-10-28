using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviour
{
    Rigidbody rb;


    public float Speed = 5f;
    public float Duration = 1f;
    public GameObject effect;


    private void Start()
    {
        Invoke("ActiveParticle", Duration);
        rb = GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * (Speed * 10));
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        ActiveParticle();
    }
    
    void ActiveParticle()
    {
        rb.velocity = Vector3.zero;
        Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaa");
        Invoke("Destroy", 0.5f);
        effect.SetActive(true);
    }
    
    void Destroy()
    {
        Destroy(gameObject);
    }
}

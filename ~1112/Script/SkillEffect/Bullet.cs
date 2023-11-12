using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR;

public class Bullet : MonoBehaviour
{
    Rigidbody rb;


    public float Speed = 5f;
    public float Duration = 1f;

    public bool leftMesh;

    public GameObject effect;
    public GameObject Meshss;

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
        if(!leftMesh)
            Meshss.SetActive(false);
        Invoke("Destroy", 0.5f);
        effect.SetActive(true);
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}

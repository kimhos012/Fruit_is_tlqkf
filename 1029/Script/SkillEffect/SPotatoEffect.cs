using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPotatoEffect : MonoBehaviour
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
        Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaa");

        Destroy(gameObject);
        //effect.SetActive(true);
    }

}

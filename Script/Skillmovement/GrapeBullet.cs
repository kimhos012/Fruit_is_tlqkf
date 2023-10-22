using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapeBullet : MonoBehaviour
{
    Rigidbody rb;
    bool collider;

    public float ThrowSpeed = 5f;
    public GameObject effect;

    private void OnEnable()
    {
        effect.SetActive(false);
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<SphereCollider>().enabled;

        rb.AddForce(Vector3.up * ThrowSpeed);
        rb.AddForce(Vector3.forward * ThrowSpeed / 2);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Invoke("Destroy", 0.7f);
        rb.useGravity = false;
        rb.velocity = new Vector3(0, 0, 0);
        enabled = false;
        effect.SetActive(true);

    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}

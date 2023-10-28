using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GeoBullet : MonoBehaviour
{
    Rigidbody rb;
    Collider col;
    Collider tri;

    public float HorizSpeed;
    public float VertiSpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<SphereCollider>();
        tri = GetComponent<BoxCollider>();

        rb.AddForce(transform.up * VertiSpeed * 10);
        rb.AddForce(transform.forward * HorizSpeed * 10);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(transform.up * -1 * 1000);
        col.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        PhotonNetwork.Instantiate("MandarinFloor(sphere)", transform.position, Quaternion.Euler(0, 0, 0), 0);     //place Floor
        Destroy(gameObject);
    }
}

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
    public GameObject PlaceFloor;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<SphereCollider>();
        tri = GetComponent<BoxCollider>();

        rb.AddForce(transform.up * VertiSpeed * 10);
        rb.AddForce(transform.forward * HorizSpeed * 10);
        rb.angularVelocity = Vector3.right * (Random.Range(-10,10));
        rb.angularVelocity = Vector3.forward * (Random.Range(-10,10));
        rb.angularVelocity.Normalize();
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        rb.velocity = Vector3.zero;
        transform.eulerAngles = Vector3.zero;
        rb.AddForce(-transform.up * 1000);
        col.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {       
        if(!other.gameObject.CompareTag("Player"))
        {
            Instantiate(PlaceFloor, this.transform.position, this.transform.rotation);

            Destroy(gameObject);
        }
        
    }
}

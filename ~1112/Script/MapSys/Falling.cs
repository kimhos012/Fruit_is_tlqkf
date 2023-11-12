using UnityEngine;
using UnityEngine.Tilemaps;

public class Falling : MonoBehaviour
{
    public float Speed;
    public GameObject particle;
    Rigidbody rb;
    MeshRenderer mr;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        mr = GetComponent<Transform>().GetChild(0).GetComponent<MeshRenderer>();

        rb.AddForce(Vector3.up * -1 * Speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var contact = collision.GetContact(0);
        rb.velocity = Vector3.zero;
        //RemoveSaRaRak(1);

        //var obj = Instantiate(particle,contact.point,Quaternion.LookRotation(-contact.normal));
        //Destroy(obj, 2f);
        Destroy(this.gameObject, 1f);
    }

    void RemoveSaRaRak(float a)
    {
        while(a > 0)
        {
            a -= Time.deltaTime;
            
        }
    }
}

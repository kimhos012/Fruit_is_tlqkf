using Photon.Pun;
using System.Collections;
using UnityEngine;

public class Falling : MonoBehaviour
{
    public float Speed;
    public GameObject particle;
    public GameObject warningDisplay;

    bool isFalling = false;

    private void OnEnable()
    {
        ShootRay();
        isFalling = true;

    }
    void ShootRay()
    {
        int layer = -1 - (1 << LayerMask.NameToLayer("Proj"));
        RaycastHit hit;
        Debug.DrawRay(transform.position, -transform.up, Color.red);
        if(Physics.Raycast(transform.position, -transform.up , out hit, 100 ,layer))
        {
            Vector3 contact = hit.point;
            GameObject waringD = Instantiate(warningDisplay, contact, Quaternion.identity);
            Destroy(waringD , 2f);
        }
    }
    private void FixedUpdate()
    {
        if(isFalling)
        {
            transform.Translate(new Vector3(0 , -Speed*Time.deltaTime , 0));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit");
        isFalling = false;
        var contact = collision.GetContact(0);
        
        Destroy(this.gameObject, 1f);
    }
}

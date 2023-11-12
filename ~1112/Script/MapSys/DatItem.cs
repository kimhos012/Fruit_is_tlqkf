using UnityEngine;

public class DatItem : MonoBehaviour
{

    public bool IsCrown;
    bool follow;
    Vector3 getPos;

    private void Start()
    {
        if(!IsCrown)
            Destroy(gameObject, 2);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsCrown)
        {
            FollowPlayer(collision.gameObject);
            follow = true;

        }
        else
        {

        }
    }


    void FollowPlayer(GameObject pos)
    {
        if (follow)
        {
            getPos = pos.GetComponent<Transform>().position;

            getPos.y += 0.5f;

            transform.position = getPos;
        }
    }
}
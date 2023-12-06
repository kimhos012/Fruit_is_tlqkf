using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    public bool OneUse;
    public float slowness;
    float curTime = 0;

    public float Duration = 4f;

    private void FixedUpdate()
    {
        if (Duration >= curTime)
        {
            curTime += Time.deltaTime;
        }
        else
        {
            curTime = 0;
            if (OneUse)
            {
                Destroy(gameObject);
            }
            else
                gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && (other.GetComponent<Controller>().isBind == false))
        {
            other.GetComponent<Controller>().speed = 0.5f;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<Controller>().isBind == false)
        {
            other.GetComponent<Controller>().speed = 1;
        }
    }
}

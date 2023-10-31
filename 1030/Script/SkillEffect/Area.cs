using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    public bool OneUse;
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
            if (OneUse)
            {
                Destroy(gameObject);
            }
            else
                gameObject.SetActive(false);
        }
    }

}

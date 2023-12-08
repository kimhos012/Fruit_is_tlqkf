using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaFollowPlayer : MonoBehaviour
{
    float curTime;
    public float Duration = 4f;

    private void OnEnable()
    {
        curTime = 0;
    }

    private void FixedUpdate()
    {
        if (Duration >= curTime)
        {
            curTime += Time.deltaTime;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}

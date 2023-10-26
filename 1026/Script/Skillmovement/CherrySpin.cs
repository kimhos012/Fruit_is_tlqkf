using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherrySpin : MonoBehaviour
{
    public float roationSpeed;
    bool move = true;

    void Update()
    {
        if(move)
            transform.localEulerAngles += new Vector3(0, Time.deltaTime * roationSpeed, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        move = false;
    }
}

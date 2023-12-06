using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAtt : MonoBehaviour
{
    public float time;
    void Start()
    {
        Invoke("Active", time);
    }
    void Active()
    {
        this.gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPotatoEffect : MonoBehaviour
{
    float GetDura;
    float curTime;
    GameObject curObj;

    float Duration;
    public GameObject Vine1;
    public GameObject Vine2;
    public GameObject Vine3;


    private void Start()
    {

        //Duration½Ã°£¿¡ µû¶ó¼­ µ¢±¼ÀÌ ¿Ã¶ó°¬´Ù ³»·Á°¬´Ù ÇÒ°ÍÀÌ´Ù.


    }

    private void Update()
    {
        curTime += Time.deltaTime;
        if(curTime >= (GetDura * 0.5))      //50%Time
        {

        }
    }
}

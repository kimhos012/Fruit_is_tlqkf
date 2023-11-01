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
        Duration = GetComponent<DamageProjectile>().Damage;

        //Duration½Ã°£¿¡ µû¶ó¼­ µ¢±¼ÀÌ ¿Ã¶ó°¬´Ù ³»·Á°¬´Ù ÇÒ°ÍÀÌ´Ù.


    }

    private void Update()
    {
        curTime += Time.deltaTime;
        if(curTime >= (GetDura * 0.5))      //50%Time
        {

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            curObj = other.gameObject;
        }
    }
    IEnumerator asd()
    {
        curObj.GetComponent<MovementController>().isBind = true;
        yield return new WaitForSeconds(Duration);
        curObj.GetComponent<MovementController>().isBind = false;
        curObj = null;
        yield return null;
    }
}

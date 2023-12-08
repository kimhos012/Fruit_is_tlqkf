using System.Collections;
using UnityEngine;

public class Damage : MonoBehaviour
{
    PlayerData playerData;
    Controller controller;
    DamageProjectile projectileSetting;
    void Start()
    {
        playerData = GetComponent<PlayerData>();
        controller = GetComponent<Controller>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("DamageObject"))
        {
            projectileSetting = collision.gameObject.GetComponent<DamageProjectile>();
            playerData.Hp -= projectileSetting.Damage;
            collision.collider.enabled = false;
        }

        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("DamageObject"))
        {
            projectileSetting = other.gameObject.GetComponent<DamageProjectile>();
            if (projectileSetting.projEffect != Effect.none)        //효과
            {
                if (projectileSetting.projEffect == Effect.Slowness)     //구속
                {
                    controller.speed = projectileSetting.intensity;
                }
                else if (projectileSetting.projEffect == Effect.Fire)    //불
                {
                    if (!dotStart)
                    {
                        StartCoroutine(DotDam((int)projectileSetting.intensity));
                    }
                }
                else
                {                                                       //묶음
                    StartCoroutine(Bind(projectileSetting.intensity));
                }
            }
            else
            {
                return;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("DamageObject"))
        {
            controller.speed = 1f;
        }
    }
    bool dotStart;

    IEnumerator DotDam(int times)           //도트데미지
    {
        dotStart = true;
        for (int i = 0; i < times; i++)
        {
            playerData.Hp -= projectileSetting.Damage;
            yield return new WaitForSeconds(0.5f);
        }
        dotStart = false;
    }
    IEnumerator Bind(float time)            //행동불가
    {
        controller.isBind = true;
        yield return new WaitForSeconds(time);
        controller.isBind = false;
    }
}

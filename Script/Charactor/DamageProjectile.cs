using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageProjectile : MonoBehaviour
{
    [Header("데미지 타입")]
    public Damtype damageType;

    [Space(10f)]

    [Header("Main Damage")]
    [Tooltip("초당 데미지면 초당 이 피해만큼 들어갑니다.")]
    [Range(0,120)]
    public int Damage;
    [Header("First Dam")]
    [Tooltip("도트 데미지일 경우, 이 값을 설정 시, 타격 시 데미지가 들어갑니다.")]
    [Range(0, 120)]
    public int dotFirstDam;

    [Space(10f)]

    [Header("Knockback Diatance")]
    [Range(0, 2)]
    public int knockbackPower = 0;

    [Space(10f)]
    [Header("Effect")]
    public GameObject effect;

    private void OnCollisionEnter(Collision collision)
    {
        /*
        var contect = collision.GetContact(0);
        var obj = Instantiate(effect, contect.point, Quaternion.LookRotation(-contect.normal));
        */

        Invoke("SaSaHeDestroy", 1f);
    }

    void SaSaHeDestroy(Color _color , float a)
    {
        _color = effect.GetComponent<MeshRenderer>().material.color;

        for (int i=0;i<100; i++)
        {
            _color = new Color(1, 1, 1, a);
            a += 0.01f;
        }
        Destroy(effect);
    }       //공통적으로 생성되는 스킬 흔적?
}

public enum Damtype
{
    Instant,
    Dot,
    InstantDeath
}

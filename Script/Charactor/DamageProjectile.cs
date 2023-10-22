using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageProjectile : MonoBehaviour
{
    [Header("������ Ÿ��")]
    public Damtype damageType;

    [Space(10f)]

    [Header("Main Damage")]
    [Tooltip("�ʴ� �������� �ʴ� �� ���ظ�ŭ ���ϴ�.")]
    [Range(0,120)]
    public int Damage;
    [Header("First Dam")]
    [Tooltip("��Ʈ �������� ���, �� ���� ���� ��, Ÿ�� �� �������� ���ϴ�.")]
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
    }       //���������� �����Ǵ� ��ų ����?
}

public enum Damtype
{
    Instant,
    Dot,
    InstantDeath
}

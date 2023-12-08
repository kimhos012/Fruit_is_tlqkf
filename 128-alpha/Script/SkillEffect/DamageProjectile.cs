using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageProjectile : MonoBehaviour
{
    [Header("데미지 타입")]
    public Damtype damageType;

    [Space(10f)]
    [Header("데미지")]
    public int Damage;

    [Space(10f)]
    [Header("EffectControll")]
    public Effect projEffect;
    public float intensity;
}

public enum Damtype
{
    Instant,
    InstantDeath
}
public enum Effect
{
    none,
    Slowness,
    Fire,
    bind
}
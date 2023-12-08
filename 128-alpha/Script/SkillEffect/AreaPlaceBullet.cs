using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaPlaceBullet : MonoBehaviour
{
    public float timer;

    public ParticleSystem MainFire;
    public ParticleSystem SubFire;

    public GameObject DamageTrigger;

    public float Duration = 1f;

    private void OnEnable()
    {
        timer = 0;
        DamageTrigger.SetActive(true);
    }
    private void Update() => stopParticle();
    void stopParticle()
    {
        if (timer >= (Duration * 0.5))
        {
            SubFire.Stop();
            if (timer >= (Duration * 0.9))
            {
                MainFire.Stop();
                DamageTrigger.SetActive(false);
                if (timer >= Duration)
                    gameObject.SetActive(false);
            }
        }
        timer += Time.deltaTime;
    }
}

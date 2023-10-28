using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{

    // 사망 후 투명 처리를 위한 MeshRenderer 컴포넌트의 배열
    private Renderer[] renderers;
    // 캐릭터의 초기 생명치
    private int initHp = 100;
    // 캐릭터의 현재 생명치
    public int yourHp = 100;
    private Animator anim;
    private CharacterController cc;
    // 애니메이터 뷰에 생성한 파라미터의 해시값 추출
    private readonly int hashDie = Animator.StringToHash("Die");
    private readonly int hashRespawn = Animator.StringToHash("Respawn");

    //데미지설정값을 위한 설정
    float _curTime;
    int Dam;
    int DC;

    void Awake()
    {
        // 캐릭터 모델의 모든 Renderer 컴포넌트를 추출한 후 배열에 할당
        renderers = GetComponentsInChildren<Renderer>();
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        //현재 생명치를 초기 생명치로 초깃값 설정
        yourHp = initHp;
    }
    void OnCollisionEnter(Collision coll)
    {
        
        if (yourHp > 0 && coll.collider.CompareTag("DamageObject"))
        {
            Dam = coll.gameObject.GetComponent<DamageProjectile>().Damage;

            switch (coll.gameObject.GetComponent<DamageProjectile>().damageType)
            {
                case Damtype.Instant:
                    //오브젝트에게 있는 데미지를 호로록감지해서 체력이 아야함
                    yourHp -= Dam;
                    break;
                case Damtype.InstantDeath:
                    StartCoroutine("Sine");

                    break;
            }
            //오브젝트에게 있는 데미지를 호로록감지해서 체력이 아야함
            yourHp -= coll.gameObject.GetComponent<DamageProjectile>().Damage;
        }
        if (yourHp <= 0)
        {
            StartCoroutine(Sine());
        }
    }
    private void OnTriggerStay(Collider other)     //도트딜은 Trigger
    {
        if(yourHp > 0 && other.CompareTag("DamageObject"))
        {
            DC = other.gameObject.GetComponent<DamageProjectile>().dotCount;

            if(DC == 0)     //Detect EffectArea
            {
                if (_curTime >= 1)
                {
                    _curTime += Time.deltaTime;
                }
                else
                {
                    _curTime = 0;
                    yourHp -= Dam;
                }
            }
            else
            {
                StartCoroutine("DotDamageSys");
            }
        }
        if (yourHp <= 0)
        {
            if(!dotIsStarted)
                StartCoroutine(Sine());
        }
    }
    bool dotIsStarted;

    IEnumerator DotDamageSys()      //코루틴으로 분리한 이유는 저거가 TriggerStay라서
    {
        dotIsStarted = true;
        var _wait = new WaitForSeconds(1f);
        
        for (int a = 0; a > DC; a++)        //횟수만큼 피해
        {
            yourHp -= Dam;
            if (yourHp <= 0)
            {
                StartCoroutine(Sine());
            }
            yield return _wait;
        }
        yield return null;
    }

    IEnumerator Sine()
    {
        // CharacterController 컴포넌트 비활성화
        cc.enabled = false;
        // 리스폰 비활성화
        anim.SetBool(hashRespawn, false);
        // 캐릭터 사망 애니메이션 실행
        anim.SetTrigger(hashDie);
        yield return new WaitForSeconds(3.0f);
        // 리스폰 활성화
        anim.SetBool(hashRespawn, true);
        // 캐릭터 투명 처리
        SetPlayerVisible(false);
        yield return new WaitForSeconds(1.5f);
        // 리스폰 시 생명 초깃값 설정
        yourHp = 100;
        // 캐릭터를 다시 보이게 처리
        SetPlayerVisible(true);
        // CharacterController 컴포넌트 활성화
        cc.enabled = true;
    }
    //Renderer 컴포넌트를 활성/비활성화하는 함수
    void SetPlayerVisible(bool isVisible)
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].enabled = isVisible;
        }
    }

}

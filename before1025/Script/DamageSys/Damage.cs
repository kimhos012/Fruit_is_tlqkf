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
        // 생명 수치가 0보다 크고 충돌체의 태그가 BULLET인 경우에 생명 수치를 차감
        if (yourHp > 0 && coll.collider.CompareTag("Hitpoint"))
        {
            
            //오브젝트에게 있는 데미지를 호로록감지해서 체력이 아야함
            yourHp -= coll.gameObject.GetComponent<DamageProjectile>().Damage;

            if (yourHp <= 0)
            {
                StartCoroutine(Sine());
            }
        }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{

    // ��� �� ���� ó���� ���� MeshRenderer ������Ʈ�� �迭
    private Renderer[] renderers;
    // ĳ������ �ʱ� ����ġ
    private int initHp = 100;
    // ĳ������ ���� ����ġ
    public int yourHp = 100;
    private Animator anim;
    private CharacterController cc;
    // �ִϸ����� �信 ������ �Ķ������ �ؽð� ����
    private readonly int hashDie = Animator.StringToHash("Die");
    private readonly int hashRespawn = Animator.StringToHash("Respawn");
    void Awake()
    {
        // ĳ���� ���� ��� Renderer ������Ʈ�� ������ �� �迭�� �Ҵ�
        renderers = GetComponentsInChildren<Renderer>();
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        //���� ����ġ�� �ʱ� ����ġ�� �ʱ갪 ����
        yourHp = initHp;
    }
    void OnCollisionEnter(Collision coll)
    {
        // ���� ��ġ�� 0���� ũ�� �浹ü�� �±װ� BULLET�� ��쿡 ���� ��ġ�� ����
        if (yourHp > 0 && coll.collider.CompareTag("Hitpoint"))
        {
            
            //������Ʈ���� �ִ� �������� ȣ�ηϰ����ؼ� ü���� �ƾ���
            yourHp -= coll.gameObject.GetComponent<DamageProjectile>().Damage;

            if (yourHp <= 0)
            {
                StartCoroutine(Sine());
            }
        }
    }
    IEnumerator Sine()
    {
        // CharacterController ������Ʈ ��Ȱ��ȭ
        cc.enabled = false;
        // ������ ��Ȱ��ȭ
        anim.SetBool(hashRespawn, false);
        // ĳ���� ��� �ִϸ��̼� ����
        anim.SetTrigger(hashDie);
        yield return new WaitForSeconds(3.0f);
        // ������ Ȱ��ȭ
        anim.SetBool(hashRespawn, true);
        // ĳ���� ���� ó��
        SetPlayerVisible(false);
        yield return new WaitForSeconds(1.5f);
        // ������ �� ���� �ʱ갪 ����
        yourHp = 100;
        // ĳ���͸� �ٽ� ���̰� ó��
        SetPlayerVisible(true);
        // CharacterController ������Ʈ Ȱ��ȭ
        cc.enabled = true;
    }
    //Renderer ������Ʈ�� Ȱ��/��Ȱ��ȭ�ϴ� �Լ�
    void SetPlayerVisible(bool isVisible)
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].enabled = isVisible;
        }
    }

}

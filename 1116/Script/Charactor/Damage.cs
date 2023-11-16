using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{

    // ��� �� ���� ó���� ���� MeshRenderer ������Ʈ�� �迭
    private Renderer[] renderers;
    // ĳ������ �ʱ� ����ġ
    private readonly int initHp = 120;
    // ĳ������ ���� ����ġ
    public int yourHp = 100;
    [HideInInspector]
    public float hpPers;            //ü�� �ۼ�Ʈ ��갪

    private Animator anim;
    private CharacterController cc;
    // �ִϸ����� �信 ������ �Ķ������ �ؽð� ����
    private readonly int hashDie = Animator.StringToHash("Die");
    private readonly int hashRespawn = Animator.StringToHash("Respawn");

    //�������������� ���� ����
    float _curTime;
    int Dam;
    int DC;

    void Awake()
    {
        // ĳ���� ���� ��� Renderer ������Ʈ�� ������ �� �迭�� �Ҵ�
        renderers = GetComponentsInChildren<Renderer>();
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        //���� ����ġ�� �ʱ� ����ġ�� �ʱ갪 ����
        yourHp = initHp;
    }
    private void Update()
    {
        hpPers = (float)yourHp / initHp;
    }//�� ���� DisplayHpBar�� �Ű��� ����

    void OnCollisionEnter(Collision coll)
    {
        
        if (yourHp > 0 && coll.collider.CompareTag("DamageObject"))
        {

            Dam = coll.gameObject.GetComponent<DamageProjectile>().Damage;

            switch (coll.gameObject.GetComponent<DamageProjectile>().damageType)
            {
                case Damtype.Instant:
                    //������Ʈ���� �ִ� �������� ȣ�ηϰ����ؼ� ü���� �ƾ���
                    yourHp -= Dam;
                    break;
                case Damtype.InstantDeath:
                    StartCoroutine("Sine");

                    break;
            }
            //������Ʈ���� �ִ� �������� ȣ�ηϰ����ؼ� ü���� �ƾ���
            yourHp -= coll.gameObject.GetComponent<DamageProjectile>().Damage;
        }
        if (yourHp <= 0)
        {
            StartCoroutine(Sine());
        }
    }

    private void OnTriggerStay(Collider other)     //��Ʈ���� Trigger
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
            StartCoroutine(Sine());
        }
    }
    bool dotIsStarted;

    IEnumerator DotDamageSys()      //�ڷ�ƾ���� �и��� ������ ���Ű� TriggerStay��
    {
        dotIsStarted = true;
        var _wait = new WaitForSeconds(1f);
        
        for (int a = 0; a > DC; a++)        //Ƚ����ŭ ����
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
        // CharacterController ������Ʈ ��Ȱ��ȭ
        DeathMovement(true);
        // ������ ��Ȱ��ȭ
        //anim.SetBool(hashRespawn, false);

        // ĳ���� ��� �ִϸ��̼� ����
        //anim.SetTrigger(hashDie);

        yield return new WaitForSeconds(3.0f);

        // ������ Ȱ��ȭ
        //anim.SetBool(hashRespawn, true);

        // ĳ���� ���� ó��
        SetPlayerVisible(false);
        yield return new WaitForSeconds(1.5f);


        // ������ �� ���� �ʱ갪 ����
        yourHp = 120;

        // ĳ���͸� �ٽ� ���̰� ó��
        SetPlayerVisible(true);

        // CharacterController ������Ʈ Ȱ��ȭ
        DeathMovement(false);
    }
    private void DeathMovement(bool a)
    {
        if(a)
        {
            cc.enabled = false;
            this.GetComponent<MovementController>().enabled = false;
        }
        else
        {
            cc.enabled = true;
            this.GetComponent<MovementController>().enabled = true;
        }
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

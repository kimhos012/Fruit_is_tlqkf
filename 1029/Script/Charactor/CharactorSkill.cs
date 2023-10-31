using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorSkill : MonoBehaviour
{
    [Header("��ų")]
    public GameObject ShootPos;


    [Space(10f)]
    public CharType charactorType;
    [Space(10f)]
    [SerializeField] GameObject Char1;
    [SerializeField] GameObject Char2;
    [SerializeField] GameObject Char3;
    [SerializeField] GameObject Char4;
    [SerializeField] GameObject Char5;
    [SerializeField] GameObject Char6;

    [Space(10f)]

    public float Cooldown;

    [HideInInspector]
    private int skillCooldown;       //��ų�� ���� ���ð�

    [SerializeField] GameObject GrapeBomb;

    [SerializeField] GameObject CherrySuriken;
    [SerializeField] float Cherry_Y = 35f;

    [SerializeField] GameObject MandarinArea;
    [SerializeField] GameObject OnionArea;
    [SerializeField] GameObject PepperArea;
    [SerializeField] GameObject SweetpotatoBind;

    private void Start()
    {
        Char1.SetActive(false);
        Char2.SetActive(false);
        Char3.SetActive(false);
        Char4.SetActive(false);
        Char5.SetActive(false);
        Char6.SetActive(false);
    }

    public void UseSkill()
    {
        if (Cooldown <= 0)
        {
            switch (charactorType)
            {
                case CharType.����:
                    GrapeSkill();
                    break;
                case CharType.ü��:
                    CherrySkill();

                    break;
                case CharType.��:
                    MandarinSkill();

                    break;
                case CharType.����:
                    OnionSkill();

                    break;
                case CharType.����:
                    PepperSkill();

                    break;
                case CharType.������:
                    SweetpotatoSkill();
                    break;
            }
            Cooldown = skillCooldown;
        }
    }


    void CheckMesh()     //ĳ������ ��� ����
    {
        switch(charactorType)
        {
            case CharType.����:
                Char1.SetActive(true);
                Char2.SetActive(false);
                Char3.SetActive(false);
                Char4.SetActive(false);
                Char5.SetActive(false);
                Char6.SetActive(false);
                break;

            case CharType.ü��:
                Char1.SetActive(false);
                Char2.SetActive(true);
                Char3.SetActive(false);
                Char4.SetActive(false);
                Char5.SetActive(false);
                Char6.SetActive(false);
                break;

            case CharType.��:
                Char1.SetActive(false);
                Char2.SetActive(false);
                Char3.SetActive(true);
                Char4.SetActive(false);
                Char5.SetActive(false);
                Char6.SetActive(false);
                break;

            case CharType.����:
                Char1.SetActive(false);
                Char2.SetActive(false);
                Char3.SetActive(false);
                Char4.SetActive(true);
                Char5.SetActive(false);
                Char6.SetActive(false);
                break;

            case CharType.����:
                Char1.SetActive(false);
                Char2.SetActive(false);
                Char3.SetActive(false);
                Char4.SetActive(false);
                Char5.SetActive(true);
                Char6.SetActive(false);
                break;

            case CharType.������:
                Char1.SetActive(false);
                Char2.SetActive(false);
                Char3.SetActive(false);
                Char4.SetActive(false);
                Char5.SetActive(false);
                Char6.SetActive(true);
                break;
            
        }
    }

    void CheckSkillC()           //ĳ������ ��ų ��ٿ�
    {
        switch (charactorType)      //��ٿ� ����
        {
            case CharType.����:
                skillCooldown = 7;

                break;
            case CharType.ü��:
                skillCooldown = 5;

                break;
            case CharType.��:
                skillCooldown = 8;

                break;
            case CharType.����:
                skillCooldown = 12;

                break;
            case CharType.����:
                skillCooldown = 7;

                break;
            case CharType.������:
                skillCooldown = 9;

                break;
        }
    }

    private void Update()
    {
        CheckMesh();
        CheckSkillC();

        if (Cooldown >= 0)
        {
            Cooldown -= Time.deltaTime;
        }
    }

    #region ĳ���Ϳ��� ��ų�� �߻��ϴ�,
    void GrapeSkill()
    {
        Instantiate(GrapeBomb, ShootPos.GetComponent<Transform>().position, ShootPos.GetComponent<Transform>().rotation);
    }

    void CherrySkill()
    {
        Quaternion rotation = ShootPos.GetComponent<Transform>().rotation;

        Vector3 rot = ShootPos.GetComponent<Transform>().eulerAngles;
        rot.y -= Cherry_Y;
        Quaternion a = Quaternion.Euler(rot);
        Instantiate(CherrySuriken, ShootPos.GetComponent<Transform>().position, a);
        rot.y += Cherry_Y;
        a = Quaternion.Euler(rot);
        Instantiate(CherrySuriken, ShootPos.GetComponent<Transform>().position, a);
        rot.y += Cherry_Y;
        a = Quaternion.Euler(rot);
        Instantiate(CherrySuriken, ShootPos.GetComponent<Transform>().position, a);

        //Instantiate(CherrySuriken, ShootPos.GetComponent<Transform>().position, rotation);
    }

    void MandarinSkill()
    {
        Instantiate(MandarinArea, ShootPos.GetComponent<Transform>().position, ShootPos.GetComponent<Transform>().rotation);
    }

    void OnionSkill()       //�� 2���� ��ų�� �÷��̾ ���󰡱� ������ Player�ȿ� ������
    {                       //���� SetActive���� ����
        OnionArea.SetActive(true);
    }
    void PepperSkill()
    {
        PepperArea.SetActive(true);
    }

    void SweetpotatoSkill()
    {
        Quaternion rotation = ShootPos.GetComponent<Transform>().rotation;

        Vector3 rot = ShootPos.GetComponent<Transform>().eulerAngles;
        rot.z += 90;
        Quaternion a = Quaternion.Euler(rot);

        Instantiate(SweetpotatoBind, ShootPos.GetComponent<Transform>().position, a);
              //None
    }
    #endregion
}
public enum CharType        //ĳ���� ����
{
    ����,
    ü��,
    ��,
    ����,
    ����,
    ������
}
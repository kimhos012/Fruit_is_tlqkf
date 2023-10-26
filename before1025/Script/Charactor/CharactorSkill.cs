using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorSkill : MonoBehaviour
{

    public CharType charactorType;
    [Space(10f)]
    public GameObject Char1;
    public GameObject Char2;
    public GameObject Char3;
    public GameObject Char4;
    public GameObject Char5;
    public GameObject Char6;

    [Space(10f)]

    public float Cooldown;

    [HideInInspector]
    private int skillCooldown;       //��ų�� ���� ���ð�

    private void Start()
    {
        Char1.SetActive(false);
        Char2.SetActive(false);
        Char3.SetActive(false);
        Char4.SetActive(false);
        Char5.SetActive(false);
        Char6.SetActive(false);

        CheckMesh();
        CheckSkillC();
    }

    public void UseSkill()
    {
        switch(charactorType)
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
            case CharType.����:
                SweetpotatoSkill();
                break;
        }
        Cooldown = skillCooldown;
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

            case CharType.����:
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
            case CharType.����:
                skillCooldown = 9;

                break;
        }
        
        if(Cooldown >= 0)
        {
            Cooldown -= Time.deltaTime;
        }
    }

    #region ĳ���Ϳ��� ��ų�� �߻��ϴ�,
    void GrapeSkill()
    {
        PhotonNetwork.Instantiate("GrapeBomb", transform.position, transform.rotation, 0);
    }

    void CherrySkill()
    {

    }

    void MandarinSkill()
    {

    }

    void OnionSkill()
    {

    }
    void PepperSkill()
    {

    }

    void SweetpotatoSkill()
    {

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
    ����
}
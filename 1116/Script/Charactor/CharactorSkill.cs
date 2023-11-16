using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorSkill : MonoBehaviour , IPunObservable
{
    [Header("��ų")]
    public GameObject ShootPos;
    public GameObject ShootPos1;

    private CharType receieveChar;
    private PhotonView pv;

    [Space(10f)]
    public CharType charactorType;
    [HideInInspector]
    public string charactorTypetoString;
    [Space(10f)]
    [SerializeField] GameObject Char1;
    [SerializeField] GameObject Char2;
    [SerializeField] GameObject Char3;
    [SerializeField] GameObject Char4;
    [SerializeField] GameObject Char5;
    [SerializeField] GameObject Char6;

    [Space(10f)]

    private int skillCooldown;       //��ٿ� �����ϴ� ��
    public float Cooldown;           //����Ǵ� ��ٿ� ��
    [HideInInspector]
    public float cooldownPer;        //��ų ��ٿ� �ۼ�Ʈ��
    

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
        pv = GetComponent<PhotonView>();

    }

    public void UseSkill()
    {
        if (Cooldown <= 0)
        {
            switch (charactorType)
            {
                case CharType.����:
                    GrapeSkill();

                    pv.RPC("GrapeSkill", RpcTarget.Others, null);
                    break;

                case CharType.ü��:
                    CherrySkill();

                    pv.RPC("CherrySkill", RpcTarget.Others, null);

                    break;

                case CharType.��:
                    MandarinSkill();

                    pv.RPC("MandarinSkill", RpcTarget.Others, null);
                    break;

                case CharType.����:
                    OnionSkill();

                    pv.RPC("OnionSkill", RpcTarget.Others, null);
                    break;

                case CharType.����:
                    PepperSkill();

                    pv.RPC("PepperSkill", RpcTarget.Others, null);
                    break;

                case CharType.����:
                    SweetpotatoSkill();

                    pv.RPC("SweetpotatoSkill", RpcTarget.Others, null);
                    break;
            }
            Cooldown = skillCooldown;
        }
    }

    [PunRPC]
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

            case CharType.����:
                Char1.SetActive(false);
                Char2.SetActive(false);
                Char3.SetActive(true);
                Char4.SetActive(false);
                Char5.SetActive(false);
                Char6.SetActive(false);
                break;

            case CharType.��:
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

    [PunRPC]
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
    }


    private void Update()
    {
        CheckSkillC();
        pv.RPC("CheckSkillC", RpcTarget.Others, null);

        CheckMesh();
        pv.RPC("CheckMesh",RpcTarget.All, null);

        if (pv.IsMine)
        {
            CheckMesh();        //�ڽ� ����� ����
            if (Cooldown >= 0)
            {
                Cooldown -= Time.deltaTime;
            }
        }
        else
        {
            charactorType = receieveChar;       //Ÿ�� ĳ���� ����
        }

        cooldownPer = (float)Cooldown / skillCooldown;        //�ۼ�Ʈ ����

        charactorTypetoString = charactorType.ToString();
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(charactorType);
        }
        else
        {
            receieveChar = (CharType)stream.ReceiveNext();
        }
    }


    #region ĳ���Ϳ��� ��ų�� �߻��ϴ�,
    [PunRPC]
    void GrapeSkill()
    {
        Instantiate(GrapeBomb, ShootPos.GetComponent<Transform>().position, ShootPos.GetComponent<Transform>().rotation);
    }

    [PunRPC]
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
    }

    [PunRPC]
    void MandarinSkill()
    {
        Instantiate(MandarinArea, ShootPos.GetComponent<Transform>().position, ShootPos.GetComponent<Transform>().rotation);
    }

    [PunRPC]
    void OnionSkill()       //�� 2���� ��ų�� �÷��̾ ���󰡱� ������ Player�ȿ� ������
    {                       //���� SetActive���� ����
        OnionArea.SetActive(true);
    }
    [PunRPC]
    void PepperSkill()
    {
        PepperArea.SetActive(true);
    }

    [PunRPC]
    void SweetpotatoSkill()
    {
        Quaternion rotation = ShootPos.GetComponent<Transform>().rotation;

        Vector3 rot = ShootPos.GetComponent<Transform>().eulerAngles;
        rot.z += 90;
        Quaternion a = Quaternion.Euler(rot);

        Instantiate(SweetpotatoBind, ShootPos1.GetComponent<Transform>() .position, a);
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
    ����
}
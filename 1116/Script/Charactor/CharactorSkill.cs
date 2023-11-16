using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorSkill : MonoBehaviour , IPunObservable
{
    [Header("스킬")]
    public GameObject ShootPos;
    public GameObject ShootPos1;

    private CharType receieveChar;
    private PhotonView pv;

    [Space(10f)]
    public CharType charactorType;
    [HideInInspector]
    public string charactorTypetoString;
    [Space(10f)]
    [SerializeField] GameObject[] Char;

    [Space(10f)]
    #region Skill
    private int skillCooldown;       //쿨다운 설정하는 값
    public float Cooldown;           //변경되는 쿨다운 값
    [HideInInspector]
    public float cooldownPer;        //스킬 쿨다운 퍼센트값
    

    [SerializeField] GameObject GrapeBomb;

    [SerializeField] GameObject CherrySuriken;
    [SerializeField] float Cherry_Y = 35f;

    [SerializeField] GameObject MandarinArea;
    [SerializeField] GameObject OnionArea;
    [SerializeField] GameObject PepperArea;
    [SerializeField] GameObject SweetpotatoBind;
    #endregion

    private void Start()
    {
        for (int i = 0; i < Char.Length; i++) { Char[i].SetActive(false); }
        pv = GetComponent<PhotonView>();
    }

    public void UseSkill()
    {
        if (Cooldown <= 0)
        {
            switch (charactorType)
            {
                case CharType.포도:
                    GrapeSkill();

                    pv.RPC("GrapeSkill", RpcTarget.Others, null);
                    break;

                case CharType.체리:
                    CherrySkill();

                    pv.RPC("CherrySkill", RpcTarget.Others, null);

                    break;

                case CharType.귤:
                    MandarinSkill();

                    pv.RPC("MandarinSkill", RpcTarget.Others, null);
                    break;

                case CharType.양파:
                    OnionSkill();

                    pv.RPC("OnionSkill", RpcTarget.Others, null);
                    break;

                case CharType.고추:
                    PepperSkill();

                    pv.RPC("PepperSkill", RpcTarget.Others, null);
                    break;

                case CharType.고구마:
                    SweetpotatoSkill();

                    pv.RPC("SweetpotatoSkill", RpcTarget.Others, null);
                    break;
            }
            Cooldown = skillCooldown;
        }
    }

    [PunRPC]
    void CheckMesh()     //캐릭터의 모냥 변경
    {
        switch(charactorType)
        {
            case CharType.포도:
                Char[0].SetActive(true);
                break;

            case CharType.체리:
                Char[1].SetActive(true);
                break;

            case CharType.양파:
                Char[2].SetActive(true);
                break;

            case CharType.귤:
                Char[3].SetActive(true);
                break;

            case CharType.고추:
                Char[4].SetActive(true);
                break;

            case CharType.고구마:
                Char[5].SetActive(true);
                break;
        }
    }

    [PunRPC]
    void CheckSkillC()           //캐릭터의 스킬 쿨다운
    {
        switch (charactorType)      //쿨다운 지정
        {
            case CharType.포도:
                skillCooldown = 7;

                break;
            case CharType.체리:
                skillCooldown = 5;

                break;
            case CharType.귤:
                skillCooldown = 8;

                break;
            case CharType.양파:
                skillCooldown = 12;

                break;
            case CharType.고추:
                skillCooldown = 7;

                break;
            case CharType.고구마:
                skillCooldown = 9;

                break;
        }
    }

    CharType ch;
    private void Update()
    {
        
        if (ch != charactorType)
        {
            for (int i = 0; i < Char.Length; i++) { Char[i].SetActive(false); }
            ch = charactorType;
        } 
        CheckSkillC();
        pv.RPC("CheckSkillC", RpcTarget.Others, null);

        CheckMesh();
        pv.RPC("CheckMesh",RpcTarget.All, null);

        if (pv.IsMine)
        {
            if (Cooldown >= 0)
            {
                Cooldown -= Time.deltaTime;
            }
        }
        else
        {
            charactorType = receieveChar;       //타인 캐릭터 변경
        }

        cooldownPer = (float)Cooldown / skillCooldown;        //퍼센트 설정

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


    #region 캐릭터에서 스킬을 발사하는,
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
    void OnionSkill()       //이 2개의 스킬은 플레이어를 따라가기 때문에 Player안에 존재함
    {                       //따라서 SetActive으로 조절
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
public enum CharType        //캐릭터 종류
{
    포도,
    체리,
    귤,
    양파,
    고추,
    고구마
}
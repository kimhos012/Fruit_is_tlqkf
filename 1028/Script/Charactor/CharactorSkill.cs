using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorSkill : MonoBehaviour
{
    [Header("스킬")]
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
    private int skillCooldown;       //스킬의 시전 대기시간

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

        CheckMesh();
        CheckSkillC();
    }

    public void UseSkill()
    {
        if (Cooldown <= 0)
        {
            switch (charactorType)
            {
                case CharType.포도:
                    GrapeSkill();
                    break;
                case CharType.체리:
                    CherrySkill(ShootPos.GetComponent<Transform>().rotation);

                    break;
                case CharType.귤:
                    MandarinSkill();

                    break;
                case CharType.양파:
                    OnionSkill();

                    break;
                case CharType.고추:
                    PepperSkill();

                    break;
                case CharType.고구마:
                    SweetpotatoSkill();
                    break;
            }
            Cooldown = skillCooldown;
        }
    }


    void CheckMesh()     //캐릭터의 모냥 변경
    {
        switch(charactorType)
        {
            case CharType.포도:
                Char1.SetActive(true);
                Char2.SetActive(false);
                Char3.SetActive(false);
                Char4.SetActive(false);
                Char5.SetActive(false);
                Char6.SetActive(false);
                break;

            case CharType.체리:
                Char1.SetActive(false);
                Char2.SetActive(true);
                Char3.SetActive(false);
                Char4.SetActive(false);
                Char5.SetActive(false);
                Char6.SetActive(false);
                break;

            case CharType.귤:
                Char1.SetActive(false);
                Char2.SetActive(false);
                Char3.SetActive(true);
                Char4.SetActive(false);
                Char5.SetActive(false);
                Char6.SetActive(false);
                break;

            case CharType.양파:
                Char1.SetActive(false);
                Char2.SetActive(false);
                Char3.SetActive(false);
                Char4.SetActive(true);
                Char5.SetActive(false);
                Char6.SetActive(false);
                break;

            case CharType.고추:
                Char1.SetActive(false);
                Char2.SetActive(false);
                Char3.SetActive(false);
                Char4.SetActive(false);
                Char5.SetActive(true);
                Char6.SetActive(false);
                break;

            case CharType.고구마:
                Char1.SetActive(false);
                Char2.SetActive(false);
                Char3.SetActive(false);
                Char4.SetActive(false);
                Char5.SetActive(false);
                Char6.SetActive(true);
                break;
            
        }
    }

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

    private void Update()
    {
        if (Cooldown >= 0)
        {
            Cooldown -= Time.deltaTime;
        }
    }

    #region 캐릭터에서 스킬을 발사하는,
    void GrapeSkill()
    {
        Instantiate(GrapeBomb, ShootPos.GetComponent<Transform>().position, ShootPos.GetComponent<Transform>().rotation);
    }

    void CherrySkill(Quaternion yAxis)
    {
        yAxis.y -= Cherry_Y;
        if (yAxis.y >= 360) { yAxis.y -= 360; }
        if (yAxis.y <= 0) { yAxis.y += 360; }
        Instantiate(CherrySuriken, ShootPos.GetComponent<Transform>().position, yAxis);
        yAxis.y += Cherry_Y;
        if (yAxis.y >= 360) { yAxis.y -= 360; }
        if (yAxis.y <= 0) { yAxis.y += 360; }
        Instantiate(CherrySuriken, ShootPos.GetComponent<Transform>().position, yAxis);
        yAxis.y += Cherry_Y;
        if (yAxis.y >= 360) { yAxis.y -= 360; }
        if (yAxis.y <= 0) { yAxis.y += 360; }
        Instantiate(CherrySuriken, ShootPos.GetComponent<Transform>().position, yAxis);
    }

    void MandarinSkill()
    {
        Instantiate(MandarinArea, ShootPos.GetComponent<Transform>().position, ShootPos.GetComponent<Transform>().rotation);
    }

    void OnionSkill()       //이 2개의 스킬은 플레이어를 따라가기 때문에 Player안에 존재함
    {                       //따라서 SetActive으로 조절
        OnionArea.SetActive(true);
    }
    void PepperSkill()
    {
        PepperArea.SetActive(true);
    }

    void SweetpotatoSkill()
    {
        PhotonNetwork.Instantiate("", ShootPos.GetComponent<Transform>().position, ShootPos.GetComponent<Transform>().rotation);        //None
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
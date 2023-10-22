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
    private int skillCooldown;       //스킬의 시전 대기시간

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
            case CharType.포도:
                GrapeSkill();
                break;
            case CharType.체리:
                CherrySkill();

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
        
        if(Cooldown >= 0)
        {
            Cooldown -= Time.deltaTime;
        }
    }

    #region 캐릭터에서 스킬을 발사하는,
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
public enum CharType        //캐릭터 종류
{
    포도,
    체리,
    귤,
    양파,
    고추,
    고구마
}
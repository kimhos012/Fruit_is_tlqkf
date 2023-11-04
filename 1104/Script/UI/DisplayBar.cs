using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayBar : MonoBehaviour
{
    public Image[] PlayerHp;
    public Image[] PlayerCool;
    public Text[] PlayerName;

    GameObject[] Players;
    float[] playerHp;
    float[] playerCool;
    string[] playerName;

    string[] editName;

    void LateUpdate()        //게임 시작 시, 이 UI는 활성화됨
    {
        Players = GameObject.FindGameObjectsWithTag("Player");
        
        playerHp = new float[Players.Length];
        playerCool = new float[Players.Length];
        playerName = new string[Players.Length];

        for (int p = 0; p < Players.Length; p++)        //GetPlayerHealth
        {
            playerHp[p] = Players[p].GetComponent<Damage>().hpPers;
            playerCool[p] = Players[p].GetComponent<CharactorSkill>().cooldownPer;
            playerName[p] = Players[p].GetComponent<PhotonView>().Controller.NickName.ToString();       //테스트환경에서 에러를 띄우지 않기 위한 똥꼬쇼
        }
        return;
    }
    void FixedUpdate()
    {
        SetHpBar();
    }

    void SetHpBar()
    {
        for(int i =0; i < Players.Length; i++)      //체력값설정
        {
            PlayerHp[i].fillAmount = playerHp[i];

            PlayerCool[i].fillAmount = playerCool[i];

            PlayerName[i].text = playerName[i];         //똥꼬쇼22

        }


    }
}

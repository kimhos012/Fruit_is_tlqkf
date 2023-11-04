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

    void LateUpdate()        //���� ���� ��, �� UI�� Ȱ��ȭ��
    {
        Players = GameObject.FindGameObjectsWithTag("Player");
        
        playerHp = new float[Players.Length];
        playerCool = new float[Players.Length];
        playerName = new string[Players.Length];

        for (int p = 0; p < Players.Length; p++)        //GetPlayerHealth
        {
            playerHp[p] = Players[p].GetComponent<Damage>().hpPers;
            playerCool[p] = Players[p].GetComponent<CharactorSkill>().cooldownPer;
            playerName[p] = Players[p].GetComponent<PhotonView>().Controller.NickName.ToString();       //�׽�Ʈȯ�濡�� ������ ����� �ʱ� ���� �˲���
        }
        return;
    }
    void FixedUpdate()
    {
        SetHpBar();
    }

    void SetHpBar()
    {
        for(int i =0; i < Players.Length; i++)      //ü�°�����
        {
            PlayerHp[i].fillAmount = playerHp[i];

            PlayerCool[i].fillAmount = playerCool[i];

            PlayerName[i].text = playerName[i];         //�˲���22

        }


    }
}

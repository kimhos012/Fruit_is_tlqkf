using System;
using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Timer : MonoBehaviourPunCallbacks
{
    int time = 0;

    public Text timerText;

    public PhotonView PV;


    private void Start()
    {
        StartTimer();
    }


    void StartTimer()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            time = 180;
            StartCoroutine(TimerCoroution());
        }
    }

    IEnumerator TimerCoroution()
    {
        if (time > 0)
        {
            time -= 1;
        }
        else
        {
            Debug.Log("Ÿ�̸� ����");

            yield break;
        }

        PV.RPC("ShowTimer", RpcTarget.All, time); //1�� ���� �� ��ο��� ����

        yield return new WaitForSeconds(1);
        StartCoroutine(TimerCoroution());
    }

    void RoundEnd()
    {

    }


    [PunRPC]
    void ShowTimer(int number)
    {
        timerText.text = number.ToString(); //Ÿ�̸� ����
    }
}
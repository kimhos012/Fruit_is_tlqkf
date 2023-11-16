using System;
using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Timer : MonoBehaviourPunCallbacks
{

    public int maxTime;
    float curTime = 0;

    public Image timerimage;

    public PhotonView PV;


    private void Start()
    {
        StartTimer();
    }


    void StartTimer()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            curTime = maxTime;
            StartCoroutine(TimerCoroution());
        }
    }

    IEnumerator TimerCoroution()
    {
        if (curTime > 0)
        {
            curTime -= 0.1f;
        }
        else
        {
            Debug.Log("타이머 종료");

            yield break;
        }

        PV.RPC("ShowTimer", RpcTarget.All, curTime); //1초 마다 방 모두에게 전달

        yield return new WaitForSeconds(0.1f);
        StartCoroutine(TimerCoroution());
    }

    void RoundEnd()
    {

    }


    [PunRPC]
    void ShowTimer(int number)
    {
        timerimage.fillAmount = (float)curTime / maxTime;
    }
}
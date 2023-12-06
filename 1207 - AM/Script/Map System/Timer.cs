using System;
using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Timer : MonoBehaviourPunCallbacks
{

    public float maxTime;
    [HideInInspector]
    public float curTime = 999;

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
            PV.RPC("RoundEnd", RpcTarget.AllBuffered);
            yield break;
        }

        PV.RPC("ShowTimer", RpcTarget.All, curTime); //Send
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(TimerCoroution());
    }
    [PunRPC]
    public void RoundEnd()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().RoundEnd();

    }

    [PunRPC]
    void ShowTimer(float time)
    {
        timerimage.fillAmount = (float)time / maxTime;
    }
}
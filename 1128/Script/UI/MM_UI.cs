using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MM_UI : MonoBehaviour
{
    public GameObject MMdisplay;
    public Text playerCount;
    public Text UpperText;
    public GameObject Timer;
    public int Time;

    RandomSelect RS;
    [Space(10f)]

    //Private
    PhotonView pv;
    int curTime;
    bool Find;

    private void Start()
    {
        RS = GetComponent<RandomSelect>();
        pv = GetComponent<PhotonView>();
        StartCoroutine(ShowPlayerCount());
        Timer.SetActive(false);
    }
    public void SlideUI(int value)
    {
        StartCoroutine(Slide(value));
    }
    IEnumerator Slide(int value)
    {
        for (int i = 0; i < 20; i++)
        {
            yield return new WaitForSeconds(0.02f);
            MMdisplay.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, value);
        }
    }

    private void FixedUpdate()
    {
        if (!PhotonNetwork.InRoom) { return; }
        int count = PhotonNetwork.CurrentRoom.PlayerCount;

        playerCount.text = ("Players : " + count.ToString() + "/4");

        if (count > 1)
        {
            Timer.SetActive(true);
            CountDown();


            if (count == 4 && curTime > 10)
            {
                curTime = 10;
            }
        }
        else
        {
            curTime = Time;
            Find = false;
            Timer.SetActive(false);
        }
    }
    void CountDown()
    {
        if (PhotonNetwork.IsMasterClient && !Find)
        {

            StartCoroutine(Clock());
            Find = true;

        }
    }
    IEnumerator Clock()
    {

        if (curTime > 0)
        {
            curTime--;
        }
        else
        {
            Debug.Log("타이머 종료");
            RS.Pick();
            yield break;
        }

        pv.RPC("WaitTime", RpcTarget.All, curTime); //Send

        yield return new WaitForSeconds(1f);
        StartCoroutine(Clock());

    }
    [PunRPC]
    void WaitTime(int time)
    {
        Timer.transform.GetChild(0).GetComponent<Text>().text = (time + " 초 뒤에 게임이 시작됩니다.");
    }
    IEnumerator ShowPlayerCount()
    {
        var a = new WaitForSeconds(0.5f);
        while (true)
        {
            UpperText.text = ("Matchmaking");
            yield return a;
            UpperText.text = ("Matchmaking.");
            yield return a;
            UpperText.text = ("Matchmaking..");
            yield return a;
            UpperText.text = ("Matchmaking...");
            yield return a;
        }
    }


}

using Newtonsoft.Json.Linq;
using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int time2Start;              //카운트다운 후 시작
    public GameObject resultScreen;     //중간결과창
    public bool isEnd = false;          //게임 종료
    public int survivors;
    PhotonView pv;

    public Text StartTimer;

    GameObject[] getPlayer;
    public bool[] isDead;
    public Transform[] points;

    private void Start()
    {
        resultScreen.SetActive(false);
        pv = GetComponent<PhotonView>();
        getPlayer = GameObject.FindGameObjectsWithTag("Player");
        points = GameObject.Find("Spawnpoint").GetComponentsInChildren<Transform>();
        isDead = new bool[PhotonNetwork.CurrentRoom.PlayerCount];
        StartTimer.text = "";
        Invoke("waitSec", 0.5f);
    }
    void waitSec() => pv.RPC("StartGame", RpcTarget.All);

    [PunRPC]
    public void StartGame()
    {
        getPlayer = GameObject.FindGameObjectsWithTag("Player");
        isBind(true);
        foreach (GameObject player in getPlayer)
        {
            PlayerData plData = player.GetComponent<PlayerData>();
            plData.Hp = 120;
            plData.curCool = 0;

            int plnum = player.GetComponent<PlayerData>().playerNum;       //넘버를 얻어서
            Transform setPosition = points[plnum];  //좌표이동
            player.transform.position = setPosition.position;
        }
        isBind(false);
        StartCoroutine(clock(time2Start));
    }
    IEnumerator clock(int time)         //시작 전 맵
    {
        int curTime = time;
        while (curTime > 0)
        {
            yield return new WaitForSeconds(1);
            curTime--;
            StartTimer.text = curTime.ToString();
        }
        //게임 시작
        Debug.Log("게임 시작");
        isBind(true);
        StartTimer.text = "";
        if(PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(Tanos());
        }
        
        yield return null;
    }
    public void isBind(bool bb)
    {

        for (int i = 0; i < getPlayer.Length; i++)
        {
            getPlayer[i].GetComponent<Controller>().enabled = bb;
            getPlayer[i].GetComponent<PlayerData>().diee = false;
        }
    }
    IEnumerator Tanos()             //사람이 1명이거나 시간이 끝나면 Result 출력
    {
        while (!isEnd)
        {
            survivors = 0;
            Debug.Log("DetectPlayerLeft");
            for (int i = 0; i < getPlayer.Length; i++)
            {
                PlayerData playerData = getPlayer[i].GetComponent<PlayerData>();
                isDead[i] = playerData.diee;
                if (playerData == null)
                {
                    Debug.Log("이거왜Null임");      //남은사람을 감지하는 친구입니다.
                    yield break;
                }

                if (!isDead[i])
                {
                    survivors++;
                }
            }
            Debug.Log(survivors);

            if (survivors == 1)         //한명남으면
            {
                //RoundEnd();
                //GivePoint(1);

                pv.RPC("RoundEnd", RpcTarget.AllBuffered);
                Debug.Log("Only one survivor left!");
                yield break;
            }
            //GameEndDectecter
            yield return new WaitForSeconds(0.05f);
        }
    }
    [PunRPC]
    public void RoundEnd()
    {
        Debug.Log("GameEnd");
        for (int i = 0; i < getPlayer.Length; i++)
        {
            if (!isDead[i])
            {
                getPlayer[i].GetComponent<PlayerData>().gameScore++;
            }
        }
        isEnd = true;
        resultScreen.SetActive(true);
        StartCoroutine(pickOther());
    }

    IEnumerator pickOther()
    {
        isBind(false);
        yield return new WaitForSeconds(5);
        if (LocalManager.MaxRound < 1)
        {
            PhotonNetwork.LoadLevel("Final");
        }
        else
        {
            LocalManager.MaxRound--;
            resultScreen.SetActive(false);
            GameObject inGame = GameObject.Find("UI");
            inGame.GetComponent<UISys>().RandomToggle(true);
            inGame.GetComponent<RandomSelect>().Pick();
        }
    }
}

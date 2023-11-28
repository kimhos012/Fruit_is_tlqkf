using Photon.Pun;
using System.Collections;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public int time2Start;
    public GameObject resultScreen;
    public GameObject timer;
    Timer time;
    int curTime;
    PhotonView pv;


    private void Start()
    {
        resultScreen.SetActive(false);
        pv = GetComponent<PhotonView>();
        curTime = time2Start;

        Transform[] points = GameObject.Find("Spawnpoint").GetComponentsInChildren<Transform>();
        //StartCoroutine(Tanos());
        //Timer();
        pv.RPC("Timer", RpcTarget.AllBuffered);
    }
    IEnumerator Tanos()
    {
        bool isEnd = false;
        while (!isEnd)
        {
            time = timer.GetComponent<Timer>();

            int survivors = 0;
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
            {
                PlayerData playerData = player.GetComponent<PlayerData>();

                if (playerData.diee == false)
                {
                    survivors++;
                }
                else
                {
                    survivors--;
                    resultScreen.SetActive(false);
                }
            }

            if (time.curTime < 1)
            {
                GameObject[] p = GameObject.FindGameObjectsWithTag("Player");
                foreach (GameObject player in p)
                {
                    PlayerData playerData = player.GetComponent<PlayerData>();
                    if (!playerData.diee)
                    {
                        player.GetComponent<PlayerData>().gameScore++;
                    }
                }
                RoundEnd();
                pv.RPC("RoundEnd", RpcTarget.Others);
            }

            if (survivors <= 1)
            {
                GameObject[] p = GameObject.FindGameObjectsWithTag("Player");
                foreach (GameObject player in p)
                {
                    PlayerData playerData = player.GetComponent<PlayerData>();
                    if (!playerData.diee)
                    {
                        player.GetComponent<PlayerData>().gameScore++;
                    }
                }

                //RoundEnd();
                pv.RPC("RoundEnd", RpcTarget.All);
                Debug.Log("Only one survivor left!");
                yield break;
            }
            //GameEndDectecter
            yield return new WaitForSeconds(0.05f);
        }
        yield return null;
    }
    [PunRPC]
    public void RoundEnd()
    {
        resultScreen.SetActive(true);

    }

    [PunRPC]
    void Timer()
    {
        StartCoroutine(clock());
        StartCoroutine(Tanos());
    }
    IEnumerator clock()
    {
        while (curTime > 0)
        {
            yield return new WaitForSeconds(1);
            curTime--;
        }
        //게임 시작
        Debug.Log("게임 시작");



        
        yield return null;
    }
}

using Photon.Pun;
using System.Collections;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public int time2Start;
    public GameObject resultScreen;
    public GameObject timer;
    public bool isEnd = false;


    Timer time;
    int curTime;
    PhotonView pv;
    GameObject[] p;

    public Transform[] points;

    private void Start()
    {
        resultScreen.SetActive(false);
        pv = GetComponent<PhotonView>();
        curTime = time2Start;
        time = timer.GetComponent<Timer>();

        points = GameObject.Find("Spawnpoint").GetComponentsInChildren<Transform>();    
        //StartCoroutine(Tanos());
        //Timer();
        pv.RPC("StartGame", RpcTarget.AllBuffered);
    }
    IEnumerator Tanos()
    {
        p = GameObject.FindGameObjectsWithTag("Player");
        while (!isEnd)
        {
            Debug.Log("Tanossss");
            //time = timer.GetComponent<Timer>();

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
                }
            }
            //Det
            if (time.curTime < 1)
            {
                foreach (GameObject player in p)
                {
                    PlayerData playerData = player.GetComponent<PlayerData>();
                    if (!playerData.diee)
                    {
                        player.GetComponent<PlayerData>().gameScore++;
                    }
                }
                //RoundEnd();
                pv.RPC("RoundEnd", RpcTarget.AllBuffered);
                Debug.Log("TimeOut");
            }

            if (survivors <= 1)
            {
                foreach (GameObject player in p)
                {
                    PlayerData playerData = player.GetComponent<PlayerData>();
                    if (!playerData.diee)
                    {
                        player.GetComponent<PlayerData>().gameScore++;
                    }
                }

                //RoundEnd();
                pv.RPC("RoundEnd", RpcTarget.AllBuffered);
                Debug.Log("Only one survivor left!");
            }
            //GameEndDectecter
            yield return new WaitForSeconds(0.05f);
        }
        yield return null;
    }
    [PunRPC]
    public void RoundEnd()
    {
        isEnd = true;
        resultScreen.SetActive(true);
        //StartCoroutine(Tanos());
    }

    [PunRPC]
    public void StartGame()
    {
        StartCoroutine(clock());
        StartCoroutine(Tanos());
    }
    IEnumerator clock()         //시작 전 맵
    {

        while (curTime > 0)
        {
            yield return new WaitForSeconds(1);
            curTime--;
        }
        //게임 시작
        Debug.Log("게임 시작");


        //StartCoroutine(Tanos());
        yield return null;
    }
}

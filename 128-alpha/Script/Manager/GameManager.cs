using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int time2Start;              //ī��Ʈ�ٿ� �� ����
    public GameObject resultScreen;     //�߰����â
    public bool isEnd = false;          //���� ����
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
        //Invoke("waitSec", 0.5f);
        waitSec();
    }
    void waitSec() => pv.RPC("StartGame", RpcTarget.All);

    [PunRPC]
    public void StartGame()
    {
        getPlayer = GameObject.FindGameObjectsWithTag("Player");
        isBind(true);
        foreach (GameObject player in getPlayer)                    //��ǥ�̵�
        {
            PlayerData plData = player.GetComponent<PlayerData>();
            plData.Hp = 120;
            plData.curCool = 0;
            plData.diee = false;

            Controller plCon = player.GetComponent<Controller>();
            plCon.speed = 1f;

            int plnum = player.GetComponent<PlayerData>().playerNum;       //�ѹ��� ��
            Transform setPosition = points[plnum];  
            player.transform.position = setPosition.position;
        }
        isBind(false);
        StartCoroutine(clock(time2Start));
    }
    IEnumerator clock(int time)         //���� �� ��
    {
        int curTime = time;
        while (curTime > 0)
        {
            yield return new WaitForSeconds(1);
            curTime--;
            StartTimer.text = curTime.ToString();
        }
        //���� ����
        Debug.Log("���� ����");
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
    IEnumerator Tanos()             //����� 1���̰ų� �ð��� ������ Result ���
    {
        //Debug.Log("DetectPlayerLeft");
        while (!isEnd)
        {
            survivors = 0;
            for (int i = 0; i < getPlayer.Length; i++)
            {
                PlayerData playerData = getPlayer[i].GetComponent<PlayerData>();
                isDead[i] = playerData.diee;
                if (playerData == null)
                {
                    Debug.Log("�̰ſ�Null��");      //��������� �����ϴ� ģ���Դϴ�.
                    yield break;
                }

                if (!isDead[i])
                {
                    survivors++;
                }
            }
            Debug.Log(survivors);

            if (survivors == 1)         //�Ѹ�����
            {

                pv.RPC("RoundEnd", RpcTarget.AllBuffered);
                Debug.Log("Only one survivor left!");
                yield break;
            }
            //GameEndDectecter
            yield return new WaitForSeconds(0.05f);
        }
    }
    public void TImerEndTrigger() => pv.RPC("RoundEnd", RpcTarget.AllBuffered);


    [PunRPC]
    public void RoundEnd()
    {
        Debug.Log("GameEnd");

        foreach (GameObject player in getPlayer)             //�����ֱ�
        {
            PlayerData plData = player.GetComponent<PlayerData>();
            if(!plData.diee)
            {
                plData.gameScore++;
            }
        }
        isEnd = true;

        pv.RPC("BoolResult", RpcTarget.All, true);

        if (PhotonNetwork.IsMasterClient)
            StartCoroutine(pickOther());
    }
    [PunRPC]
    public void BoolResult(bool bo) => resultScreen.SetActive(bo);

    IEnumerator pickOther()
    {
        isBind(false);
        yield return new WaitForSeconds(5);
        pv.RPC("BoolResult", RpcTarget.All, false);     //
        if (LocalManager.MaxRound == 1)
        {
            isBind(true);
            
            PhotonNetwork.LoadLevel("Final");
        }
        else
        {
            LocalManager.MaxRound--;
            GameObject inGame = GameObject.Find("UI");
            inGame.GetComponent<UISys>().RandomToggle(true);
            inGame.GetComponent<RandomSelect>().Pick();
        }
    }
}

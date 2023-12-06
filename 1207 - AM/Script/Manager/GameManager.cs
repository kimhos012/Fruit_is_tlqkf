using Newtonsoft.Json.Linq;
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

            int plnum = player.GetComponent<PlayerData>().playerNum;       //�ѹ��� ��
            Transform setPosition = points[plnum];  //��ǥ�̵�
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

using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;
using System.Collections;

public class ShowScoreBorad : MonoBehaviour
{
    public GameObject[] Player;
    public GameObject[] Array2Score;

    public int maxPlayers = 4; // �ִ� �÷��̾� ��

    public Text[] scoreText;
    public Text[] nameText;
    public bool isFirst = false;


    PhotonView pv;
    private void Start()
    {
        pv = GetComponent<PhotonView>();
        if (isFirst)
        {
            pv.RPC("SortPlayer", RpcTarget.AllBuffered);
            pv.RPC("ShowPanel", RpcTarget.AllBuffered);
        }
        else
        {
            StartCoroutine(UpdateScore());
        }
    }
    IEnumerator UpdateScore()
    {
        while(true)
        {
            pv.RPC("SortPlayer", RpcTarget.AllBuffered);
            pv.RPC("ShowPanel", RpcTarget.AllBuffered);
            yield return new WaitForFixedUpdate();
        }
        
    }

    [PunRPC]
    public void ShowPanel()
    {
        for (int i = 0; i < Player.Length; i++)
        {
            scoreText[i].text = Array2Score[i].GetComponent<PlayerData>().gameScore.ToString();
            nameText[i].text = Array2Score[i].GetComponent<PhotonView>().Controller.NickName.ToString();
        }
    }
    [PunRPC]
    void SortPlayer()
    {
        Player = GameObject.FindGameObjectsWithTag("Player");

        // PlayerValue ��ũ��Ʈ�� ���� Player GameObject���� List�� ����
        List<GameObject> playerList = new List<GameObject>();
        foreach (GameObject playerObject in Player)
        {
            if (playerObject.GetComponent<PlayerData>() != null)
            {
                playerList.Add(playerObject);
            }
        }

        // playerList�� gameScore �� �������� �������� ����
        playerList.Sort((a, b) =>
        {
            PlayerData playerValueA = a.GetComponent<PlayerData>();
            PlayerData playerValueB = b.GetComponent<PlayerData>();
            if (playerValueA != null && playerValueB != null)
            {
                return playerValueB.gameScore.CompareTo(playerValueA.gameScore);
            }
            else
            {
                return 0;
            }
        });

        Array2Score = new GameObject[playerList.Count];
        // ���ĵ� playerList�� Array2Score�� ����
        for (int i = 0; i < playerList.Count; i++)
        {
            Array2Score[i] = playerList[i];
        }
        ShowPanel();
    }

    public void GoToMenu()          //E
    {
        PhotonNetwork.LeaveRoom();
        GameObject[] p = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in p)
        {
            Destroy(player);
        }
        SceneManager.LoadScene("MainMenu");
    }
}
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

    public int maxPlayers = 4; // 최대 플레이어 수

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

        // PlayerValue 스크립트를 가진 Player GameObject들을 List로 저장
        List<GameObject> playerList = new List<GameObject>();
        foreach (GameObject playerObject in Player)
        {
            if (playerObject.GetComponent<PlayerData>() != null)
            {
                playerList.Add(playerObject);
            }
        }

        // playerList를 gameScore 값 기준으로 내림차순 정렬
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
        // 정렬된 playerList를 Array2Score에 복사
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
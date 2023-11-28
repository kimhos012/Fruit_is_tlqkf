using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowScoreBorad : MonoBehaviour
{
    public GameObject[] Player;
    public static GameObject[] Array2Score;

    public int maxPlayers = 4; // 최대 플레이어 수

    public Text[] scoreText;
    public Text[] nameText;

    private void Start()
    {
        SortPlayer();
    }

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
    }

    private void Update()
    {
        SortPlayer();

        //Set
        for (int i = 0; i < Player.Length; i++)
        {

            if (Array2Score[i] != null)
            {
                nameText[i] = Array2Score[i].transform.GetChild(0).gameObject.GetComponent<Text>();
                scoreText[i] = Array2Score[i].transform.GetChild(1).gameObject.GetComponent<Text>();

                scoreText[i].text = Array2Score[i].GetComponent<PlayerData>().gameScore.ToString();
                nameText[i].text = Array2Score[i].GetComponent<PhotonView>().Controller.NickName;
            }

        }
    }
}
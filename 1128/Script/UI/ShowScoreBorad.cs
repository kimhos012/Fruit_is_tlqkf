using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowScoreBorad : MonoBehaviour
{
    public GameObject[] Player;
    public static GameObject[] Array2Score;

    public int maxPlayers = 4; // �ִ� �÷��̾� ��

    public Text[] scoreText;
    public Text[] nameText;

    private void Start()
    {
        SortPlayer();
    }

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
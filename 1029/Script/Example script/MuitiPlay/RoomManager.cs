using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class RoomManager : MonoBehaviourPunCallbacks
{

    public InputField FieldInput;
    private string RoomNum = null;


    private string version = "0";

    private void Awake()
    {
        //������ Ŭ���̾�Ʈ(���� ������ ����)�� �� �ڵ� ����ȭ �ɼ�
        PhotonNetwork.AutomaticallySyncScene = true;

        //���� ���� ���� (���� ������ �������� ������)
        PhotonNetwork.GameVersion = version;


        //���� �������� �������� �ʴ� ���� Ƚ�� (�⺻ 30ȸ)
        Debug.Log(PhotonNetwork.SendRate);

        //���� ���� ����
        PhotonNetwork.ConnectUsingSettings();
    }

    // Start is called before the first frame update
    void Start()
    {
        FieldInput.onEndEdit.AddListener(ValueChanged);
    }

    void ValueChanged(string text)
    {
        RoomNum = text;
        Debug.Log(text+"�� ����");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room Created");
    }

    public void FindMatch()
    {
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("Finding RandomRoom..");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("404 not found");
    }

    //�� ����
    public void MakinRoom()
    {
        if (RoomNum != null)     //Detect NameLength
        {
            Debug.Log("Creating Room " + RoomNum);

            RoomOptions ro = new RoomOptions();
            ro.MaxPlayers = 4;
            ro.IsOpen = true;
            ro.IsVisible = true;

            PhotonNetwork.CreateRoom(RoomNum, ro);
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"PhotonNetwork.InRoom = {PhotonNetwork.InRoom}");
        Debug.Log($"Player Count = {PhotonNetwork.CurrentRoom.PlayerCount}");

        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log($"{player.Value.NickName} , {player.Value.ActorNumber}");
        }

        // SaveArraySpawning
        Transform[] points = GameObject.Find("SpawnpointList").GetComponentsInChildren<Transform>();
        int idx = Random.Range(1, points.Length);

        // Create
        PhotonNetwork.Instantiate("Player", points[idx].position, points[idx].rotation, 0);
    }
}

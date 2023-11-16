using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MultiplayerSystem : MonoBehaviourPunCallbacks //PUN�� �پ��� �ݹ� �Լ��� �������̵��ؼ� �ۼ�
{
    //���� ����
    private readonly string version = "0.1";
    //���� �г���
    private string userId = "JackJack_E";

    // �������� �Է��� TextMeshPro Input Field
    public TMP_InputField userIF;

    public static int assignedNumber;
    public GameObject StartPlayer;
    private static readonly int MaxPlayers = 4;

    public UISys uiScript;

    void Awake()
    {
        
        //������ Ŭ���̾�Ʈ(���� ������ ����)�� �� �ڵ� ����ȭ �ɼ�
        PhotonNetwork.AutomaticallySyncScene = true;

        //���� ���� ���� (���� ������ �������� ������)
        PhotonNetwork.GameVersion = version;

        //���� ������ �г��� ����
        PhotonNetwork.NickName = userId;

        //���� �������� �������� �ʴ� ���� Ƚ�� (�⺻ 30ȸ)
        Debug.Log(PhotonNetwork.SendRate);

        //���� ���� ����
        PhotonNetwork.ConnectUsingSettings();
    }

    void Start()
    {

        //�̸�â�� ǥ��
        uiScript.NameToggle(true);
        uiScript.MainToggle(false);
        uiScript.OptionToggle(false);
        uiScript.RandomToggle(false);
        uiScript.CharToggle(false);

        // ����� �������� �ε�
        userId = PlayerPrefs.GetString("USER_ID", $"USER_{Random.Range(1, 100):00}");
        userIF.text = userId;

        // ���� ������ �г��� ���
        PhotonNetwork.NickName = userId;
    }

    #region PhotonSys
    // �������� �����ϴ� ����
    public void SetUserId()
    {
        if (string.IsNullOrEmpty(userIF.text))
        {
            userId = $"USER_{Random.Range(1, 100):00}";
        }
        else
        {
            if (userIF.text.Length <= 10)
            {
                userId = userIF.text;


                // ������ ����
                PlayerPrefs.SetString("USER_ID", userId);

                // ���� ������ �г��� ���
                PhotonNetwork.NickName = userId;

                uiScript.NameToggle(false);
                uiScript.MainToggle(true);
            }
            else
            {
                Debug.Log("�̸��� �ʹ� ��!");
                userId = $"USER_{Random.Range(1, 21):00}";
            }

        }

    }

    //���� ������ ���� �� ���� ���� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}"); //�ڵ� ������ �ƴϹǷ� false
        //PhotonNetwork.JoinLobby(); //�κ� ���� ��� OnJoinedLobby ȣ��
    }

    //�κ� ���� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnJoinedLobby()
    {
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
    }

    //������ �� ������ �������� ��� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        OnMakeRoomClick();
        
    }

    public override void OnCreateRoomFailed(short returnCode, string message) => OnMakeRoomClick();

    public void OnMakeRoomClick()
    {
        // ������ ����
        SetUserId();

        // ���� �Ӽ� ����
        RoomOptions ro = new RoomOptions
        {
            MaxPlayers = 4,     // �뿡 ������ �� �ִ� �ִ� ������ ��
            IsOpen = true,       // ���� ���� ����
            IsVisible = true    // �κ񿡼� �� ��Ͽ� �����ų ����
        };

        // �� ����
        PhotonNetwork.CreateRoom((Random.Range(0, 3000)) + (Random.Range(0, 3000) + "a"), ro);
    }

    //�� ���� �Ϸ� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnCreatedRoom()
    {
        Create(assignedNumber);
        Debug.Log($"Room Name = {PhotonNetwork.CurrentRoom.Name}");
    }

    //�뿡 ������ �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnJoinedRoom()
    {

        Debug.Log($"PhotonNetwork.InRoom = {PhotonNetwork.InRoom}");
        Debug.Log($"Player Count = {PhotonNetwork.CurrentRoom.PlayerCount}");

        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log($"{player.Value.NickName} , {player.Value.ActorNumber}");
        }

        //AssignPlayerNumber(PhotonNetwork.LocalPlayer);
        PhotonView pv = PhotonView.Get(this);
        pv.RPC("AssignPlayerNumber", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.ActorNumber);
        //�� ���� �� �÷��̾� ����
        if (!PhotonNetwork.IsMasterClient)
        {
            Create(assignedNumber);
            Debug.Log("JoinedRoom���� ����");
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //AssignPlayerNumber(Player);
        PhotonView pv = PhotonView.Get(this);
        pv.RPC("AssignPlayerNumber", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.ActorNumber);
        if (PhotonNetwork.IsMasterClient)
        {
            //Create();
            Debug.Log("EnderRoom���� ����");
        }
    }

    #endregion

    [PunRPC]
    private void AssignPlayerNumber(int playerActorNumber)
    {
        Player player = PhotonNetwork.CurrentRoom.GetPlayer(playerActorNumber);
        if (player != null)
        {
            // ���� �뿡 �ִ� �÷��̾� ��
            int currentPlayerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            if (currentPlayerCount <= MaxPlayers)
            {
                assignedNumber = currentPlayerCount; //��ȣ �ο�
                player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "PlayerNumber", assignedNumber } });

                Debug.Log($"{player.NickName} assigned to Player Number {assignedNumber}");
            }
            else
            {
                Debug.LogWarning("Room is already full.");
            }
        }
        else
        {
            Debug.LogError("Player not found");
        }

    }
    void Create(int assignedNumber)
    {
        if(StartPlayer != null) Destroy(StartPlayer);

        // ���� ��ġ ������ �迭�� ����
        Transform[] points = GameObject.Find("Spawnpoint").GetComponentsInChildren<Transform>();
        int idx = UnityEngine.Random.Range(1, points.Length);

        // ��Ʈ��ũ�� ĳ���� ����
        GameObject pLaYeR = PhotonNetwork.Instantiate("Player", points[idx].position, points[idx].rotation, 0);
        pLaYeR.GetComponent<PlayerValue>().number = assignedNumber;
        Debug.Log("���� �÷��̾�");

    }

    #region UI_EFFECT
    public void SetNickName()
    {
        SetUserId();
    }


    public void MatchMacking()
    {
        PhotonNetwork.JoinRandomRoom();

    }

    //---------Will Be Removed-------------//
    public void StartButton()
    {

        //Select Level


        PhotonNetwork.LoadLevel("BooAuk");
    }
    #endregion


}

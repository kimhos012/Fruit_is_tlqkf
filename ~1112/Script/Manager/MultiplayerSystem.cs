using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerSystem : MonoBehaviourPunCallbacks //PUN�� �پ��� �ݹ� �Լ��� �������̵��ؼ� �ۼ�
{
    //���� ����
    private readonly string version = "0.1";
    //���� �г���
    private string userId = "JackJack_E";

    // �������� �Է��� TextMeshPro Input Field
    public TMP_InputField userIF;
    public GameObject namePanel;
    public Image toggleColor;

    private Toggle m_Toggle;
    public static int assignedNumber;
    private static readonly int MaxPlayers = 4;
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

        //�̸� ���ϴ� ȭ�� ����
        namePanel.SetActive(true);
    }

    void Start()
    {
        // ����� �������� �ε�
        userId = PlayerPrefs.GetString("USER_ID", $"USER_{Random.Range(1, 100):00}");
        userIF.text = userId;

        // ���� ������ �г��� ���
        PhotonNetwork.NickName = userId;

        //UI������ �̵�
        toggleColor = GameObject.Find("��ġ����ŷ_��ư").transform.GetChild(0).GetComponent<Image>();

        m_Toggle = GameObject.Find("��ġ����ŷ_��ư").GetComponent<Toggle>();

        m_Toggle.onValueChanged.AddListener(delegate
        {
            ToggleValueChanged(m_Toggle);
        }
        );
        toggleColor.color = new Color(0, 1, 0, 1);
        //-------------
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

                namePanel.SetActive(false);
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
    public override void OnJoinRandomFailed(short returnCode, string message) => OnMakeRoomClick();
    //�� ���� �Ϸ� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnCreatedRoom() => Debug.Log($"Room Name = {PhotonNetwork.CurrentRoom.Name}");
    //�뿡 ������ �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnJoinedRoom()
    {
        Debug.Log($"PhotonNetwork.InRoom = {PhotonNetwork.InRoom}");
        Debug.Log($"Player Count = {PhotonNetwork.CurrentRoom.PlayerCount}");

        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log($"{player.Value.NickName} , {player.Value.ActorNumber}");
        }

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("FindPlayers");
        }
        AssignPlayerNumber(PhotonNetwork.LocalPlayer);
        //�� ���� �� �÷��̾� ����
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AssignPlayerNumber(newPlayer);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        OnMakeRoomClick();
    }

    //----------------------
    void ToggleValueChanged(Toggle m_Toggle)
    {
        if (m_Toggle.isOn)
        {
            toggleColor.color = new Color(1, 0, 0, 1);
            Debug.Log("True");
            MatchMacking();
        }
        else
        {
            toggleColor.color = new Color(0, 1, 0, 1);
            PhotonNetwork.LeaveRoom();
            Debug.Log("False");
        }
    }
    //-------------------

    [PunRPC]
    private void AssignPlayerNumber(Player player)
    {
        // ���� �뿡 �ִ� �÷��̾� ��
        int currentPlayerCount = PhotonNetwork.CurrentRoom.PlayerCount;

        // �ִ� �÷��̾� ���� �ʰ����� �ʴ� ��쿡�� ��ȣ �Ҵ�
        if (currentPlayerCount <= MaxPlayers)
        {
            assignedNumber = currentPlayerCount; //��ȣ �ο�
            player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "PlayerNumber", assignedNumber } });

            Debug.Log($"{player.NickName} assigned to Player Number {assignedNumber}");
            Create();
        }
        else
        {
            Debug.LogWarning("Room is already full.");
        }
    }

    #endregion

    void Create()
    {
        // ���� ��ġ ������ �迭�� ����
        Transform[] points = GameObject.Find("Spawnpoint").GetComponentsInChildren<Transform>();
        int idx = UnityEngine.Random.Range(1, points.Length);

        // ��Ʈ��ũ�� ĳ���� ����
        GameObject pLaYeR = PhotonNetwork.Instantiate("Player", points[idx].position, points[idx].rotation, 0);
        pLaYeR.GetComponent<PlayerValue>().number = assignedNumber;
        ;

    }

    #region UI_EFFECT

    public void OnLoginClick()
    {
        SetUserId();

    }

    public void SetNickName()
    {
        SetUserId();
    }

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

    public void MatchMacking()
    {
        PhotonNetwork.JoinRandomRoom();

    }

    //---------Will Be Removed-------------//
    public void StartButton()               //�� ������ȣ : ���� : �÷��̾� ���� : �÷��̾� Ÿ��
    {

        //Select Level


        PhotonNetwork.LoadLevel("BooAuk");
    }
    #endregion


}

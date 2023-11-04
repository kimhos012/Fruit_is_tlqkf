using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

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

        toggleColor = GameObject.Find("��ġ����ŷ_��ư").transform.GetChild(0).GetComponent<Image>();

        m_Toggle = GameObject.Find("��ġ����ŷ_��ư").GetComponent<Toggle>();

        m_Toggle.onValueChanged.AddListener(delegate
        {
            ToggleValueChanged(m_Toggle);
        }
        );
        toggleColor.color = new Color(0, 1, 0, 1);


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
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"JoinRandom Failed = {returnCode}:{message}");
        Debug.Log("Macking Room...");
        OnMakeRoomClick();
    }

    //�� ���� �Ϸ� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnCreatedRoom()
    {
        Debug.Log("Created Room");
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

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("FindPlayers");
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        OnMakeRoomClick();
    }

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
            Debug.Log("False");
        }
    }
    #endregion

    public static string MapN;      //�� ����
    public static string playerS;   //�÷��̾� ����
    public static int round;        //���� ���� ��

    public int Round;
    public string playerCurrentScore;

    //�ʰ� �÷��̾� ������ ������ �༮(String)  �ſ� �߿��� �༮
    private void Update()       //Setting Gamenum
    {
        Round = round;
        playerCurrentScore = ("p1p2p3p4" + playerS);
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
        PhotonNetwork.CreateRoom((Random.Range(0, 3000)) + (Random.Range(0 , 3000) + "a"), ro);
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

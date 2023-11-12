using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerSystem : MonoBehaviourPunCallbacks //PUN의 다양한 콜백 함수를 오버라이드해서 작성
{
    //게임 버전
    private readonly string version = "0.1";
    //유저 닉네임
    private string userId = "JackJack_E";

    // 유저명을 입력할 TextMeshPro Input Field
    public TMP_InputField userIF;
    public GameObject namePanel;
    public Image toggleColor;

    private Toggle m_Toggle;
    public static int assignedNumber;
    private static readonly int MaxPlayers = 4;
    void Awake()
    {
        //마스터 클라이언트(룸을 생성한 유저)의 씬 자동 동기화 옵션
        PhotonNetwork.AutomaticallySyncScene = true;

        //게임 버전 설정 (동일 버전의 유저끼리 컨넥팅)
        PhotonNetwork.GameVersion = version;

        //게임 유저의 닉네임 설정
        PhotonNetwork.NickName = userId;

        //포톤 서버와의 데이터의 초당 전송 횟수 (기본 30회)
        Debug.Log(PhotonNetwork.SendRate);

        //포톤 서버 접속
        PhotonNetwork.ConnectUsingSettings();

        //이름 정하는 화면 생성
        namePanel.SetActive(true);
    }

    void Start()
    {
        // 저장된 유저명을 로드
        userId = PlayerPrefs.GetString("USER_ID", $"USER_{Random.Range(1, 100):00}");
        userIF.text = userId;

        // 접속 유저의 닉네임 등록
        PhotonNetwork.NickName = userId;

        //UI쪽으로 이동
        toggleColor = GameObject.Find("매치메이킹_버튼").transform.GetChild(0).GetComponent<Image>();

        m_Toggle = GameObject.Find("매치메이킹_버튼").GetComponent<Toggle>();

        m_Toggle.onValueChanged.AddListener(delegate
        {
            ToggleValueChanged(m_Toggle);
        }
        );
        toggleColor.color = new Color(0, 1, 0, 1);
        //-------------
    }

    #region PhotonSys
    // 유저명을 설정하는 로직
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


                // 유저명 저장
                PlayerPrefs.SetString("USER_ID", userId);

                // 접속 유저의 닉네임 등록
                PhotonNetwork.NickName = userId;

                namePanel.SetActive(false);
            }
            else
            {
                Debug.Log("이름이 너무 긺!");
                userId = $"USER_{Random.Range(1, 21):00}";
            }

        }

    }

    //포턴 서버에 접속 후 가장 먼저 호출되는 콜백 함수
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}"); //자동 입장이 아니므로 false
        //PhotonNetwork.JoinLobby(); //로비 입장 명령 OnJoinedLobby 호출
    }

    //로비에 접속 후 호출되는 콜백 함수
    public override void OnJoinedLobby()
    {
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
    }

    //램덤한 룸 입장이 실패했을 경우 호출되는 콜백 함수
    public override void OnJoinRandomFailed(short returnCode, string message) => OnMakeRoomClick();
    //룸 생성 완료 후 호출되는 콜백 함수
    public override void OnCreatedRoom() => Debug.Log($"Room Name = {PhotonNetwork.CurrentRoom.Name}");
    //룸에 입장한 후 호출되는 콜백 함수
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
        //방 접속 후 플레이어 생성
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
        // 현재 룸에 있는 플레이어 수
        int currentPlayerCount = PhotonNetwork.CurrentRoom.PlayerCount;

        // 최대 플레이어 수를 초과하지 않는 경우에만 번호 할당
        if (currentPlayerCount <= MaxPlayers)
        {
            assignedNumber = currentPlayerCount; //번호 부여
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
        // 출현 위치 정보를 배열에 저장
        Transform[] points = GameObject.Find("Spawnpoint").GetComponentsInChildren<Transform>();
        int idx = UnityEngine.Random.Range(1, points.Length);

        // 네트워크상에 캐릭터 생성
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
        // 유저명 저장
        SetUserId();

        // 룸의 속성 정의
        RoomOptions ro = new RoomOptions
        {
            MaxPlayers = 4,     // 룸에 입장할 수 있는 최대 접속자 수
            IsOpen = true,       // 룸의 오픈 여부
            IsVisible = true    // 로비에서 룸 목록에 노출시킬 여부
        };

        // 룸 생성
        PhotonNetwork.CreateRoom((Random.Range(0, 3000)) + (Random.Range(0, 3000) + "a"), ro);
    }

    public void MatchMacking()
    {
        PhotonNetwork.JoinRandomRoom();

    }

    //---------Will Be Removed-------------//
    public void StartButton()               //맵 순서번호 : 라운드 : 플레이어 점수 : 플레이어 타입
    {

        //Select Level


        PhotonNetwork.LoadLevel("BooAuk");
    }
    #endregion


}

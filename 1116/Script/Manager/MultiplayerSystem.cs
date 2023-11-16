using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MultiplayerSystem : MonoBehaviourPunCallbacks //PUN의 다양한 콜백 함수를 오버라이드해서 작성
{
    //게임 버전
    private readonly string version = "0.1";
    //유저 닉네임
    private string userId = "JackJack_E";

    // 유저명을 입력할 TextMeshPro Input Field
    public TMP_InputField userIF;

    public static int assignedNumber;
    public GameObject StartPlayer;
    private static readonly int MaxPlayers = 4;

    public UISys uiScript;

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
    }

    void Start()
    {

        //이름창만 표시
        uiScript.NameToggle(true);
        uiScript.MainToggle(false);
        uiScript.OptionToggle(false);
        uiScript.RandomToggle(false);
        uiScript.CharToggle(false);

        // 저장된 유저명을 로드
        userId = PlayerPrefs.GetString("USER_ID", $"USER_{Random.Range(1, 100):00}");
        userIF.text = userId;

        // 접속 유저의 닉네임 등록
        PhotonNetwork.NickName = userId;
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

                uiScript.NameToggle(false);
                uiScript.MainToggle(true);
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
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        OnMakeRoomClick();
        
    }

    public override void OnCreateRoomFailed(short returnCode, string message) => OnMakeRoomClick();

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

    //룸 생성 완료 후 호출되는 콜백 함수
    public override void OnCreatedRoom()
    {
        Create(assignedNumber);
        Debug.Log($"Room Name = {PhotonNetwork.CurrentRoom.Name}");
    }

    //룸에 입장한 후 호출되는 콜백 함수
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
        //방 접속 후 플레이어 생성
        if (!PhotonNetwork.IsMasterClient)
        {
            Create(assignedNumber);
            Debug.Log("JoinedRoom에서 생성");
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
            Debug.Log("EnderRoom에서 생성");
        }
    }

    #endregion

    [PunRPC]
    private void AssignPlayerNumber(int playerActorNumber)
    {
        Player player = PhotonNetwork.CurrentRoom.GetPlayer(playerActorNumber);
        if (player != null)
        {
            // 현재 룸에 있는 플레이어 수
            int currentPlayerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            if (currentPlayerCount <= MaxPlayers)
            {
                assignedNumber = currentPlayerCount; //번호 부여
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

        // 출현 위치 정보를 배열에 저장
        Transform[] points = GameObject.Find("Spawnpoint").GetComponentsInChildren<Transform>();
        int idx = UnityEngine.Random.Range(1, points.Length);

        // 네트워크상에 캐릭터 생성
        GameObject pLaYeR = PhotonNetwork.Instantiate("Player", points[idx].position, points[idx].rotation, 0);
        pLaYeR.GetComponent<PlayerValue>().number = assignedNumber;
        Debug.Log("만든 플레이엉");

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

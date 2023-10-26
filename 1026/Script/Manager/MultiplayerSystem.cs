using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;


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

        toggleColor = GameObject.Find("매치메이킹_버튼").transform.GetChild(0).GetComponent<Image>();

        m_Toggle = GameObject.Find("매치메이킹_버튼").GetComponent<Toggle>();

        m_Toggle.onValueChanged.AddListener(delegate
        {
            ToggleValueChanged(m_Toggle);
        }
        );
        toggleColor.color = new Color(0, 1, 0, 1);
    }

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
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"JoinRandom Failed = {returnCode}:{message}");
        Debug.Log("Macking Room...");
        OnMakeRoomClick();
    }

    //룸 생성 완료 후 호출되는 콜백 함수
    public override void OnCreatedRoom()
    {
        Debug.Log("Created Room");
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
        PhotonNetwork.CreateRoom(Random.Range(-210000000,210000000)+("apple"), ro);
    }

    public void MatchMacking()
    {
        PhotonNetwork.JoinRandomRoom();

    }

    //---------Will Be Removed-------------//

    public void StartButton()
    {
        SceneManager.LoadScene("GamePlay");
    }

    #endregion
}

using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    private readonly string version = "1.0";
    private string userId = "ME";

    // 유저명을 입력할 TextMeshPro Input Field
    public TMP_InputField userInputField;
    // 룸 이름을 입력할 TextMexhPro Input Field
    public TMP_InputField roomNameInputField;

    public Button gameStartButton;
    public Button loginButton;
    public Button makeRoomButton;
    public GameObject panelLogin;
    public GameObject howToPlay;

    public static PhotonManager instance;

    public int maxPlayer = 3;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;    // 자신을 넣어줌
            DontDestroyOnLoad(gameObject);  // 씬이 로드됐을 때 자신을 파괴하지 않고 유지
        }
        else
        {
            Debug.LogWarning("씬에 두 개 이상의 게임 매니저가 존재합니다!");
            Destroy(gameObject);    // 두 개 이상일 떄는 나 자신을 삭제
        }

        // 마스터 클라이언트의 씬 자동 동기화 옵션
        // 룸에 입장한 다른 접속 유저들에게도 마스터 클라이언트의 씬을 자동으로 로딩 해주기 위해
        PhotonNetwork.AutomaticallySyncScene = true;

        // 게임 버전 설정
        PhotonNetwork.GameVersion = version;

        // 접속 유저의 닉네임 설정
        // PhotonNetwork.NickName = userId;

        // 포톤 서버와의 데이터의 초당 전송 횟수 (초당 30회로 설정돼 있음)
        Debug.Log(PhotonNetwork.SendRate);

        // 포톤 서버 접속
        PhotonNetwork.ConnectUsingSettings();
    }

    private void Start()
    {
        // 첫 화면에서는 로그인 창 false, 게임 방법 창 false;
        panelLogin.SetActive(false);
        howToPlay.SetActive(false);

        // 닉네임,룸 생성 버튼 잠시 비활성화
        loginButton.interactable = false;
        makeRoomButton.interactable = false;

        // 저장된 유저명 로드
        userId = PlayerPrefs.GetString("USER_ID", $"USER_{Random.Range(1, 21):00}");
        userInputField.text = userId;
        // 접속 유저의 닉네임 등록
        PhotonNetwork.NickName = userId;
    }

    public void TogglePanelLogin()
    {
        // 게임 시작 버튼 누를 시 로그인 창 상태 변경
        panelLogin.SetActive(!panelLogin.activeSelf);
    }

    public void ToggleHowToPlay()
    {
        howToPlay.SetActive(!howToPlay.activeSelf);
    }

    // 유저명 설정
    public void SetUserId()
    {
        if (string.IsNullOrEmpty(userInputField.text))
        {
            userId = $"USER_{Random.Range(1, 21):00}";
        }
        else
        {
            userId = userInputField.text;
        }

        // 유저명 저장
        PlayerPrefs.SetString("UWER_ID", userId);
        // 접속 유저의 닉네임 등록
        PhotonNetwork.NickName = userId;
    }

    // 룸 이름 입력 여부를 확인하는
    private string SetRoomName()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            roomNameInputField.text = $"ROOM_{Random.Range(1, 101):000}";
        }

        return roomNameInputField.text;
    }

    // 마스터 서버 접속 성공 후 가장 먼저 호출되는 콜백 함수
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");

        // 로비 입장 함수
        PhotonNetwork.JoinLobby();
    }

    // 로비 접속 성공 후 호출되는 콜백 함수
    public override void OnJoinedLobby()
    {
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");

        // 룸에 수동으로 접속하기 위해 자동 입장은 주석 처리함
        // PhotonNetwork.JoinRandomRoom();

        // 닉네임,룸 생성 버튼 활성화
        loginButton.interactable = true;
        makeRoomButton.interactable = true;
    }

    // 랜덤 룸 입장 실패했을 경우 호출되는 콜백 함수
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"JoinRandom Filed {returnCode}:{message}");

        // 룸 생성하는 함수 실행
        OnMakeRoomClick();
    }

    // 룸 생성 완료 후 호출되는 콜백 함수
    public override void OnCreatedRoom()
    {
        Debug.Log("Created Room");
        Debug.Log($"Room Name = {PhotonNetwork.CurrentRoom.Name}");
    }

    // 룸 입장 후 호출되는 콜백 함수
    public override void OnJoinedRoom()
    {
        Debug.Log($"PhotonNetwork.InRoom = {PhotonNetwork.InRoom}");
        Debug.Log($"Player Count = {PhotonNetwork.CurrentRoom.PlayerCount}");

        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log($"{player.Value.NickName} , {player.Value.ActorNumber}");
        }


        //// 마스터 클라이언트인 경우에 룸에 입장한 후 플레이 씬을 로딩
        //if (PhotonNetwork.IsMasterClient)
        //{
        Debug.Log($"{userId}");

        PhotonNetwork.LoadLevel("02.ReadyScene");
        //}

    }

    //public void OnPlayScene()

    //    // 마스터 클라이언트인 경우에 룸에 입장한 후 플레이 씬을 로딩
    //    if (PhotonNetwork.IsMasterClient)
    //    {
    //        Debug.Log($"{userId}");

    //        PhotonNetwork.LoadLevel("03.PirateMapScene");
    //    }
    //}

    #region UI_BUTTON_EVENT

    public void OnLoginClick()
    {
        // 중복 접속 시도를 막기 위해 접속 버튼 잠시 비활성화
        loginButton.interactable = false;

        // 유저명 저장
        SetUserId();

        // 랜덤으로 룸 입장
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnMakeRoomClick()
    {
        // 중복 접속 시도를 막기 위해 접속 버튼 잠시 비활성화
        makeRoomButton.interactable = false;

        // 유저명 저장
        SetUserId();

        // 룸 속성 정의
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxPlayer;     // 룸에 입장 할 수 있는 최대 접속자 수
        roomOptions.IsOpen = true;      // 룸 오픈 여부
        roomOptions.IsVisible = true;   // 로비에서 룸 목록에 노출시킬지

        // 룸 생성
        Debug.Log("Create New Room");
        PhotonNetwork.CreateRoom(SetRoomName(), roomOptions);
    }

    #endregion
}
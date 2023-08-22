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

    // �������� �Է��� TextMeshPro Input Field
    public TMP_InputField userInputField;
    // �� �̸��� �Է��� TextMexhPro Input Field
    public TMP_InputField roomNameInputField;

    public Button gameStartButton;
    public Button loginButton;
    public Button makeRoomButton;
    public GameObject panelLogin;

    public static PhotonManager instance;

    public int maxPlayer = 2;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;    // �ڽ��� �־���
            DontDestroyOnLoad(gameObject);  // ���� �ε���� �� �ڽ��� �ı����� �ʰ� ����
        }
        else
        {
            Debug.LogWarning("���� �� �� �̻��� ���� �Ŵ����� �����մϴ�!");
            Destroy(gameObject);    // �� �� �̻��� ���� �� �ڽ��� ����
        }

        // ������ Ŭ���̾�Ʈ�� �� �ڵ� ����ȭ �ɼ�
        // �뿡 ������ �ٸ� ���� �����鿡�Ե� ������ Ŭ���̾�Ʈ�� ���� �ڵ����� �ε� ���ֱ� ����
        PhotonNetwork.AutomaticallySyncScene = true;

        // ���� ���� ����
        PhotonNetwork.GameVersion = version;

        // ���� ������ �г��� ����
        // PhotonNetwork.NickName = userId;

        // ���� �������� �������� �ʴ� ���� Ƚ�� (�ʴ� 30ȸ�� ������ ����)
        Debug.Log(PhotonNetwork.SendRate);

        // ���� ���� ����
        PhotonNetwork.ConnectUsingSettings();
    }

    private void Start()
    {
        // ù ȭ�鿡���� �α��� â false
        panelLogin.SetActive(false);

        // �г���,�� ���� ��ư ��� ��Ȱ��ȭ
        loginButton.interactable = false;
        makeRoomButton.interactable = false;

        // ����� ������ �ε�
        userId = PlayerPrefs.GetString("USER_ID", $"USER_{Random.Range(1, 21):00}");
        userInputField.text = userId;
        // ���� ������ �г��� ���
        PhotonNetwork.NickName = userId;
    }

    public void TogglePanelLogin()
    {
        // ���� ���� ��ư ���� �� �α��� â ���� ����
        panelLogin.SetActive(!panelLogin.activeSelf);
    }

    // ������ ����
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

        // ������ ����
        PlayerPrefs.SetString("UWER_ID", userId);
        // ���� ������ �г��� ���
        PhotonNetwork.NickName = userId;
    }

    // �� �̸� �Է� ���θ� Ȯ���ϴ�
    private string SetRoomName()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            roomNameInputField.text = $"ROOM_{Random.Range(1, 101):000}";
        }

        return roomNameInputField.text;
    }

    // ������ ���� ���� ���� �� ���� ���� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");

        // �κ� ���� �Լ�
        PhotonNetwork.JoinLobby();
    }

    // �κ� ���� ���� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnJoinedLobby()
    {
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");

        // �뿡 �������� �����ϱ� ���� �ڵ� ������ �ּ� ó����
        // PhotonNetwork.JoinRandomRoom();

        // �г���,�� ���� ��ư Ȱ��ȭ
        loginButton.interactable = true;
        makeRoomButton.interactable = true;
    }

    // ���� �� ���� �������� ��� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"JoinRandom Filed {returnCode}:{message}");

        // �� �����ϴ� �Լ� ����
        OnMakeRoomClick();
    }

    // �� ���� �Ϸ� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnCreatedRoom()
    {
        Debug.Log("Created Room");
        Debug.Log($"Room Name = {PhotonNetwork.CurrentRoom.Name}");
    }

    // �� ���� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnJoinedRoom()
    {
        Debug.Log($"PhotonNetwork.InRoom = {PhotonNetwork.InRoom}");
        Debug.Log($"Player Count = {PhotonNetwork.CurrentRoom.PlayerCount}");

        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log($"{player.Value.NickName} , {player.Value.ActorNumber}");
        }


        //// ������ Ŭ���̾�Ʈ�� ��쿡 �뿡 ������ �� �÷��� ���� �ε�
        //if (PhotonNetwork.IsMasterClient)
        //{
            Debug.Log($"{userId}");

            PhotonNetwork.LoadLevel("02.ReadyScene");
        //}

    }

    //public void OnPlayScene()
    
    //    // ������ Ŭ���̾�Ʈ�� ��쿡 �뿡 ������ �� �÷��� ���� �ε�
    //    if (PhotonNetwork.IsMasterClient)
    //    {
    //        Debug.Log($"{userId}");

    //        PhotonNetwork.LoadLevel("03.PirateMapScene");
    //    }
    //}

    #region UI_BUTTON_EVENT

    public void OnLoginClick()
    {
        // �ߺ� ���� �õ��� ���� ���� ���� ��ư ��� ��Ȱ��ȭ
        loginButton.interactable = false;

        // ������ ����
        SetUserId();

        // �������� �� ����
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnMakeRoomClick()
    {
        // �ߺ� ���� �õ��� ���� ���� ���� ��ư ��� ��Ȱ��ȭ
        makeRoomButton.interactable = false;

        // ������ ����
        SetUserId();

        // �� �Ӽ� ����
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxPlayer;     // �뿡 ���� �� �� �ִ� �ִ� ������ ��
        roomOptions.IsOpen = true;      // �� ���� ����
        roomOptions.IsVisible = true;   // �κ񿡼� �� ��Ͽ� �����ų��

        // �� ����
        Debug.Log("Create New Room");
        PhotonNetwork.CreateRoom(SetRoomName(), roomOptions);
    }

    #endregion
}

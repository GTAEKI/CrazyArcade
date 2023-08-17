using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    private readonly string version = "1.0";
    private string userId = "ME";

    // �������� �Է��� TextMeshPro Input Field
    public TMP_InputField userInputField;
    // �� �̸��� �Է��� TextMexhPro Input Field
    public TMP_InputField roomNameInputField;

    private void Awake()
    {
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
        // ����� ������ �ε�
        userId = PlayerPrefs.GetString("USER_ID", $"USER_{Random.Range(1, 21):00}");
        userInputField.text = userId;
        // ���� ������ �г��� ���
        PhotonNetwork.NickName = userId;
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
    }

    // ���� �� ���� �������� ��� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"JoinRandom Filed {returnCode}:{message}");
        
        // �� �����ϴ� �Լ� ����
        OnMakeRoomClick();

        //// �� �Ӽ� ����
        //RoomOptions roomOptions = new RoomOptions();
        //roomOptions.MaxPlayers = 2;     // �뿡 ���� �� �� �ִ� �ִ� ������ ��
        //roomOptions.IsOpen = true;      // �� ���� ����
        //roomOptions.IsVisible = true;   // �κ񿡼� �� ��Ͽ� �����ų��

        //// �� ����
        //Debug.Log("Create New Room");
        //PhotonNetwork.CreateRoom(null, roomOptions);
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

        //// ĳ���� ���� ��ġ ������ �迭�� ����
        //Transform[] points = GameObject.Find("CharactorRespawnController").
        //    GetComponentsInChildren<Transform>();
        //int idx = Random.Range(1, points.Length);

        //// ��Ʈ��ũ�� ĳ���� ����
        //PhotonNetwork.Instantiate("PlayerBazzi", points[idx].position, points[idx].rotation, 0);

        // ������ Ŭ���̾�Ʈ�� ��쿡 �뿡 ������ �� �÷��� ���� �ε�
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log($"{userId}");

            PhotonNetwork.LoadLevel("MJ_Scene_COPY_KT");
        }
    }

    #region UI_BUTTON_EVENT

    public void OnLoginClick()
    {
        // ������ ����
        SetUserId();

        // �������� �� ����
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnMakeRoomClick()
    {
        // ������ ����
        SetUserId();

        // �� �Ӽ� ����
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;     // �뿡 ���� �� �� �ִ� �ִ� ������ ��
        roomOptions.IsOpen = true;      // �� ���� ����
        roomOptions.IsVisible = true;   // �κ񿡼� �� ��Ͽ� �����ų��

        // �� ����
        Debug.Log("Create New Room");
        PhotonNetwork.CreateRoom(SetRoomName(), roomOptions);
    }

    #endregion
}

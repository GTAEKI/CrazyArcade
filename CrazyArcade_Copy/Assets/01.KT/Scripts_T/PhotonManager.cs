using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    private readonly string version = "1.0";
    private string userId = "ME";

    private void Awake()
    {
        // ������ Ŭ���̾�Ʈ�� �� �ڵ� ����ȭ �ɼ�
        // �뿡 ������ �ٸ� ���� �����鿡�Ե� ������ Ŭ���̾�Ʈ�� ���� �ڵ����� �ε� ���ֱ� ����
        PhotonNetwork.AutomaticallySyncScene = true;

        PhotonNetwork.GameVersion = version;
        PhotonNetwork.NickName = userId;

        // ���� �������� �������� �ʴ� ���� Ƚ�� (�ʴ� 30ȸ�� ������ ����)
        Debug.Log(PhotonNetwork.SendRate);

        // ���� ���� ����
        PhotonNetwork.ConnectUsingSettings();
    }

    // ���� ���� ���� �� ���� ���� ȣ��Ǵ� �ݹ� �Լ�
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
        PhotonNetwork.JoinRandomRoom();
    }

    // ���� �� ���� �������� ��� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"JoinRandom Filed {returnCode}:{message}");

        // �� �Ӽ� ����
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;     // �뿡 ���� �� �� �ִ� �ִ� ������ ��
        roomOptions.IsOpen = true;      // �� ���� ����
        roomOptions.IsVisible = true;   // �κ񿡼� �� ��Ͽ� �����ų��

        // �� ����
        PhotonNetwork.CreateRoom("My Room", roomOptions);
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

        foreach(var player in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log($"{player.Value.NickName} , {player.Value.ActorNumber}");
        }

        // ĳ���� ���� ��ġ ������ �迭�� ����
        Transform[] points = GameObject.Find("CharactorRespawnController").
            GetComponentsInChildren<Transform>();
        int idx = Random.Range(1, points.Length);
        
        // ��Ʈ��ũ�� ĳ���� ����
        PhotonNetwork.Instantiate("PlayerBazzi", points[idx].position, points[idx].rotation, 0);
    }

}

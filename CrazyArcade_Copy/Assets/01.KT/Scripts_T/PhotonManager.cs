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
        // 마스터 클라이언트의 씬 자동 동기화 옵션
        // 룸에 입장한 다른 접속 유저들에게도 마스터 클라이언트의 씬을 자동으로 로딩 해주기 위해
        PhotonNetwork.AutomaticallySyncScene = true;

        PhotonNetwork.GameVersion = version;
        PhotonNetwork.NickName = userId;

        // 포톤 서버와의 데이터의 초당 전송 횟수 (초당 30회로 설정돼 있음)
        Debug.Log(PhotonNetwork.SendRate);

        // 포톤 서버 접속
        PhotonNetwork.ConnectUsingSettings();
    }

    // 포톤 서버 접속 후 가장 먼저 호출되는 콜백 함수
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
        PhotonNetwork.JoinRandomRoom();
    }

    // 랜덤 룸 입장 실패했을 경우 호출되는 콜백 함수
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"JoinRandom Filed {returnCode}:{message}");

        // 룸 속성 정의
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;     // 룸에 입장 할 수 있는 최대 접속자 수
        roomOptions.IsOpen = true;      // 룸 오픈 여부
        roomOptions.IsVisible = true;   // 로비에서 룸 목록에 노출시킬지

        // 룸 생성
        PhotonNetwork.CreateRoom("My Room", roomOptions);
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

        foreach(var player in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log($"{player.Value.NickName} , {player.Value.ActorNumber}");
        }

        // 캐릭터 출현 위치 정보를 배열에 저장
        Transform[] points = GameObject.Find("CharactorRespawnController").
            GetComponentsInChildren<Transform>();
        int idx = Random.Range(1, points.Length);
        
        // 네트워크상에 캐릭터 생성
        PhotonNetwork.Instantiate("PlayerBazzi", points[idx].position, points[idx].rotation, 0);
    }

}

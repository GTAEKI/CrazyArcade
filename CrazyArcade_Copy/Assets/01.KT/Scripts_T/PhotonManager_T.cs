using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager_T : MonoBehaviourPunCallbacks
{
    // 게임의 버전
    private readonly string version = "1.0";

    // 유저의 닉네임
    private string userId = "Zack";


    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        PhotonNetwork.GameVersion = version;

        //접속 유저의 닉네임 설정
        PhotonNetwork.NickName = userId;

        PhotonNetwork.SendRate = 45;

        //포톤 서버와 데이터의 초당 전송 횟수
        Debug.Log(PhotonNetwork.SendRate);

        // 포톤 서버 접속
        PhotonNetwork.ConnectUsingSettings();
    }

    // 포톤 서버에 접속 후 호출되는 콜백 함수
    // 이 콜백함수에서 로비에 들어왔는지 여부를 나타내는 PhotonNetwork.InLobby 속성을 출력해본다.
    // 포톤은 로비에 자동으로 입장시키지 않기 때문에 False가 출력된다.
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master!");
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinLobby();//JoinLobby함수 >> 로비에 입장하는 함수
    }

    //로비에 접속 후 호출되는 콜백함수
    public override void OnJoinedLobby()
    {
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinRandomRoom();
    }

    // 포톤 서버는 랜덤 매치 메이킹 기능을 제공한다. JoinRandomRoom은 포톤 서버에 접속하거나 로비에 입장한 후
    // 이미 생성된 룸 중에서 무작위로 선택해 입장할 수 있는 함수이다.
    // 아무런 룸이 생성되지 않았다면 룸에 입장할 수 없으며 이때 OnJoinedRandomFailed 콜백 함수가 발생한다.

    //랜덤한 룸 입장이 실패했을 경우 호출되는 콜백 함수
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"JoinRandom Failed {returnCode}:{message}");

        //룸의 속성 정의
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 2; // 룸에 입장할 수 있는 최대 접속자 수
        ro.IsOpen = true; // 룸의 오픈 여부
        ro.IsVisible = true; // 로비에서 룸 목록에 노출시킬지 여부

        //룸 생성
        PhotonNetwork.CreateRoom("MyRoom", ro);
    }

    // 룸 생성이 완료된 후 호출되는 콜백 함수
    public override void OnCreatedRoom()
    {
        Debug.Log("Created Room");
        Debug.Log($"Room Name = {PhotonNetwork.CurrentRoom.Name}");
    }

    // 룸에 입장한 후 호출되는 콜백 함수
    public override void OnJoinedRoom()
    {
        // 룸에 입장시, 닉네임과 ActorNumber 로그 찍음
        foreach(var player in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log($"{player.Value.NickName}, {player.Value.ActorNumber}");
        }

        // 룸에 입장시 실행되는 함수
        GameObject.Find("CharactorRespawnController").GetComponent<CharactorRespawnController>().CreatePlayer();
        GameObject.Find("ItemRespawnController").GetComponent<ItemRespawnController>().CreateItems();
    }
}

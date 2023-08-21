using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class ReadySceneManager : MonoBehaviourPunCallbacks
{
    public Button startButton;

    private GameObject[] readyPlayerPositions;
    private void Awake()
    {
        // 마스터 클라이언트의 씬 자동 동기화 옵션
        // 룸에 입장한 다른 접속 유저들에게도 마스터 클라이언트의 씬을 자동으로 로딩 해주기 위해
        PhotonNetwork.AutomaticallySyncScene = true;

        // 하이어라키창의 캐릭터 입장 위치들을 배열에 저장
        readyPlayerPositions = GameObject.FindGameObjectsWithTag("ReadyPlayerPosition");

        // 캐릭터 입장시 순서에 맞춰 캐릭터 오브젝트 true
        OnReadyScene();

    }

    public void OnReadyScene()
    {

    }
    public void OnStartButtonClick()
    {
        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log($"{player.Value.NickName} , {player.Value.ActorNumber}");
        }

        // 마스터 클라이언트인 경우에만 플레이 씬 로딩
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("03.PirateMapScene");
        }
        else
        {
            Debug.Log("Only Master Client can move to Scene 03.");
        }
    }
}

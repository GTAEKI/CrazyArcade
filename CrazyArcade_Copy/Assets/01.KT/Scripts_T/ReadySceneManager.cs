using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class ReadySceneManager : MonoBehaviourPunCallbacks
{
    public Button startButton;

    private int playerNum = 0;
    private GameObject[] readyPlayerPositions;

    private void Awake()
    {
        // 마스터 클라이언트의 씬 자동 동기화 옵션
        // 룸에 입장한 다른 접속 유저들에게도 마스터 클라이언트의 씬을 자동으로 로딩 해주기 위해
        PhotonNetwork.AutomaticallySyncScene = true;


        readyPlayerPositions = new GameObject[8];
        // 하이어라키창의 캐릭터 입장 위치들을 배열에 저장
        //readyPlayerPositions = GameObject.FindGameObjectsWithTag("ReadyPlayerPosition");
        readyPlayerPositions[0] = GameObject.Find("ReadyPlayerPosition").transform.GetChild(0).gameObject;
        Debug.Log(readyPlayerPositions[0].name);
        Debug.Log(GameObject.Find("FirstPlayer").transform.root.GetChild(0).name);

        // GameObject.Find(이름) -> Scene 하위의 모든 오브젝트 중에서 일치하는 이름의 GameObject 를 가져옴
        // gameObject.transform.Find 또는 gameObject.transform.GetChild(인덱스 번호) -> 'gameObject' 이 오브젝트를 부모로 삼고
        // 그 하위에 있는 자식 오브젝트를 인덱스 번호에 맞게 가져옴
        // gameObject.transform.parent.parent -> 부모-부모를 부름
        // gameObject.transform.root.getChild(인덱스번호) -> 씬을 제외하고 가장 최상의 오브젝트의 인덱스번호 번째 오브젝트를 부름


        // 캐릭터 입장시 순서에 맞춰 캐릭터 오브젝트 true
        OnReadyScene();

    }


    // 캐릭터 입장 순서에 맞춰서 오브젝트 나타내는
    public void OnReadyScene()
    {
        Debug.Log($"{readyPlayerPositions.Length}");
        Debug.Log($"{playerNum}");

        readyPlayerPositions[playerNum].SetActive(false);
        playerNum++;
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

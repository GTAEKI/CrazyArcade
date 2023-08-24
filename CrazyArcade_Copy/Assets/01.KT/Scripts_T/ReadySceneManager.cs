using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class ReadySceneManager : MonoBehaviourPunCallbacks
{
    public Button startButton;
    public GameObject button;

    private int playerNum = 0;
    private GameObject[] readyPlayerPositions;


    private void Awake()
    {
        // 마스터 클라이언트의 씬 자동 동기화 옵션
        // 룸에 입장한 다른 접속 유저들에게도 마스터 클라이언트의 씬을 자동으로 로딩 해주기 위해
        PhotonNetwork.AutomaticallySyncScene = true;

        // 캐릭터 입장시 순서에 맞춰 캐릭터 오브젝트 true
        OnReadyScene();

        if (PhotonNetwork.IsMasterClient)
        {
            // 마스터 클라이언트는 다른 플레이어가 모두 준비하기 전까지 버튼 비활성화
            startButton.interactable = false;
        }
        else
        {
            // 마스터를 제외한 플레이어는 버튼 활성화
            startButton.interactable = true;
        }

        #region Find,parent,root 설명
        // 하이어라키창의 캐릭터 입장 위치들을 배열에 저장
        //readyPlayerPositions = GameObject.FindGameObjectsWithTag("ReadyPlayerPosition");
        //readyPlayerPositions[0] = GameObject.Find("ReadyPlayerPosition").transform.GetChild(0).gameObject;
        //Debug.Log(readyPlayerPositions[0].name);
        //Debug.Log(GameObject.Find("FirstPlayer").transform.root.GetChild(0).name);

        // GameObject.Find(이름) -> Scene 하위의 모든 오브젝트 중에서 일치하는 이름의 GameObject 를 가져옴
        // gameObject.transform.Find 또는 gameObject.transform.GetChild(인덱스 번호) -> 'gameObject' 이 오브젝트를 부모로 삼고
        // 그 하위에 있는 자식 오브젝트를 인덱스 번호에 맞게 가져옴
        // gameObject.transform.parent.parent -> 부모-부모를 부름
        // gameObject.transform.root.getChild(인덱스번호) -> 씬을 제외하고 가장 최상의 오브젝트의 인덱스번호 번째 오브젝트를 부름
        #endregion
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
        if (PhotonNetwork.IsMasterClient && startButton.interactable)
        {
            PhotonNetwork.LoadLevel("03.PirateMapScene");
        }
        else
        {
            Debug.Log("Only Master Client can move to Scene 03" +
                "or all players are not ready.");
        }
    }

    public void OnButtonAPressed()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("ButtonAPressed", RpcTarget.MasterClient);
        }
    }

    [PunRPC]
    private void ButtonAPressed()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            button.SetActive(true);
        }
    }
}

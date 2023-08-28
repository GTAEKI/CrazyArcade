using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class ReadySceneManager : MonoBehaviourPunCallbacks
{
    #region 씬 입장시 이미지 출력하기 위한
    public Transform[] appearPositions;     // 출력할 위치 배열
    public GameObject[] enterPlayerImages;  // 출력할 이미지 프리팹 배열
    private int selectedNumber;
    #endregion

    public Button startButton;
    public Button priateMap;
    public Button tankMap;
    public Button firstChoiceButton;
    public Button secondChoiceButton;

    public AudioClip readySceneSound;

    public GameObject priate_Choiced;
    public GameObject tank_Choiced;
    public GameObject choiceMap;
    public GameObject priateYellowEffect;
    public GameObject tankYellowEffect;
    public GameObject button;

    private bool isMapPriate;
    private bool isMapTank;

    private enum ChoiceMap
    {
        map_Priate,
        map_Tank
    }
    ChoiceMap confirmMap = default;

    private void Awake()
    {
        confirmMap = ChoiceMap.map_Priate;

        AudioManager.instance.PlayMusicLoop(readySceneSound);

        CreatePlayerImg();
    }

    public void CreatePlayerImg()
    {
        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        int idx = playerCount - 1;

        selectedNumber = Random.Range(0, enterPlayerImages.Length);

        PhotonNetwork.Instantiate(enterPlayerImages[selectedNumber].name,
            appearPositions[idx].position, Quaternion.identity);
    }
    #region 맵 선택하는 부분
    // 맵 고르는 창을 여는 첫번째 버튼클릭시 실행
    public void ClickFirstChoiceButton()
    {
        //맵 선택은 방장만 할 수 있습니다.
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        choiceMap.SetActive(true); // 맵 고르는 창 활성화
        firstChoiceButton.interactable = false; //맵 고르는 버튼 비활성화
        startButton.interactable = false; // 시작버튼 비활성화
    }
    // 맵을 고른 뒤 확정하는 두번째 버튼 클릭시 실행
    public void ClickSecondChoiceButton()
    {
        firstChoiceButton.interactable = true; // 맵 고르는 버튼 활성화
        startButton.interactable = true; // 시작버튼 활성화
        choiceMap.SetActive(false); // 맵 고르는 창 비활성화
        if (PhotonNetwork.IsMasterClient) // 방장만 맵 확정가능
        {
            //확정한 함수 다른 클라이언트에게 전달하여 똑같이 실행하도록 함
            photonView.RPC("ConfirmMap", RpcTarget.All, isMapPriate, isMapTank);
        }
    }

    // 해적맵 고를시 실행되는 함수
    public void ClickPirateMap()
    {
        isMapPriate = true;
        isMapTank = false;
        priateYellowEffect.SetActive(true);
        tankYellowEffect.SetActive(false);
    }

    // 탱크맵 고를시 실행되는 함수
    public void ClickTankMap()
    {
        isMapTank = true;
        isMapPriate = false;
        tankYellowEffect.SetActive(true);
        priateYellowEffect.SetActive(false);
    }

    // 맵 확정시 실행되는 함수
    [PunRPC]
    public void ConfirmMap(bool isMapPriate, bool isMapTank)
    {
        this.isMapPriate = isMapPriate; //호스트에서 받은 bool값 클라이언트에서 전달받음
        this.isMapTank = isMapTank;//호스트에서 받은 bool값 클라이언트에서 전달받음

        if (this.isMapPriate)
        {
            confirmMap = ChoiceMap.map_Priate;
            priate_Choiced.SetActive(true);
            tank_Choiced.SetActive(false);

        }
        else if (this.isMapTank)
        {
            confirmMap = ChoiceMap.map_Tank;
            priate_Choiced.SetActive(false);
            tank_Choiced.SetActive(true);
        }
    }
    #endregion

    #region PlayScene으로 이동
    public void OnStartButtonClick()
    {
        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log($"{player.Value.NickName} , {player.Value.ActorNumber}");
        }

        // 마스터 클라이언트인 경우에만 플레이 씬 로딩
        if (PhotonNetwork.IsMasterClient && startButton.interactable)
        {
            if (confirmMap == ChoiceMap.map_Priate)
            {
                PhotonNetwork.LoadLevel("03.PirateMapScene");
            }
            else if (confirmMap == ChoiceMap.map_Tank)
            {
                PhotonNetwork.LoadLevel("04.TankMapScene");
            }
        }
        else
        {
            Debug.Log("Only Master Client can move to Scene 03" +
                "or all players are not ready.");
        }
    }
    #endregion
}
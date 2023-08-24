using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class ReadySceneManager : MonoBehaviourPunCallbacks
{
    public Button startButton;
    public Button priateMap;
    public Button tankMap;
    public Button firstChoiceButton;
    public Button secondChoiceButton;

    public GameObject priate_Choiced;
    public GameObject tank_Choiced;
    public GameObject choiceMap;
    public GameObject priateYellowEffect;
    public GameObject tankYellowEffect;
    public GameObject button;

    private int playerNum = 0;
    private GameObject[] readyPlayerPositions;

    private bool[] playersReady;
    private bool allPlayersReady = false;
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
        // 마스터 클라이언트의 씬 자동 동기화 옵션
        // 룸에 입장한 다른 접속 유저들에게도 마스터 클라이언트의 씬을 자동으로 로딩 해주기 위해
        PhotonNetwork.AutomaticallySyncScene = true;
        confirmMap = ChoiceMap.map_Priate;

        // 마스터 클라이언트가 아닌 플레이어들만 버튼을 누를 수 있도록 설정
        readyPlayerPositions = new GameObject[PhotonManager.instance.maxPlayer];
        playersReady = new bool[PhotonManager.instance.maxPlayer];

        // 하이어라키창의 캐릭터 입장 위치들을 배열에 저장
        // ĳ���� ����� ������ ���� ĳ���� ������Ʈ true
        OnReadyScene();

        if (PhotonNetwork.IsMasterClient)
        {
            // ������ Ŭ���̾�Ʈ�� �ٸ� �÷��̾ ��� �غ��ϱ� ������ ��ư ��Ȱ��ȭ
            startButton.interactable = false;
        }
        else
        {
            // �����͸� ������ �÷��̾�� ��ư Ȱ��ȭ
            startButton.interactable = true;
        }

        #region Find,parent,root ����
        // ���̾��Űâ�� ĳ���� ���� ��ġ���� �迭�� ����
        //readyPlayerPositions = GameObject.FindGameObjectsWithTag("ReadyPlayerPosition");
        //readyPlayerPositions[0] = GameObject.Find("ReadyPlayerPosition").transform.GetChild(0).gameObject;
        //Debug.Log(readyPlayerPositions[0].name);
        //Debug.Log(GameObject.Find("FirstPlayer").transform.root.GetChild(0).name);

        // GameObject.Find(�̸�) -> Scene ������ ��� ������Ʈ �߿��� ��ġ�ϴ� �̸��� GameObject �� ������
        // gameObject.transform.Find �Ǵ� gameObject.transform.GetChild(�ε��� ��ȣ) -> 'gameObject' �� ������Ʈ�� �θ�� ���
        // �� ������ �ִ� �ڽ� ������Ʈ�� �ε��� ��ȣ�� �°� ������
        // gameObject.transform.parent.parent -> �θ�-�θ� �θ�
        // gameObject.transform.root.getChild(�ε�����ȣ) -> ���� �����ϰ� ���� �ֻ��� ������Ʈ�� �ε�����ȣ ��° ������Ʈ�� �θ�
        #endregion
    }

    // 맵 고르는 창을 여는 첫번째 버튼클릭시 실행
    public void ClickFirstChoiceButton()
    {
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
        ConfirmMap(); // 맵 확정함수 실행
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
    public void ConfirmMap()
    {
        Debug.Log(confirmMap);

        if (isMapPriate)
        {
            confirmMap = ChoiceMap.map_Priate;
            priate_Choiced.SetActive(true);
            tank_Choiced.SetActive(false);

        }
        else if (isMapTank)
        {
            confirmMap = ChoiceMap.map_Tank;
            priate_Choiced.SetActive(false);
            tank_Choiced.SetActive(true);
        }
    }

    // 캐릭터 입장 순서에 맞춰서 오브젝트 나타내는
    public void OnReadyScene()
    {
        Debug.Log($"{readyPlayerPositions.Length}");
        Debug.Log($"{playerNum}");

        readyPlayerPositions[playerNum].SetActive(false);
        playerNum++;
    }

    private void CheckAllReadyButton()
    {
        bool allReady = true;

        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            if (i + 1 != PhotonNetwork.MasterClient.ActorNumber)
            {
                if (!playersReady[i])
                {
                    allReady = false;
                    break;
                }
            }
        }
        allPlayersReady = allReady;
        startButton.interactable = allReady;
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
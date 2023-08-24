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
        // ������ Ŭ���̾�Ʈ�� �� �ڵ� ����ȭ �ɼ�
        // �뿡 ������ �ٸ� ���� �����鿡�Ե� ������ Ŭ���̾�Ʈ�� ���� �ڵ����� �ε� ���ֱ� ����
        PhotonNetwork.AutomaticallySyncScene = true;

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


    // ĳ���� ���� ������ ���缭 ������Ʈ ��Ÿ����
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

        // ������ Ŭ���̾�Ʈ�� ��쿡�� �÷��� �� �ε�
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

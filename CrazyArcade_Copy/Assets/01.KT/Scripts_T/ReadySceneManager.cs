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
        // ������ Ŭ���̾�Ʈ�� �� �ڵ� ����ȭ �ɼ�
        // �뿡 ������ �ٸ� ���� �����鿡�Ե� ������ Ŭ���̾�Ʈ�� ���� �ڵ����� �ε� ���ֱ� ����
        PhotonNetwork.AutomaticallySyncScene = true;

        // ���̾��Űâ�� ĳ���� ���� ��ġ���� �迭�� ����
        readyPlayerPositions = GameObject.FindGameObjectsWithTag("ReadyPlayerPosition");

        // ĳ���� ����� ������ ���� ĳ���� ������Ʈ true
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

        // ������ Ŭ���̾�Ʈ�� ��쿡�� �÷��� �� �ε�
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

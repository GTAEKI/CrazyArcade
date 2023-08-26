using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;
    //public GameObject[] playerCount;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("씬에 두 개 이상의 게임 매니저가 존재합니다!");
            Destroy(gameObject);
        }
    }

    public GameObject[] playerCount;
    public bool isLose = false; // 패배 상태
    public bool isWin = false; // 승리 상태
    public bool isDraw = false; // 무승부 상태
    public bool isGameOver = false; // 게임 종료

    public GameObject inventory_NiddleImage;
    public GameObject itemCtrl_NiddleImage;
    public GameObject loseImage;
    public GameObject winImage;
    public GameObject drawImage;
    public Text timeText_Sec;
    public Text timeText_Min;
    public Text niddleAmount;

    public float remainTime_Sec = 120f; // 남은 시간 2분
    private float time_Sec;
    private int time_Min;

    public AudioClip winSound;
    public AudioClip loseAndDrawSound;

    void Start()
    {
        // 시간 초기화
        time_Sec = 60f;
        time_Min = (int)(remainTime_Sec / time_Sec);
        time_Sec = remainTime_Sec % time_Sec;

        // 이미지 비활성화
        inventory_NiddleImage.SetActive(false);
        itemCtrl_NiddleImage.SetActive(false);
        loseImage.SetActive(false);
        winImage.SetActive(false);
        drawImage.SetActive(false);
    }

    void Update()
    {
        if (!isGameOver)
        {
            if (isLose)
            {
                loseImage.SetActive(true);
                AudioManager.instance.PlayMusic(loseAndDrawSound);
                isGameOver = true;
            }
            else if (isWin)
            {
                winImage.SetActive(true);
                AudioManager.instance.PlayMusic(winSound);
                isGameOver = true;
            }
            else if (isDraw)
            {
                drawImage.SetActive(true);
                AudioManager.instance.PlayMusic(loseAndDrawSound);
                isGameOver = true;
            }
            else
            {
                time_Sec -= Time.deltaTime;
                timeText_Min.text = "0" + time_Min; //분은 10이상일 경우가 없으므로 0을 항상 앞에 붙임

                if (time_Sec < 10) //초 10이하일경우 0을 앞에 붙임
                {
                    timeText_Sec.text = "0" + (int)time_Sec;
                }
                else //10이하가 아닐경우 앞의 0 제거
                {
                    timeText_Sec.text = "" + (int)time_Sec;
                }

                if (time_Sec <= 0) //초가 0이하로 내려갈경우 분 -1, 초 60으로 다시 되돌림
                {
                    if (time_Sec <= 0 && time_Min == 0) // 분과 초 모두 0이될 경우 무승부
                    {
                        isDraw = true;
                    }
                    time_Min--;
                    time_Sec = 60f;
                }
            }
        }
    } //Update()

    public void NiddleCount(int niddleCount)
    {
        niddleAmount.text = "x" + niddleCount;
    }

    public void CheckPlayerCount()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        playerCount = GameObject.FindGameObjectsWithTag("Player");

        if (playerCount.Length == 1 && playerCount[0] != null)
        {
            CheckPlayerResult(playerCount[0]);
        }
        else if(playerCount.Length == 1 && playerCount[1] != null)
        {
            CheckPlayerResult(playerCount[1]);
        }
    }

    private void CheckPlayerResult(GameObject playerObject)
    {
        PhotonView photonView = playerObject.GetComponent<PhotonView>();

        if (photonView != null)
        {
            if (photonView.IsMine)
            {
                isWin = true;
                isLose = false;
                photonView.RPC("Result", RpcTarget.Others, isLose, isWin);
            }
            else
            {
                isLose = true;
                isWin = false;
                photonView.RPC("Result", RpcTarget.Others, isLose, isWin);
            }
        }
    }

    [PunRPC]
    public void Result(bool isWin, bool isLose)
    {
        this.isWin = isWin;
        this.isLose = isLose;
        Debug.Log("나는 클라이언트야");
    }
}

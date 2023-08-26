//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.SceneManagement;
//using Photon.Pun;
//using Photon.Realtime;

//public class GameManager : MonoBehaviourPunCallbacks
//{
//    public static GameManager instance;
//    public GameObject[] playerCount;

//    private void Awake()
//    {
//        if (instance == null)
//        {
//            instance = this; // 자신을 넣어줌
//            //DontDestroyOnLoad(gameObject); // 씬이 로드되었을때 자신을 파괴하지 않고 유지
//        }
//        else
//        {
//            Debug.LogWarning("씬에 두 개 이상의 게임 매니저가 존재합니다!");
//            //Destroy(gameObject); // 두개이상일때는 나 자신을 삭제
//        }
//    }

//    public GameObject inventory_NiddleImage;
//    public GameObject itemCtrl_NiddleImage;
//    public GameObject loseImage;
//    public GameObject winImage;
//    public GameObject drawImage;
//    public Text timeText_Sec;
//    public Text timeText_Min;
//    public Text niddleAmount;

//    public float remainTime_Sec = 120f; // 남은 시간 2분
//    public bool isLose = false; // 패배 상태
//    public bool isWin = false; // 승리 상태
//    public bool isDraw = false; // 무승부 상태
//    private float time_Sec;
//    private int time_Min;

//    void Start()
//    {
//        //시간 제어
//        time_Sec = 60f;
//        time_Min = (int)(remainTime_Sec / time_Sec);
//        time_Sec = remainTime_Sec % time_Sec;

//        //이미지 false
//        inventory_NiddleImage.SetActive(false);
//        itemCtrl_NiddleImage.SetActive(false);
//        loseImage.SetActive(false);
//        winImage.SetActive(false);
//        drawImage.SetActive(false);
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (isLose) //졌을 경우
//        {
//            loseImage.SetActive(true);
//        }
//        else if (isWin) //이겼을 경우
//        {
//            winImage.SetActive(true);
//        }
//        else if (isDraw) // 비겼을 경우
//        {
//            drawImage.SetActive(true);
//        }
//        else //결과(승리,패배,무승부)가 나오지 않았다면
//        {
//            time_Sec -= Time.deltaTime;
//            timeText_Min.text = "0" + time_Min; //분은 10이상일 경우가 없으므로 0을 항상 앞에 붙임

//            if (time_Sec < 10) //초 10이하일경우 0을 앞에 붙임
//            {
//                timeText_Sec.text = "0" + (int)time_Sec;
//            }
//            else //10이하가 아닐경우 앞의 0 제거
//            {
//                timeText_Sec.text = "" + (int)time_Sec;
//            }

//            if (time_Sec <= 0) //초가 0이하로 내려갈경우 분 -1, 초 60으로 다시 되돌림
//            {
//                if (time_Sec <= 0 && time_Min == 0) // 분과 초 모두 0이될 경우 무승부
//                {
//                    isDraw = true;
//                }
//                time_Min--;
//                time_Sec = 60f;
//            }
//        }
//    }// Update();

//    public void NiddleCount(int niddleCount)
//    {
//        niddleAmount.text = "x" + niddleCount;
//    }

//    public void CheckPlayerCount()
//    {
//        Debug.Log("일단 체크하러는 왔다.");

//        if (!PhotonNetwork.IsMasterClient)
//        {
//            return;
//        }

//        playerCount = GameObject.FindGameObjectsWithTag("Player");
//        Debug.Log(playerCount[0]);
//        Debug.Log(playerCount.Length);

//        if (playerCount.Length == 1 && playerCount[0] != null)
//        {
//            PhotonView photonView = playerCount[0].GetComponent<PhotonView>();
//            if (photonView != null)
//            {
//                if (photonView.IsMine)
//                {
//                    isWin = true;
//                    isLose = false;
//                    photonView.RPC("Result", RpcTarget.Others, isLose, isWin);
//                }
//                else
//                {
//                    isLose = true;
//                    isWin = false;
//                    photonView.RPC("Result", RpcTarget.Others, isLose, isWin);
//                }
//            }

//            //if (playerCount.Length == 1)
//            //{
//            //    if (playerCount[0] != null)
//            //    {
//            //        if (playerCount[0].GetComponent<PhotonView>().IsMine)
//            //        {
//            //            isWin = true;
//            //            isLose = false;
//            //            photonView.RPC("Result", RpcTarget.Others, isLose, isWin);
//            //        }
//            //        else
//            //        {
//            //            isLose = true;
//            //            isWin = false;
//            //            photonView.RPC("Result", RpcTarget.Others,isLose,isWin);
//            //        }
//            //    }
//            //    else
//            //    {
//            //        if (playerCount[1].GetComponent<PhotonView>().IsMine)
//            //        {
//            //            isWin = true;
//            //            isLose = false;
//            //            photonView.RPC("Result", RpcTarget.Others, isLose, isWin);
//            //        }
//            //        else
//            //        {
//            //            isLose = true;
//            //            isWin = false;
//            //            photonView.RPC("Result", RpcTarget.Others, isLose, isWin);
//            //        }
//            //    }
//            //}
//        }
//        else if (playerCount.Length == 1 && playerCount[1] != null)
//        {
//            PhotonView photonView = playerCount[1].GetComponent<PhotonView>();
//            if (photonView != null)
//            {
//                if (photonView.IsMine)
//                {
//                    isWin = true;
//                    isLose = false;
//                    photonView.RPC("Result", RpcTarget.Others, isLose, isWin);
//                }
//                else
//                {
//                    isLose = true;
//                    isWin = false;
//                    photonView.RPC("Result", RpcTarget.Others, isLose, isWin);
//                }
//            }
//        }
//    }
//    //test
//    [PunRPC]
//    public void Result(bool isWin, bool isLose)
//    {
//        this.isWin = isWin;
//        this.isLose = isLose;
//        Debug.Log("나는 클라이언트야");
//    }
//    //test
//}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

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

    //public GameObject inventory_NiddleImage;
    //public GameObject itemCtrl_NiddleImage;
    //public GameObject loseImage;
    //public GameObject winImage;
    //public GameObject drawImage;
    //public Text timeText_Sec;
    //public Text timeText_Min;
    //public Text niddleAmount;

    //public float remainTime_Sec = 120f; // 남은 시간 2분
    //public bool isLose = false; // 패배 상태
    //public bool isWin = false; // 승리 상태
    //public bool isDraw = false; // 무승부 상태
    //private float time_Sec;
    //private int time_Min;

    //void Start()
    //{
    //    // 시간 초기화
    //    time_Sec = 60f;
    //    time_Min = (int)(remainTime_Sec / time_Sec);
    //    time_Sec = remainTime_Sec % time_Sec;

    //    // 이미지 비활성화
    //    inventory_NiddleImage.SetActive(false);
    //    itemCtrl_NiddleImage.SetActive(false);
    //    loseImage.SetActive(false);
    //    winImage.SetActive(false);
    //    drawImage.SetActive(false);
    //}

    //void Update()
    //{
    //    if (isLose)
    //    {
    //        loseImage.SetActive(true);
    //    }
    //    else if (isWin)
    //    {
    //        winImage.SetActive(true);
    //    }
    //    else if (isDraw)
    //    {
    //        drawImage.SetActive(true);
    //    }
    //    else
    //    {
    //        time_Sec -= Time.deltaTime;
    //        timeText_Min.text = "0" + time_Min; //분은 10이상일 경우가 없으므로 0을 항상 앞에 붙임

    //        if (time_Sec < 10) //초 10이하일경우 0을 앞에 붙임
    //        {
    //            timeText_Sec.text = "0" + (int)time_Sec;
    //        }
    //        else //10이하가 아닐경우 앞의 0 제거
    //        {
    //            timeText_Sec.text = "" + (int)time_Sec;
    //        }

    //        if (time_Sec <= 0) //초가 0이하로 내려갈경우 분 -1, 초 60으로 다시 되돌림
    //        {
    //            if (time_Sec <= 0 && time_Min == 0) // 분과 초 모두 0이될 경우 무승부
    //            {
    //                isDraw = true;
    //            }
    //            time_Min--;
    //            time_Sec = 60f;
    //        }
    //    }
    //}

    //public void NiddleCount(int niddleCount)
    //{
    //    niddleAmount.text = "x" + niddleCount;
    //}

    //public void CheckPlayerCount()
    //{
    //    if (!PhotonNetwork.IsMasterClient)
    //    {
    //        return;
    //    }

    //    playerCount = GameObject.FindGameObjectsWithTag("Player");

    //    if (playerCount.Length >= 1 && playerCount[0] != null)
    //    {
    //        CheckPlayerResult(playerCount[0]);
    //    }

    //    if (playerCount.Length >= 2 && playerCount[1] != null)
    //    {
    //        CheckPlayerResult(playerCount[1]);
    //    }
    //}

    //private void CheckPlayerResult(GameObject playerObject)
    //{
    //    PhotonView photonView = playerObject.GetComponent<PhotonView>();

    //    if (photonView != null)
    //    {
    //        if (photonView.IsMine)
    //        {
    //            isWin = true;
    //            isLose = false;
    //            photonView.RPC("Result", RpcTarget.Others, isLose, isWin);
    //        }
    //        else
    //        {
    //            isLose = true;
    //            isWin = false;
    //            photonView.RPC("Result", RpcTarget.Others, isLose, isWin);
    //        }
    //    }
    //}

    //[PunRPC]
    //public void Result(bool isWin, bool isLose)
    //{
    //    this.isWin = isWin;
    //    this.isLose = isLose;
    //    Debug.Log("나는 클라이언트야");
    //}
}

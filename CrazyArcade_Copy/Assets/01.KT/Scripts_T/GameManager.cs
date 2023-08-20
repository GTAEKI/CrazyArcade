using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;



public class GameManager : MonoBehaviour
{
    //public GameObject loseImage;
    //public GameObject winImage;
    public Text timeText_Sec;
    public Text timeText_Min;

    public float remainTime_Sec = 120f; //남은 시간 2분
    private bool isLose = false; // 패배 상태
    private bool isWin = false; // 승리 상태
    private bool isDraw = false; // 무승부 상태
    private float time_Sec;
    private int time_Min;

    //private void Awake()
    //{
    //    CreatePlayer();
    //}

    // Start is called before the first frame update
    void Start()
    {
        //시간 제어
        time_Sec = 60f;
        time_Min = (int)(remainTime_Sec / time_Sec);
        time_Sec = remainTime_Sec % time_Sec;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLose && !isWin && !isDraw) //결과(승리,패배,무승부)가 나오지 않았다면
        {
            time_Sec -= Time.deltaTime;
            timeText_Min.text = "0" + time_Min; //분은 10이상일 경우가 없으므로 0을 항상 앞에 붙임
            if (time_Sec < 10) //초 10이하일경우 0을 앞에 붙임
            {
                timeText_Sec.text = "0" + (int)time_Sec;
            }
            else//10이하가 아닐경우 앞의 0 제거
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
    }// Update();

    //void CreatePlayer()
    //{
    //    // 출현 위치 정보를 배열에 저장
    //    Transform[] points = GameObject.Find("CharactorRespawnController_J").GetComponents<Transform>();
    //    int idx = Random.Range(1, points.Length);

    //    // 네트워크상에 캐릭터 생성
    //    PhotonNetwork.Instantiate("PlayerBazzi", points[idx].position, points[idx].rotation, 0);

    //}
}

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorRespawnController_J : MonoBehaviour
{
    //// 생성할 플레이어오브젝트
    //public GameObject player;

    // 리스폰포인트 오브젝트들 저장
    private GameObject[] charactorRespawnPoints;

    // CharactorRespawnPoint중 랜덤으로 설치 위치를 설정하는데 필요한 변수
    private int randomIndex;
    private List<int> exceptNumbers = new List<int>();
    private bool goContinue;

    void Awake()
    {
        // 하이어라키창의 캐릭터리스폰 포인트들을 배열에 저장
        charactorRespawnPoints = GameObject.FindGameObjectsWithTag("CharactorRespawnPoint");

        // 플레이어 생성
        CreatePlayer();
    } //Awake()

    public void CreatePlayer()
    {
        for (int i = 0; i < 1; i++)
        {

            //랜덤한 인덱스번호 >> 랜덤한 위치로 리스폰
            randomIndex = Random.Range(0, charactorRespawnPoints.Length);

            if (exceptNumbers.Count != 0) //제외할 인덱스넘버가 비어있지 않다면
            {
                foreach (int exceptNumber in exceptNumbers) //인덱스번호를 하나씩 꺼내서 랜덤으로 뽑힌 번호와 비교
                {
                    if (exceptNumber == randomIndex) //랜덤 인덱스번호가 제외된 번호와 같다면 for문 반복실행
                    {
                        i--;
                        goContinue = true;
                    }
                }

                // 랜덤 인덱스번호가 제외된 번호와 같다면 for문 반복실행
                if (goContinue)
                {
                    goContinue = false;
                    continue;
                }
                else { exceptNumbers.Add(randomIndex); } //같지 않다면 현재 랜덤인덱스 번호를 제외번호에 추가
            }
            else { exceptNumbers.Add(randomIndex); }//같지 않다면 현재 랜덤인덱스 번호를 제외번호에 추가

            // 플레이어 생성
            PhotonNetwork.Instantiate("PlayerBazzi", charactorRespawnPoints[randomIndex].transform.position, Quaternion.identity);

            //void CreatePlayer()
            //{
            //    // 출현 위치 정보를 배열에 저장
            //    Transform[] points = GameObject.Find("CharactorRespawnController_J").GetComponents<Transform>();
            //    int idx = Random.Range(1, points.Length);

            //    // 네트워크상에 캐릭터 생성
            //    PhotonNetwork.Instantiate("PlayerBazzi", points[idx].position, points[idx].rotation, 0);

            //}

        }//for
    }//CreatePlayer()
} // Class CharactorRespawnController

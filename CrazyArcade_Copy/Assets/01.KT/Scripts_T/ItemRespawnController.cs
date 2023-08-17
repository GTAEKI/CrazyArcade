using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRespawnController : MonoBehaviour
{
    // 생성된 아이템을 넣어놓을 오브젝트
    public GameObject inGameItems = default;

    //생성할 아이템의 개수를 설정
    public int amount_BalloonItem = 7;
    public int amount_SmallPowerPotion = 3;
    public int amount_BigPowerPotion = 3;
    public int amount_SpeedItem = 3; 
    public int amount_Niddle = 10;
    public int amount_Shoe = 10;


    //생성할 아이템 프리펩
    public GameObject balloonItem;
    public GameObject smallPowerPotion;
    public GameObject bigPowerPotion;
    public GameObject speedItem;
    public GameObject niddle;
    public GameObject shoe;

    // ItemRespawnPoint중 랜덤으로 설치 위치를 설정하는데 필요한 변수
    private int randomIndex;
    private List<int> exceptNumbers = new List<int>();
    private bool goContinue;

    private GameObject[] itemRespawnPoints;

    void Awake()
    {
        // 하이어라키창 안에있는 ItemRespawnPoint를 찾아서 배열에 저장
        itemRespawnPoints = GameObject.FindGameObjectsWithTag("ItemRespawnPoint");

        CreateRandomItems(balloonItem, amount_BalloonItem);
        CreateRandomItems(smallPowerPotion, amount_SmallPowerPotion);
        CreateRandomItems(bigPowerPotion, amount_BigPowerPotion);
        CreateRandomItems(speedItem, amount_SpeedItem);
        CreateRandomItems(niddle, amount_Niddle);
        CreateRandomItems(shoe, amount_Shoe);

    }//Awake()

    //생성할 아이템과 갯수를 삽입
    private void CreateRandomItems(GameObject itemPrefab, int Itemamount)
    {
        for (int i = 0; i < Itemamount; i++) //생성할 아이템갯수만큼 반복
        {
            randomIndex = Random.Range(0, itemRespawnPoints.Length); //랜덤위치 뽑기

            if (exceptNumbers.Count != 0) //제외된 숫자 List가 비어있지 않다면
            {
                foreach (int exceptNumber in exceptNumbers) //제외된 숫자를 하나씩 꺼내서 randomIndex와 비교
                {
                    if (exceptNumber == randomIndex)//같은 숫자가 있다면 다시 for문 반복
                    {
                        i--;
                        goContinue = true;
                    }
                }

                // 제외된 숫자와 같은 숫자가 있다면 다시 for문 반복
                if (goContinue)
                {
                    goContinue = false;
                    continue;
                }
                else { exceptNumbers.Add(randomIndex); } //없을 경우 랜덤인덱스 제외된숫자에 추가
            }
            else { exceptNumbers.Add(randomIndex); }//없을 경우 랜덤인덱스 제외된숫자에 추가

            //아이템 생성
            GameObject createdItem =
            Instantiate(itemPrefab, itemRespawnPoints[randomIndex].transform.position, Quaternion.identity, inGameItems.transform);


            //아이템 스케일 UP
            createdItem.transform.localScale *= 1.5f;
        }//for
    }//CreateRandomItems()
} // Class ItemRespawnController

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRespawnController : MonoBehaviour
{
    // 생성된 아이템을 넣어놓을 오브젝트
    public GameObject inGameItems = default;

    //생성할 아이템의 개수를 설정
    public int Amount_BalloonItem = 7;
    public int Amount_SmallPowerPotion = 3;
    public int Amount_BigPowerPotion = 3;
    public int Amount_SpeedItem = 3; 
    public int Amount_Niddle = 10;


    //생성할 
    public GameObject BalloonItem;
    public GameObject SmallPowerPotion;
    public GameObject BigPowerPotion;
    public GameObject SpeedItem;
    public GameObject Niddle;

    // ItemRespawnPoint중 랜덤으로 설치 위치를 설정하는데 필요한 변수
    private int RandomIndex;
    private List<int> ExceptNumbers = new List<int>();
    private bool goContinue;

    private GameObject[] itemRespawnPoints;

    void Awake()
    {
        // 하이어라키창 안에있는 ItemRespawnPoint를 찾아서 배열에 저장
        itemRespawnPoints = GameObject.FindGameObjectsWithTag("ItemRespawnPoint");

        CreateRandomItems(BalloonItem, Amount_BalloonItem);
        CreateRandomItems(SmallPowerPotion, Amount_SmallPowerPotion);
        CreateRandomItems(BigPowerPotion, Amount_BigPowerPotion);
        CreateRandomItems(SpeedItem, Amount_SpeedItem);
        CreateRandomItems(Niddle, Amount_Niddle);

    }//Awake()

    private void CreateRandomItems(GameObject itemPrefab, int ItemAmount)
    {
        for (int i = 0; i < ItemAmount; i++)
        {
            RandomIndex = Random.Range(0, itemRespawnPoints.Length);

            if (ExceptNumbers.Count != 0) //ExceptNumbers가 비어있지 않다면
            {
                foreach (int ExceptNumber in ExceptNumbers)
                {
                    if (ExceptNumber == RandomIndex)
                    {
                        i--;
                        goContinue = true;
                    }
                }

                // 제외된 숫자와 랜덤숫자가 같다면 Continue를 통해 For문을 다시 실행
                if (goContinue)
                {
                    goContinue = false;
                    continue;
                }
                else { ExceptNumbers.Add(RandomIndex); }
            }
            else { ExceptNumbers.Add(RandomIndex); }

            GameObject CreatedItem =
            Instantiate(itemPrefab, itemRespawnPoints[RandomIndex].transform.position, Quaternion.identity, inGameItems.transform);
            CreatedItem.transform.localScale *= 1.5f;
        }//for
    }//CreateRandomItems()
}

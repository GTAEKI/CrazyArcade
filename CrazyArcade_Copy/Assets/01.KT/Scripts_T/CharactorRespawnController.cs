using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorRespawnController : MonoBehaviour
{
    public GameObject Player;

    private GameObject[] CharactorRespawnPoints;

    // CharactorRespawnPoint중 랜덤으로 설치 위치를 설정하는데 필요한 변수
    private int RandomIndex;
    private List<int> ExceptNumbers = new List<int>();
    private bool goContinue;

    void Awake()
    {
        CharactorRespawnPoints = GameObject.FindGameObjectsWithTag("CharactorRespawnPoint");
        CreatePlayer(Player,1);
    }

    private void CreatePlayer(GameObject player, int playerAmount)
    {
        for (int i = 0; i < playerAmount; i++)
        {
            RandomIndex = Random.Range(0, CharactorRespawnPoints.Length);

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

            GameObject CreatedPlayer =
            Instantiate(player, CharactorRespawnPoints[RandomIndex].transform.position, Quaternion.identity);
        }//for
    }//CreatePlayer()

}

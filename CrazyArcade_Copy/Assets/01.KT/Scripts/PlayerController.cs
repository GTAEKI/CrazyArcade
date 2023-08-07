using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRB;
    private int waterBalloonCount = 1;

    private int niddleCount = 0;
    private bool isStuckWater = false;
    public float speed = 4.0f;
    public GameObject waterBalloon;

    public GameObject[] waterBalloons;

    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
    }//Start()

    void Update()
    {
        //Implement player movement using GetAxisRaw
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        playerRB.velocity = new Vector2(speed * horizontal, speed * vertical);

        // { 물풍선 설치 개수 제한
        // waterBalloons배열에 하이어라키에 있는 WaterBalloon을 넣어줌
        waterBalloons = GameObject.FindGameObjectsWithTag("WaterBalloon");

        //waterBalloons의 개수를 체크하여 설치 가능한 숫자와 비교함
        if(waterBalloons.Length < waterBalloonCount)
        {
            //Press the spacebar to create a water balloon
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Instantiate(waterBalloon, transform.position, Quaternion.identity);
            }
        }
        // } 물풍선 설치 개수 제한

        // 바늘 아이템 사용시
        if (Input.GetKeyDown(KeyCode.Alpha1) && isStuckWater && niddleCount != 0)
        {
            Debug.Log("바늘 아이템 사용");
            niddleCount--;
            isStuckWater = false;
            //TODO 물풍선 빠져나오는 애니메이션
        }

        // {임의로 물에 갇힌 상황 표현
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            isStuckWater = true; 
        }
        // } 임의로 물에 갇힌 상황 표현


    }//Update()

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "SpeedItem") // 스피드아이템일 경우
        {
            Destroy(collision.gameObject);
            speed += 5.0f;
        }
        else if(collision.tag == "BalloonItem") // 풍선 아이템일 경우
        {
            Destroy(collision.gameObject);
            waterBalloonCount += 1;

        }
        else if(collision.tag == "SmallPowerPotion") // 작은 파워업아이템일 경우
        {
            Destroy(collision.gameObject);
            //TODO 파워 +1;
        }
        else if(collision.tag == "BigPowerPotion") // 큰 파워업 아이템일 경우
        {
            Destroy(collision.gameObject);
            //TODO 파워 MAX;

        }
        else if(collision.tag == "Niddle") // 바늘일 경우
        {
            Destroy(collision.gameObject);
            niddleCount++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.isTrigger = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Tile") //만난 오브젝트가 타일일 경우
        {
            // TODO움직이는 함수 추가
        }
    }

    private void StuckWaterBalloon()
    {
        //TODO 물방울 갇혔을때 함수
        isStuckWater = true;
    }

    private void Die()
    {
        //TODO 죽었을때 함수, 다이 애니메이션 추가
    }

    private void UsingNiddle()
    {
        //바늘 사용 함수
    }

}

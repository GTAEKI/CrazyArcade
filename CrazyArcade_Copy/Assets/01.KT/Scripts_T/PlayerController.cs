using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRB;
    private Animator animator;
    private int waterBalloonCount = 1;

    private int niddleCount = 0;
    private bool isDead = false;
    private bool isStuckWater = false;
    public float speed = 4.0f;
    public float stuckSpeed = 0.5f;
    private float remainSpeed = default;

    public float power = 0.66666f;
    private float maxPower = 3.99996f;

    // 물풍선 변수
    public GameObject waterBalloon;
    public GameObject[] waterBalloons;

    // 먹은 아이템 저장하는 변수
    public List<GameObject> saveGetItem;

    // 시간 체크하는 변수 (사망처리에 사용)
    private float time = 0f;
    private float setTime = 6f;

    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        remainSpeed = speed; //최초 속도 저장

    }//Start()

    void Update()
    {
        if (!isDead)
        {
            //Implement player movement using GetAxisRaw
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            playerRB.velocity = new Vector2(speed * horizontal, speed * vertical);

            animator.SetInteger("Horizontal", (int)horizontal);
            animator.SetInteger("Vertical", (int)vertical);
        }


        // { 물풍선 설치 개수 제한
        // waterBalloons배열에 하이어라키에 있는 WaterBalloon을 넣어줌
        waterBalloons = GameObject.FindGameObjectsWithTag("WaterBalloon");

        //waterBalloons의 개수를 체크하여 설치 가능한 숫자와 비교함
        if(waterBalloons.Length < waterBalloonCount)
        {
            //Press the spacebar to create a water balloon
            if (Input.GetKeyDown(KeyCode.Space) && !isStuckWater)
            {
                Vector2 waterBalloonPosition = new Vector2(transform.position.x, transform.position.y - 0.2f);
                Instantiate(waterBalloon, waterBalloonPosition, Quaternion.identity);
            }
        }
        // } 물풍선 설치 개수 제한

        // 바늘 아이템 사용시
        if (Input.GetKeyDown(KeyCode.Alpha1) && isStuckWater && niddleCount != 0)
        {
            UsingNiddle();
        }

        // 물풍선에 갇힌 상황이라면 일정시간 뒤 죽음
        if (isStuckWater)
        {
            //시간 더해주기
            time += Time.deltaTime;

            //일정 시간 지날경우 Die함수 실행
            if (time > setTime)
            {
                Die();
            }
        }
    }//Update()

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 죽거나 물에 갇히면 아래 행동을 못함(아이템 먹기, 물풍선에 맞기)
        if(!isDead && !isStuckWater)
        {
            if(collision.tag == "SpeedItem") // 스피드아이템일 경우
            {
                saveGetItem.Add(collision.gameObject);

                collision.gameObject.SetActive(false);
                //Destroy(collision.gameObject);
                speed += 1.0f;
                remainSpeed = speed;
            }
            else if(collision.tag == "BalloonItem") // 풍선 아이템일 경우
            {
                Destroy(collision.gameObject);
                waterBalloonCount += 1;
            }
            else if(collision.tag == "SmallPowerPotion") // 작은 파워업아이템일 경우
            {
                if(power < maxPower) //Max파워를 넘지 못하도록 조정
                {
                    power += 0.66666f;
                }

                Destroy(collision.gameObject);
            }
            else if(collision.tag == "BigPowerPotion") // 큰 파워업 아이템일 경우
            {
                power = maxPower;
                Destroy(collision.gameObject);
            }
            else if(collision.tag == "Niddle") // 바늘일 경우
            {
                Destroy(collision.gameObject);
                niddleCount++;
            }
            else if(collision.tag == "WaterExplosion")
            {
                StuckWaterBalloon();
            }
            else if (collision.tag == "ShoeItem") // 신발 아이템일 경우
            {
                Destroy(collision.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "WaterBalloon")
        {
            collision.isTrigger = false;
        }
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
        isStuckWater = true;
        animator.SetBool("StuckWater", isStuckWater);
        animator.SetTrigger("StuckTrigger");
        speed = stuckSpeed;
    }

    private void Die()
    {
        isDead = true;
        playerRB.velocity = Vector2.zero;
        animator.SetTrigger("Die");

        foreach(GameObject item in saveGetItem)
        {
            item.transform.position = transform.position + new Vector3(3,0,0);
            item.gameObject.SetActive(true);
        }

        //TODO 죽었을때 lose, 상대방이 죽을경우 Win
    }

    private void UsingNiddle()
    {
        niddleCount--;
        isStuckWater = false;
        animator.SetBool("StuckWater", isStuckWater);
        speed = remainSpeed;
    }

}

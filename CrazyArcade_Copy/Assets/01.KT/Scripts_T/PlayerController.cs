using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun
{
    private Rigidbody2D playerRB;
    private Animator animator;
    private int waterBalloonCount = 1;

    private PhotonView pv;

    private int niddleCount = 0;
    private bool isStuckWater = false;
    public float speed = 4.0f;
    public float stuckSpeed = 0.5f;
    private float remainSpeed = default;

    public int power = 1;

    private int maxPower = 6;
    public GameObject waterBalloon;

    public GameObject[] waterBalloons;

    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        remainSpeed = speed; //최초 속도 저장

    }//Start()

    void Update()
    {
        if (!photonView.IsMine) { return; }

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
    }//Update()

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        //Implement player movement using GetAxisRaw
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        playerRB.velocity = new Vector2(speed * horizontal, speed * vertical);

        animator.SetInteger("Horizontal", (int)horizontal);
        animator.SetInteger("Vertical", (int)vertical);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "SpeedItem") // 스피드아이템일 경우
        {
            Destroy(collision.gameObject);
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
            power += 1;
            Destroy(collision.gameObject);
            //TODO 파워 +1;
        }
        else if(collision.tag == "BigPowerPotion") // 큰 파워업 아이템일 경우
        {
            power = maxPower;
            Destroy(collision.gameObject);
            //TODO 파워 MAX;

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
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "WaterBalloon")
        {
            collision.isTrigger = false;
        }

        if(collision.tag == "tile")
        {
            collision.gameObject.GetComponent<SpriteRenderer>().color = Color.clear; 
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
        //TODO 물방울 갇혔을때 함수
        isStuckWater = true;
        animator.SetBool("StuckWater", isStuckWater);
        animator.SetTrigger("StuckTrigger");
        speed = stuckSpeed;
    }

    private void Die()
    {
        //TODO 죽었을때 함수, 다이 애니메이션 추가
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

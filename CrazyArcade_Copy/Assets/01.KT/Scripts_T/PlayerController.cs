using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks
{
    // 변경시킬 Rigidbody와 animator
    private Rigidbody2D playerRB;
    private Animator animator;

    //플레이어 기본 능력치
    private int waterBalloonCount = 2;

    private int niddleCount = 0;
    public float speed = 4.0f;
    public bool onShoe = false;
    public float power = 0.66666f;

    // bool값 및 제한사항
    private bool isDead = false;
    private bool isStuckWater = false;
    public float stuckSpeed = 0.2f; //물풍선 갇혔을때 이동속도
    private float maxPower = 3.99996f; //최대 파워
    private float remainSpeed = default; //물풍선에서 바늘을 사용해서 나왔을때를 위해 속도 저장변수

    // 물풍선 변수
    public GameObject waterBalloon;
    public GameObject[] waterBalloons;

    // 먹은 아이템 저장하는 변수
    public List<GameObject> saveGetItem;

    // 시간 체크하는 변수 (사망처리에 사용)
    private float time = 0f;
    private float setTime = 6f;


    // 물풍선 중복방지 변수
    public float range = 0.34f;
    private bool isWaterBalloon = false;

    //오버랩 변수
    public Vector2 boxSize = new Vector2(0.67f, 0.67f);
    //// DEBUG:
    //private AudioSource testAudio = default;
    //[Header("For test")]
    //[Space(2)]
    //public AudioClip bumbBalloonClip;   // 풍선 폭발 소리

    //// DEBUG:
    //private void Awake()
    //{
    //    testAudio = Camera.main.GetComponent<AudioSource>();
    //}

    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        remainSpeed = speed; //최초 속도 저장

        //// 포톤뷰 컴포넌트 연결
        //pv = GetComponent<PhotonView>();

    }//Start()

    void Update()
    {
        //if (!isDead)
        //{
        //    //Implement player movement using GetAxisRaw
        //    float horizontal = Input.GetAxisRaw("Horizontal");
        //    float vertical = Input.GetAxisRaw("Vertical");
        //    playerRB.velocity = new Vector2(speed * horizontal, speed * vertical);

        //    animator.SetInteger("Horizontal", (int)horizontal);
        //    animator.SetInteger("Vertical", (int)vertical);
        //}

        // 로컬 아니면 return
        if (!photonView.IsMine) { return; }

        // { 물풍선 설치 개수 제한
        // waterBalloons배열에 하이어라키에 있는 WaterBalloon을 넣어줌
        waterBalloons = GameObject.FindGameObjectsWithTag("WaterBalloon");

        // 로컬 유저일 때만 물풍선 설치
        if (photonView.IsMine && Input.GetKeyDown(KeyCode.Space))
        {
            //스페이스바를 눌러 물풍선 생성
            if (Input.GetKeyDown(KeyCode.Space) && !isStuckWater)
            {
                Vector2 m_tr_Vector2 = new Vector2(transform.position.x, transform.position.y);
                //캐릭터가 물풍선 중복해서 놓을수 없도록 체크하는 범위
                Collider2D[] cols = Physics2D.OverlapBoxAll(m_tr_Vector2, boxSize * 0.9f, 0);

                foreach (Collider2D col in cols)
                {
                    if (col.tag == "WaterBalloon")
                    {
                        isWaterBalloon = true;
                        Debug.Log("생성불가");
                    }
                }

                if (!isWaterBalloon)
                {
                    Vector2 waterBalloonPosition = new Vector2(transform.position.x, transform.position.y - 0.2f);
                    Instantiate(waterBalloon, waterBalloonPosition, Quaternion.identity);
                }
                isWaterBalloon = false;
            }
            //// DEBUG:
            //testAudio.PlayOneShot(bumbBalloonClip);

            PutBalloon();

            // RPC로 원격지에 있는 함수 호출
            // 호출하지 않으면 상대방이 설치한 물풍선 동기화가 안됨 
            photonView.RPC("PutBalloon", RpcTarget.Others, null);
        }

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

    // 물풍선 설치 함수 
    [PunRPC]
    private void PutBalloon()
    {
        // { 물풍선 설치 개수 제한
        // waterBalloons의 개수를 체크하여 설치 가능한 숫자와 비교함
        if (waterBalloons.Length < waterBalloonCount)
        {
            //Press the spacebar to create a water balloon
            if (!isStuckWater)
            {
                Vector2 waterBalloonPosition = new Vector2(transform.position.x, transform.position.y - 0.2f);
                Instantiate(waterBalloon, waterBalloonPosition, Quaternion.identity);
            }
        }
        // } 물풍선 설치 개수 제한
    }

    private void FixedUpdate()
    {
        //if (!photonView.IsMine) { return; }
     
        if (photonView.IsMine && !isDead)
        { 
            Move();
        }
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
        if (!photonView.IsMine) { return; }
        // 죽거나 물에 갇히면 아래 행동을 못함(아이템 먹기, 물풍선에 맞기)
        if (!isDead && !isStuckWater)
        {
            if (collision.tag == "SpeedItem") // 스피드아이템일 경우
            {
                AudioSource eatItemSound = GetComponent<AudioSource>();
                eatItemSound.Play();

                saveGetItem.Add(collision.gameObject);
                collision.gameObject.SetActive(false);
                //Destroy(collision.gameObject);
                speed += 1.0f;
                remainSpeed = speed;
            }
            else if (collision.tag == "BalloonItem") // 풍선 아이템일 경우
            {
                AudioSource eatItemSound = GetComponent<AudioSource>();
                eatItemSound.Play();

                saveGetItem.Add(collision.gameObject);
                collision.gameObject.SetActive(false);

                waterBalloonCount += 1;
            }
            else if (collision.tag == "SmallPowerPotion") // 작은 파워업아이템일 경우
            {
                AudioSource eatItemSound = GetComponent<AudioSource>();
                eatItemSound.Play();

                saveGetItem.Add(collision.gameObject);
                collision.gameObject.SetActive(false);

                if (power < maxPower) //Max파워를 넘지 못하도록 조정
                {
                    power += 0.66666f;
                }
            }
            else if (collision.tag == "BigPowerPotion") // 큰 파워업 아이템일 경우
            {
                AudioSource eatItemSound = GetComponent<AudioSource>();
                eatItemSound.Play();

                saveGetItem.Add(collision.gameObject);
                collision.gameObject.SetActive(false);

                power = maxPower;
            }
            else if (collision.tag == "Niddle") // 바늘일 경우
            {
                AudioSource eatItemSound = GetComponent<AudioSource>();
                eatItemSound.Play();

                saveGetItem.Add(collision.gameObject);
                collision.gameObject.SetActive(false);

                niddleCount++;
            }
            else if (collision.tag == "ShoeItem") // 신발 아이템일 경우
            {
                AudioSource eatItemSound = GetComponent<AudioSource>();
                eatItemSound.Play();

                onShoe = true;
                collision.gameObject.SetActive(false);
            }
        }
    } // OnTriggerEnter2D()

    // 물풍선 위에 서있다가 내려갈경우 Trigger를 false
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "WaterBalloon")
        {
            collision.isTrigger = false;
        }
    } //OnTriggerExit2D()

    //물에 갇히는 함수
    public void StuckWaterBalloon()
    {
        isStuckWater = true;
        animator.SetBool("StuckWater", isStuckWater);
        animator.SetTrigger("StuckTrigger");
        speed = stuckSpeed;
    } // StuckWaterBalloon()

    // 죽는 함수
    private void Die()
    {
        playerRB.velocity = Vector2.zero; //속도 0으로 조정
        animator.SetTrigger("Die"); //Die 애니메이션 실행

        if (!isDead) //1번 생성하고 더이상 실행하지 않음
        {
            foreach (GameObject item in saveGetItem)
            {
                float x = Random.Range(-3f, 3f);
                float y = Random.Range(-3f, 3f);

                item.transform.position = transform.position + new Vector3(x, y, 0);
                item.gameObject.SetActive(true);
            }
        }
        isDead = true;
        //TODO 내가 죽었을때 상대방이 살아있다면, 내 모니터에는 Lose / 상대방이 먼저 죽었다면 Win
    } // Die()

    // 바늘 사용 함수
    private void UsingNiddle()
    {
        niddleCount--;
        isStuckWater = false;
        animator.SetBool("StuckWater", isStuckWater);
        speed = remainSpeed;
    } // UsingNiddle()
} // Class PlayerController

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPunCallbacks
{
    // 변경시킬 Rigidbody와 animator
    private Rigidbody2D playerRB;
    private Animator animator;
    private CircleCollider2D playerCD;
    
    //플레이어 기본 능력치
    private int waterBalloonCount = 1;
    private int niddleCount = 0;
    public float speed = 3.0f;
    public bool onShoe = false;
    public float power = 0.7f;

    // bool값 및 제한사항
    private bool isDead = false;
    public bool isStuckWater = false; 
    public float stuckSpeed = 0.2f; //물풍선 갇혔을때 이동속도
    public float maxPower = 3.5f; //최대 파워
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
    private bool isWaterBalloon = false;

    //오버랩 변수
    public Vector2 boxSize = new Vector2(0.67f, 0.67f);

    //캐릭터 머리위 화살표 표시 변수
    [SerializeField]
    private Sprite[] arrows;

    public SpriteRenderer BazziArrowSprite;
    private PhotonView pv;
    private GameObject gameResult;

    //AudioClip
    public AudioClip niddlePopSound;
    public AudioClip eatItemSound;
    public AudioClip dieSound;
    public AudioClip stuckBubbleSound;

    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerCD = GetComponent<CircleCollider2D>();
        gameResult = GameObject.Find("GameResult");
        remainSpeed = speed; //최초 속도 저장

        // 포톤뷰 컴포넌트 연결
        pv = GetComponent<PhotonView>();

        if (pv.IsMine)
        {
            BazziArrowSprite.sprite = arrows[0];
            Debug.Log(BazziArrowSprite.sprite.name);
        }
        else
        {
            BazziArrowSprite.sprite = arrows[1];
            Debug.Log(BazziArrowSprite.sprite.name);
        }
    }//Start()

    void Update()
    {
        // waterBalloons배열에 하이어라키에 있는 WaterBalloon을 넣어줌
        waterBalloons = GameObject.FindGameObjectsWithTag("WaterBalloon");

        //스페이스바를 눌러 물풍선 생성
        if (pv.IsMine && Input.GetKeyDown(KeyCode.Space) && !isStuckWater)
        {
            //PutBalloon();
            photonView.RPC("PutBalloon", RpcTarget.All, null);
        }

        // 바늘 아이템 사용시
        if (Input.GetKeyDown(KeyCode.LeftControl) && isStuckWater && niddleCount != 0)
        {
            UsingNiddle();
        }

        // 물풍선에 갇힌 상황이라면 일정시간 뒤 죽음
        if (isStuckWater)
        {
            //시간 더해주기
            time += Time.deltaTime;

            //일정 시간 지나거나 죽지 않았을 경우 Die함수 실행
            if (time > setTime && !isDead)
            {
                Die();
            }
        }
        
    }//Update()

    // 물풍선 설치 함수 
    [PunRPC]
    private void PutBalloon()
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
            int putWaterCount = 0;
            foreach(GameObject myWaterNumber in waterBalloons)
            {
                if(myWaterNumber.GetComponent<WaterBalloonController>().actorNumber == pv.Owner.ActorNumber)
                {
                    putWaterCount++;
                }
            }
            // { 물풍선 설치 개수 제한
            //waterBalloons의 개수를 체크하여 설치 가능한 숫자와 비교함
            if (putWaterCount < waterBalloonCount)
            {
                putWaterCount = 0;
                //Press the spacebar to create a water balloon
                if (!isStuckWater)
                {
                    Vector2 waterBalloonPosition = new Vector2(transform.position.x, transform.position.y - 0.2f);
                    if (PhotonNetwork.IsMasterClient)
                    {
                        GameObject myWaterBalloon =PhotonNetwork.Instantiate("WaterBalloon", waterBalloonPosition, Quaternion.identity);
                        myWaterBalloon.GetComponent<WaterBalloonController>().actorNumber = pv.Owner.ActorNumber;
                    }
                    //유저 고유번호 저장
                }
            }
            // } 물풍선 설치 개수 제한
        }
        isWaterBalloon = false;
    }

    private void FixedUpdate()
    {
        if (!pv.IsMine) { return; }
        if (!isDead)
        {
            Move();
        }
    }

    private void Move()
    {
        float horizontal = 0;
        float vertical = 0;

        // 대각선방향으로 곧바로 움직이지 못하도록 if를 두번 사용함
        if(horizontal == 0)
        {
            vertical = Input.GetAxisRaw("Vertical");
        }
        if(vertical == 0)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
        }


        playerRB.velocity = new Vector2(speed * horizontal, speed * vertical);

        animator.SetInteger("Horizontal", (int)horizontal);
        animator.SetInteger("Vertical", (int)vertical);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 죽거나 물에 갇히면 아래 행동을 못함(아이템 먹기, 물풍선에 맞기)
        if(!isDead && !isStuckWater)
        {
            if(collision.tag == "SpeedItem") // 스피드아이템일 경우
            {
                GetItem(collision);

                speed += 0.4f;
                remainSpeed = speed;
            }
            else if(collision.tag == "BalloonItem") // 풍선 아이템일 경우
            {
                GetItem(collision);
                waterBalloonCount += 1;
            }
            else if(collision.tag == "SmallPowerPotion") // 작은 파워업아이템일 경우
            {
                GetItem(collision);

                if (power < maxPower) //Max파워를 넘지 못하도록 조정
                {
                    power += 0.7f;
                }
            }
            else if(collision.tag == "BigPowerPotion") // 큰 파워업 아이템일 경우
            {
                GetItem(collision);
                power = maxPower;
            }
            else if(collision.tag == "Niddle") // 바늘일 경우
            {
                GetItem(collision);
                gameResult.GetComponent<GameResult>().inventory_NiddleImage.SetActive(true);
                gameResult.GetComponent<GameResult>().itemCtrl_NiddleImage.SetActive(true);
                //GameManager.instance.inventory_NiddleImage.SetActive(true);
                //GameManager.instance.itemCtrl_NiddleImage.SetActive(true);
                CountCheckNiddle(1);
            }
            else if (collision.tag == "ShoeItem") // 신발 아이템일 경우
            {
                GetItem(collision);
                onShoe = true;
            }
            else if(collision.tag == "Player" && collision.GetComponent<PlayerController>().isStuckWater)
            {
                Debug.Log("터뜨린다");
                collision.GetComponent<PlayerController>().Die();    
            }
        }
    } // OnTriggerEnter2D()

    // 아이템 얻을경우
    private void GetItem(Collider2D collision)
    {
        AudioManager.instance.PlayOneShot(eatItemSound);
        saveGetItem.Add(collision.gameObject);
        collision.gameObject.SetActive(false);
    }//GetItem()

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
        AudioManager.instance.PlayOneShot(stuckBubbleSound);
    } // StuckWaterBalloon()

    // 죽는 함수
    private void Die()
    {
        AudioManager.instance.PlayOneShot(dieSound);
        playerRB.velocity = Vector2.zero; //속도 0으로 조정
        animator.SetTrigger("Die"); //Die 애니메이션 실행

        if (!isDead) //1번 생성하고 더이상 실행하지 않음
        {
            foreach (GameObject item in saveGetItem)
            {
                Debug.Log(item.name);
                float x = Random.Range(-2f, 2f);
                float y = Random.Range(-2f, 2f);

                item.transform.position = transform.position + new Vector3(x, y, 0);
                item.gameObject.SetActive(true);
            }
        }
        //test
        gameObject.SetActive(false);
        gameResult.GetComponent<GameResult>().CheckPlayerCount(); //죽을경우 플레이어를 셈

        //StartCoroutine(ResultCalculate());
        //test
        isDead = true;
    } // Die()

    //tset
    //IEnumerator ResultCalculate()
    //{
    //    yield return new WaitForSeconds(1.2f);
    //    GameManager.instance.CheckPlayerCount(); //죽을경우 플레이어를 셈
        
        
    //}
    //test

    // 바늘 사용 함수
    private void UsingNiddle()
    {
        //물방울 빠져나오는 소리 재생
        AudioManager.instance.PlayOneShot(niddlePopSound);

        CountCheckNiddle(-1);
        isStuckWater = false;
        animator.SetBool("StuckWater", isStuckWater);
        speed = remainSpeed;
        if(niddleCount == 0)
        {
            gameResult.GetComponent<GameResult>().inventory_NiddleImage.SetActive(false);
            gameResult.GetComponent<GameResult>().itemCtrl_NiddleImage.SetActive(false);
            //GameManager.instance.inventory_NiddleImage.SetActive(false);
            //GameManager.instance.itemCtrl_NiddleImage.SetActive(false);
        }
    } // UsingNiddle()

    private void CountCheckNiddle(int number)
    {
        if (!pv.IsMine)
        {
            return;
        }
        niddleCount = niddleCount + number;
        gameResult.GetComponent<GameResult>().NiddleCount(this.niddleCount);

        //GameManager.instance.NiddleCount(this.niddleCount);
    }
} // Class PlayerController

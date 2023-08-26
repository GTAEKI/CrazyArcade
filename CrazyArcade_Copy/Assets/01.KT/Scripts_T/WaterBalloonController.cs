using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class WaterBalloonController : MonoBehaviour
{
    public AudioClip bombSound;
    public AudioClip bombSetSound;
    public AudioClip kickSound;

    public float range = 0.25f; //물풍선이 Player가 어떤방향으로 들어오는지 감지하기 위한 범위

    // 플레이어가 어떤방향으로 왔는지 체크한 뒤 변화시킬 bool변수
    public bool playerIsDown = false, playerIsUp = false, playerIsLeft = false, playerIsRight = false;

    //레이캐스트 변수
    private RaycastHit2D hitInfo;

    //레이캐스트에서 물풍선을 위치시킬때 간격을 띄우며 사용할 변수
    private float tileSize = 0.7f;

    // 물풍선 파워
    public float power;
    public GameObject player;
    public float plusPosition = 0.666665f;

    // 물풍선 터졌을때 나오는 애니메이션
    public GameObject bombWater_Center;
    public GameObject bombWater_Down_Last;
    public GameObject bombWater_Down_Mid;
    public GameObject bombWater_Left_Last;
    public GameObject bombWater_Left_Mid;
    public GameObject bombWater_Right_Last;
    public GameObject bombWater_Right_Mid;
    public GameObject bombWater_Up_Last;
    public GameObject bombWater_Up_Mid;
    public GameObject waterExplosionSound;
    
    // Overlap 변수
    public Vector2 boxSize = new Vector2(0.67f, 0.67f);

    // 시간 체크하는 변수 (상자 위치 일치에 사용)
    private float time = 0f;
    private float setTime = 1f;

    public int actorNumber;

    void Start()
    {
        //배찌 플레이어를 찾아서
        player = GameObject.Find("PlayerBazzi(Clone)");
        //배찌 플레이어의 파워를 받아와서 물풍선에 적용
        power = player.GetComponent<PlayerController>().power;
        // 물풍선 설치시 자동으로 폭발
        StartCoroutine(Explosion());
        // 물풍선 설치 소리재생
        AudioManager.instance.PlayOneShot(bombSetSound);
    }//Start()


    // 2.5초 뒤 실행할 물풍선 폭발 관련 내용 전체
    public IEnumerator Explosion()
    {
        yield return new WaitForSeconds(2.5f);
        ExplosionFunc();
        AudioManager.instance.PlayOneShot(bombSound);
        //GameObject destroySound = Instantiate(waterExplosionSound, Vector3.zero, Quaternion.identity);
        Destroy(gameObject);
    }//IEnumerator Explosion()

    public void ExplosionFunc()
    {
        Bomb(bombWater_Center, transform.position);
        // 좌측 폭발
        for (float i = 0; i >= -power; i = i - plusPosition)
        {
            if (i == 0)
            {
                continue;
            }
            // 폭발방향의 타일을 체크하며 폭발
            bool CheckTile = CheckTile_Horizontal(i);

            // 폭발방향에 타일이 있다면 한번 더 실행하고 For문을 break하여 물줄기 정지
            if (CheckTile)
            {
                break;
            }
        }

        // 우측 폭발
        for (float i = 0; i <= power; i = i + plusPosition)
        {
            if (i == 0)
            {
                continue;
            }
            // 폭발방향의 타일을 체크하며 폭발
            bool CheckTile = CheckTile_Horizontal(i);

            // 폭발방향에 타일이 있다면 한번 더 실행하고 For문을 break하여 물줄기 정지
            if (CheckTile)
            {
                break;
            }
        }

        // 위측 폭발
        for (float i = 0; i <= power; i = i + plusPosition)
        {
            if (i == 0)
            {
                continue;
            }
            // 폭발방향의 타일을 체크하며 폭발
            bool CheckTile = CheckTile_Vertical(i);

            // 폭발방향에 타일이 있다면 한번 더 실행하고 For문을 break하여 물줄기 정지
            if (CheckTile)
            {
                break;
            }
        }

        // 아래측 폭발
        for (float i = 0; i >= -power; i = i - plusPosition)
        {
            if (i == 0)
            {
                continue;
            }
            // 폭발방향의 타일을 체크하며 폭발
            bool CheckTile = CheckTile_Vertical(i);

            // 폭발방향에 타일이 있다면 한번 더 실행하고 For문을 break하여 물줄기 정지
            if (CheckTile)
            {
                break;
            }
        }
    }

    // Overlap을 이용해서 가로방향 타일을 체크하여 물줄기 제어
    private bool CheckTile_Horizontal(float i)
    {
        //overlap을 실행할 포인트를 설정
        Vector2 overlapPoint_Vector2 = new Vector2(transform.position.x+i, transform.position.y);
        // boxSize만큼의 범위의 collider를 감지하여 cols에 저장
        Collider2D[] cols = Physics2D.OverlapBoxAll(overlapPoint_Vector2, boxSize * 0.1f, 0);

        if(i < 0)
        {
            return CheckTileAndBomb(bombWater_Left_Mid,bombWater_Left_Last, cols, i);
        }
        else //(i>0)
        {
            return CheckTileAndBomb(bombWater_Right_Mid,bombWater_Right_Last, cols, i);
        }
    }//CheckTile_Horizontal()

    // Overlap을 이용해서 세로방향 타일을 체크하여 물줄기 제어
    private bool CheckTile_Vertical(float i)
    {
        //overlap을 실행할 포인트를 설정
        Vector2 overlapPoint_Vector2 = new Vector2(transform.position.x, transform.position.y + i);
        // boxSize만큼의 범위의 collider를 감지하여 cols에 저장
        Collider2D[] cols = Physics2D.OverlapBoxAll(overlapPoint_Vector2, boxSize * 0.1f, 0);
        // cols에 어떤순서로 오브젝트가 들어갔을지 모르기 때문에 foreach를 2개로 나누었음

        if (i < 0)
        {
            return CheckTileAndBomb(bombWater_Down_Mid, bombWater_Down_Last, cols, i);
        }
        else //(i>0)
        {
            return CheckTileAndBomb(bombWater_Up_Mid, bombWater_Up_Last, cols, i);
        }

    }//CheckTile_Vertical()

    private bool CheckTileAndBomb(GameObject bombAnimation_Mid, GameObject bombAnimation_Last, Collider2D[] cols, float i)
    {
        bool playLast = false;
        if(Mathf.Abs(i) >= power - plusPosition)
        {
            playLast = true;
        }

        // cols에 어떤순서로 오브젝트가 들어갔을지 모르기 때문에 foreach를 2개로 나누었음
        foreach (Collider2D col in cols)
        {
            // 감지된 콜라이더중 박스를 감지하여 있다면 한번더 폭발을 실행시키고 true를 반환하여 더이상 폭발이 진행되지 않도록 함
            if (col.tag == "FixedBox" || col.tag == "MoveBox")
            {
                Bomb(bombAnimation_Last, col.transform.position);

                return true;
            }
            // 감지된 콜라이더가 Wall라면 그자리에 폭발을 진행시키지는 않음
            else if (col.tag == "Wall")
            {
                return true;
            }
            else if (col.tag == "WaterBalloon")
            {
                Destroy(col.gameObject);
                col.GetComponent<WaterBalloonController>().ExplosionFunc();
                return true;
            }
        }

        foreach (Collider2D col in cols)
        {
            // 타일일 경우 폭발을 계속 진행시킴
            if (col.tag == "Tile")
            {
                if (playLast)
                {
                    Bomb(bombAnimation_Last, col.transform.position);
                }
                else
                {
                    Bomb(bombAnimation_Mid, col.transform.position);
                }
            }
        }
        return false;
    }//CheckTileAndBomb()

    // 물풍선 폭발 오브젝트 처리 함수
    private void Bomb(GameObject tilePrefab,Vector2 bombPosition)
    {
        GameObject obj = Instantiate(tilePrefab, bombPosition, Quaternion.identity);
    }//BombHorizontal()

    // 타일 중앙에 맞춰서 물풍선 포지션값 조정하는 함수
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Tile")
        {
            transform.position = collision.transform.position;
        }
        else if(collision.gameObject.tag == "Bottom_Thorn")
        {
            transform.position = collision.transform.position;
            ExplosionFunc();
            GameObject destroySound = Instantiate(waterExplosionSound, Vector3.zero, Quaternion.identity);
            Destroy(gameObject);
        }
    }//OnTriggerEnter2D()

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>().onShoe) // PlayerController스크립트를 갖고있고, onShoe가 true라면
        {
            AudioManager.instance.PlayOneShot(kickSound);
            // 물풍선기준 플레이어는 왼쪽위치
            if (collision.transform.position.x - transform.position.x < 0 && Mathf.Abs(collision.transform.position.y - transform.position.y) < range)
            {
                playerIsLeft = true;
                Debug.Log("플레이어: 왼쪽");
            }
            // 물풍선기준 플레이어는 오른쪽위치
            else if (collision.transform.position.x - transform.position.x > 0 && Mathf.Abs(collision.transform.position.y - transform.position.y) < range)
            {
                playerIsRight = true;
                Debug.Log("플레이어: 오른쪽");
            }
            // 물풍선기준 플레이어는 위쪽위치
            else if (collision.transform.position.y - transform.position.y > 0 && Mathf.Abs(collision.transform.position.x - transform.position.x) < range)
            {
                playerIsUp = true;
                Debug.Log("플레이어: 위쪽");
            }
            // 물풍선기준 플레이어는 아래쪽위치
            else if (collision.transform.position.y - transform.position.y < 0 && Mathf.Abs(collision.transform.position.x - transform.position.x) < range)
            {
                playerIsDown = true;
                Debug.Log("플레이어: 아래쪽");
            }
        }
    }//OnCollisionEnter2D()

    private void MoveWaterBalloon(Vector2 collisionPosition)
    {
        // 물풍선을 원하는 위치로 변경
        transform.position = Vector2.Lerp(transform.position, collisionPosition, Time.deltaTime * 4);
        time += Time.deltaTime;
        if (time > setTime)
        {
            time = 0f;
            //반복실행 방지를 위해 bool값 원상복구
            playerIsLeft = false;
        }
    }//MoveWaterBalloon()

    private void CheckDirection()
    {
        if (playerIsLeft)
        {
            //레이캐스트 시작지점 설정
            Vector2 rayStartPoint = new Vector2(transform.position.x + tileSize, transform.position.y);
            //레이캐스트 실행
            hitInfo = Physics2D.Raycast(rayStartPoint, Vector2.right, 10f, LayerMask.GetMask("Wall"));
            Vector2 collisionPosition = new Vector2(hitInfo.transform.position.x-tileSize, hitInfo.transform.position.y);
            if (hitInfo == true)
            {
                Debug.Log("오른쪽으로 움직이는중");
                MoveWaterBalloon(collisionPosition);
            }
        }
        else if (playerIsRight)
        {
            //레이캐스트 시작지점 설정
            Vector2 rayStartPoint = new Vector2(transform.position.x - tileSize, transform.position.y);
            //레이캐스트 실행
            hitInfo = Physics2D.Raycast(rayStartPoint, -Vector2.right, 10f, LayerMask.GetMask("Wall"));
            Vector2 collisionPosition = new Vector2(hitInfo.transform.position.x + tileSize, hitInfo.transform.position.y);
            if (hitInfo == true)
            {
                Debug.Log("왼쪽으로 움직이는 중");
                MoveWaterBalloon(collisionPosition);
            }
        }
        else if (playerIsUp)
        {
            //레이캐스트 시작지점 설정
            Vector2 rayStartPoint = new Vector2(transform.position.x, transform.position.y - tileSize);
            //레이캐스트 실행
            hitInfo = Physics2D.Raycast(rayStartPoint, Vector2.down, 10f, LayerMask.GetMask("Wall"));
            Vector2 collisionPosition = new Vector2(hitInfo.transform.position.x , hitInfo.transform.position.y + tileSize);
            if (hitInfo == true)
            {
                Debug.Log("아래로 움직이는 중");
                MoveWaterBalloon(collisionPosition);
            }
        }
        else if (playerIsDown)
        {
            //레이캐스트 시작지점 설정
            Vector2 rayStartPoint = new Vector2(transform.position.x, transform.position.y + tileSize);
            //레이캐스트 실행
            hitInfo = Physics2D.Raycast(rayStartPoint, Vector2.up, 10f, LayerMask.GetMask("Wall"));
            Vector2 collisionPosition = new Vector2(hitInfo.transform.position.x, hitInfo.transform.position.y - tileSize);
            if (hitInfo == true)//레이캐스트에 정보가 들어왔다면
            {
                Debug.Log("위로 움직이는 중");
                MoveWaterBalloon(collisionPosition);
            }
        }

    }

    private void Update()
    {
        CheckDirection();
    }//Update()
}//class WaterBalloonController

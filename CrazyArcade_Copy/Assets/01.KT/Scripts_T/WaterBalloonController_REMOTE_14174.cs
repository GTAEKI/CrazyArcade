using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaterBalloonController : MonoBehaviour
{
    // 물풍선 파워
    public int power;
    public GameObject player;
    private PlayerController playerController;

    // 물풍선 터졌을때 나오는 애니메이션
    public GameObject BombWater_Center;
    public GameObject BombWater_Down_Last;
    public GameObject BombWater_Down_Mid;
    public GameObject BombWater_Left_Last;
    public GameObject BombWater_Left_Mid;
    public GameObject BombWater_Right_Last;
    public GameObject BombWater_Right_Mid;
    public GameObject BombWater_Up_Last;
    public GameObject BombWater_Up_Mid;

    public GameObject waterExplosionSound;
    
    // Overlap 변수
    public Vector2 boxSize = new Vector2(0.67f, 0.67f);

    void Start()
    {

        //playerAudioPlayer = GetComponent<AudioSource>();
        player = GameObject.Find("PlayerBazzi(Clone)");
        power = player.GetComponent<PlayerController>().power;

        Debug.Log(power);
        // 물풍선 설치시 2.5초뒤 폭발
        StartCoroutine(Explosion());

    }//Start()

    // 2.5초 뒤 실행할 물풍선 폭발 관련 내용 전체
    public IEnumerator Explosion()
    {
        yield return new WaitForSeconds(2.5f);

        ExplosionFunc();

        GameObject destroySound = Instantiate(waterExplosionSound, Vector3.zero, Quaternion.identity);
        Destroy(destroySound);

        // 오브젝트 삭제
        Destroy(gameObject);

    }//IEnumerator Explosion()

    public void ExplosionFunc()
    {
        // CenterExplosion
        BombHorizontal(BombWater_Center, 0);

        // LeftExplosion
        for (float i = 0; i >= -power; i = i - 0.5f)
        {
            // 폭발방향의 타일을 체크
            bool CheckTile = CheckTile_Horizontal(i);

            // 폭발방향에 타일이 있다면 한번 더 실행하고 For문을 break하여 물줄기 정지
            if (CheckTile)
            {
                if (i == -power)
                {
                    BombHorizontal(BombWater_Left_Last, i);
                }
                else if (i < 0)
                {
                    BombHorizontal(BombWater_Left_Mid, i);
                }
                break;
            }

            if (i == 0)
            {
                continue;
            }
            else if (i == -power)
            {
                BombHorizontal(BombWater_Left_Last, i);
            }
            else if (i < 0)
            {
                BombHorizontal(BombWater_Left_Mid, i);
            }
        }

        // RightExplosion
        for (float i = 0; i <= power; i = i + 0.5f)
        {
            // 폭발방향의 타일을 체크
            bool CheckTile = CheckTile_Horizontal(i);

            // 폭발방향에 타일이 있다면 한번 더 실행하고 For문을 break하여 물줄기 정지
            if (CheckTile)
            {
                if (i == power)
                {
                    BombHorizontal(BombWater_Left_Last, i);
                }
                else if (i > 0)
                {
                    BombHorizontal(BombWater_Left_Mid, i);
                }
                break;
            }

            if (i == 0)
            {
                continue;
            }
            else if (i == power)
            {
                BombHorizontal(BombWater_Right_Last, i);
            }
            else if (i > 0)
            {
                BombHorizontal(BombWater_Right_Mid, i);
            }
        }
        // UpExplosion
        for (float i = 0; i <= power; i = i + 0.5f)
        {
            // 폭발방향의 타일을 체크
            bool CheckTile = CheckTile_Vertical(i);

            // 폭발방향에 타일이 있다면 한번 더 실행하고 For문을 break하여 물줄기 정지
            if (CheckTile)
            {
                if (i == power)
                {
                    BombVertical(BombWater_Up_Last, i);
                }
                else if (i > 0)
                {
                    BombVertical(BombWater_Up_Mid, i);
                }
                break;
            }

            if (i == 0)
            {
                continue;
            }
            else if (i == power)
            {
                BombVertical(BombWater_Up_Last, i);
            }
            else if (i > 0)
            {
                BombVertical(BombWater_Up_Mid, i);
            }
        }
        // DownExplosion
        for (float i = 0; i >= -power; i = i - 0.5f)
        {
            // 폭발방향의 타일을 체크
            bool CheckTile = CheckTile_Vertical(i);

            // 폭발방향에 타일이 있다면 한번 더 실행하고 For문을 break하여 물줄기 정지
            if (CheckTile)
            {
                if (i == -power)
                {
                    BombVertical(BombWater_Down_Last, i);
                }
                else if (i < 0)
                {
                    BombVertical(BombWater_Down_Mid, i);
                }

                break;
            }

            if (i == 0)
            {
                continue;
            }
            else if (i == -power)
            {
                BombVertical(BombWater_Down_Last, i);
            }
            else if (i < 0)
            {
                BombVertical(BombWater_Down_Mid, i);
            }
        }
    }

    // Overlap을 이용해서 타일을 체크하여 물줄기 제어
    private bool CheckTile_Horizontal(float i)
    {
        Vector2 m_tr_Vector2 = new Vector2(transform.position.x+i, transform.position.y);
        Collider2D[] cols = Physics2D.OverlapBoxAll(m_tr_Vector2, boxSize * 0.5f, 0);

        foreach (Collider2D col in cols)
        {
            if(col.tag== "FixedBox" || col.tag == "MoveBox")
            {
                Debug.Log(col.name);
                return true;
            }
        }
        return false;
    }//CheckTile_Horizontal()

    private bool CheckTile_Vertical(float i)
    {
        Vector2 m_tr_Vector2 = new Vector2(transform.position.x, transform.position.y + i);
        Collider2D[] cols = Physics2D.OverlapBoxAll(m_tr_Vector2, boxSize * 0.5f, 0);

        foreach (Collider2D col in cols)
        {
            if (col.tag == "FixedBox" || col.tag == "MoveBox")
            {
                Debug.Log(col.name);
                return true;
            }
        }
        return false;
    }//CheckTile_Vertical()

    // 수평축 물풍선 폭발 오브젝트 처리 함수
    private void BombHorizontal(GameObject tilePrefab, float i)
    {
        Vector3 horizontalPosition = new Vector3(transform.position.x + i, transform.position.y, 0);
        Instantiate(tilePrefab, horizontalPosition, Quaternion.identity);
    }//BombHorizontal()

    // 수직축 물풍선 폭발 오브젝트 처리 함수
    private void BombVertical(GameObject tilePrefab, float i)
    {
        Vector3 verticalPosition = new Vector3(transform.position.x, transform.position.y + i, 0);
        Instantiate(tilePrefab, verticalPosition, Quaternion.identity);
    }//BombVertical()

    // 타일 중앙에 맞춰서 물풍선 포지션값 조정하는 함수
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Tile")
        {
            transform.position = collision.transform.position;
        }
    }//OnTriggerEnter2D()

}//class WaterBalloonController

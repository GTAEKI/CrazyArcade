using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaterBalloonController : MonoBehaviour
{
    // 물풍선 파워
    public float power;
    public GameObject player;
    private PlayerController playerController;
    public float plusPosition = 0.666665f;

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
    
    // Overlap 변수
    public Vector2 boxSize = new Vector2(0.67f, 0.67f);

    void Start()
    {
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

        // 오브젝트 삭제
        Destroy(gameObject);
    }//IEnumerator Explosion()

    public void ExplosionFunc()
    {
        //// CenterExplosion
        //Bomb(BombWater_Center, Vector2.zero);

        // LeftExplosion
        for (float i = 0; i >= -power; i = i - plusPosition)
        {
            // 폭발방향의 타일을 체크
            bool CheckTile = CheckTile_Horizontal(i);

            // 폭발방향에 타일이 있다면 한번 더 실행하고 For문을 break하여 물줄기 정지
            if (CheckTile)
            {
                break;
            }
        }

        // RightExplosion
        for (float i = 0; i <= power; i = i + plusPosition)
        {
            // 폭발방향의 타일을 체크
            bool CheckTile = CheckTile_Horizontal(i);

            // 폭발방향에 타일이 있다면 한번 더 실행하고 For문을 break하여 물줄기 정지
            if (CheckTile)
            {
                break;
            }
        }

        // UpExplosion
        for (float i = 0; i <= power; i = i + plusPosition)
        {
            // 폭발방향의 타일을 체크
            bool CheckTile = CheckTile_Vertical(i);

            // 폭발방향에 타일이 있다면 한번 더 실행하고 For문을 break하여 물줄기 정지
            if (CheckTile)
            {
                break;
            }
        }

        // DownExplosion
        for (float i = 0; i >= -power; i = i - plusPosition)
        {
            // 폭발방향의 타일을 체크
            bool CheckTile = CheckTile_Vertical(i);

            // 폭발방향에 타일이 있다면 한번 더 실행하고 For문을 break하여 물줄기 정지
            if (CheckTile)
            {
                break;
            }
        }
    }

    // Overlap을 이용해서 타일을 체크하여 물줄기 제어
    private bool CheckTile_Horizontal(float i)
    {
        Vector2 m_tr_Vector2 = new Vector2(transform.position.x+i, transform.position.y);
        Collider2D[] cols = Physics2D.OverlapBoxAll(m_tr_Vector2, boxSize * 0.1f, 0);

        // cols에 어떤순서로 감지가 되었는지 체크를 할 수없기때문에 foreach를 2번 사용하였음
        foreach (Collider2D col in cols)
        {
            if (col.tag == "FixedBox" || col.tag == "MoveBox")
            {
                Debug.Log(col.name);
                Bomb(BombWater_Center, col.transform.position);

                return true;
            }
            else if (col.tag == "Wall")
            {
                return true;
            }
        }

        foreach (Collider2D col in cols)
        {
            if (col.tag == "Tile")
            {
                Bomb(BombWater_Center, col.transform.position);
            }
        }
        return false;
    }//CheckTile_Horizontal()

    private bool CheckTile_Vertical(float i)
    {
        Vector2 m_tr_Vector2 = new Vector2(transform.position.x, transform.position.y + i);
        Collider2D[] cols = Physics2D.OverlapBoxAll(m_tr_Vector2, boxSize * 0.1f, 0);

        foreach (Collider2D col in cols)
        {
            if (col.tag == "FixedBox" || col.tag == "MoveBox")
            {
                Debug.Log(col.name);
                Bomb(BombWater_Center, col.transform.position);

                return true;
            }
            else if (col.tag == "Wall")
            {
                return true;
            }
        }

        foreach (Collider2D col in cols)
        {
            if(col.tag == "Tile")
            {
                Bomb(BombWater_Center, col.transform.position);
            }
        }
        return false;
    }//CheckTile_Vertical()

    // 물풍선 폭발 오브젝트 처리 함수
    private void Bomb(GameObject tilePrefab,Vector2 bombPosition)
    {
        //Vector3 bombPosition = new Vector3(transform.position.x, transform.position.y, 0);
        Instantiate(tilePrefab, bombPosition, Quaternion.identity);
    }//BombHorizontal()

    // 타일 중앙에 맞춰서 물풍선 포지션값 조정하는 함수
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Tile")
        {
            transform.position = collision.transform.position;
        }
    }//OnTriggerEnter2D()

    

}//class WaterBalloonController

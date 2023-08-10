using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBomb : MonoBehaviour
{
    //// Overlap 예제
    //public Transform m_tr;
    //public Vector2 boxSize = new Vector2(0.67f, 0.67f);
    //public float halfSize = 1.0f;
    //public LayerMask m_LayerMask = -1;
    //// Overlap 예제


    // 폭발 시작시 1초뒤 삭제
    void Start()
    {
        Destroy(gameObject, 1f);
    }

    //private void Update()
    //{
    //    Vector2 m_tr_Vector2 = new Vector2(m_tr.position.x, m_tr.position.y);
    //    Collider2D[] cols = Physics2D.OverlapBoxAll(m_tr_Vector2, boxSize * 0.5f,0);

    //    foreach(Collider2D col in cols)
    //    {
    //        Debug.Log(col.name);
    //    }
        


    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Debug.Log("Player 물에 맞음");
        }
        else if(collision.tag == "WaterBalloon")
        {
            // TODO 물풍선 터지게 만들기
        }
        else if(collision.tag == "FixedBox")
        {
            Destroy(collision.gameObject);
            // TODO FixedBox 만날시 파괴, 필요한 내용 작성
        }
        else if(collision.tag == "MoveBox")
        {
            Destroy(collision.gameObject);

            //TODO MoveBox 만날시 파괴, 필요한 내용 작성
        }
    }

    //// OverLap 예제
    //private void OnDrawGizmos()
    //{
    //    if(m_tr != null)
    //    {
    //        Gizmos.color = Color.yellow;
    //        Gizmos.DrawCube(m_tr.position, boxSize);
    //    }
    //}
    //// Overlap 예제
}

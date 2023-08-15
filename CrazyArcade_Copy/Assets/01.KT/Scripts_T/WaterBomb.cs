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

    public Animator waterBombAnimator;

    // 폭발 시작시 0.5초뒤 삭제
    void Start()
    {
        waterBombAnimator = GetComponent<Animator>();

        Destroy(gameObject, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Debug.Log("Player 물에 맞음");
        }
        else if(collision.tag == "WaterBalloon")
        {
            WaterBalloonController waterBalloonFuc;
            waterBalloonFuc = collision.GetComponent<WaterBalloonController>();
            waterBalloonFuc.ExplosionFunc();
            Destroy(collision.gameObject);
        }
        else if(collision.tag == "FixedBox")
        {
            Destroy(collision.gameObject);
        }
        else if(collision.tag == "MoveBox")
        {
            Destroy(collision.gameObject);
        }
        else if (collision.GetComponent<Item>())
        {
            collision.GetComponent<Item>().itemHp--;
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

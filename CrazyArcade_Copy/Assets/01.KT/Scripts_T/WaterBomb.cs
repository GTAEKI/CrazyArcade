using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBomb : MonoBehaviour
{
    // 폭발 시작시 1초뒤 삭제
    void Start()
    {
        Destroy(gameObject, 1f);
    }

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
            // TODO FixedBox 만날시 파괴, 필요한 내용 작성
        }
        else if(collision.tag == "MoveBox")
        {
            //TODO MoveBox 만날시 파괴, 필요한 내용 작성
        }
    }
}

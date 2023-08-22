using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class WaterBomb : MonoBehaviour
{
    // 애니메이터 조정 변수
    public Animator waterBombAnimator;

    void Start()
    {
        waterBombAnimator = GetComponent<Animator>();

        // 폭발 시작시 0.5초뒤 삭제
        Destroy(gameObject, 0.5f);
    }//Start()

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerStuckCheck")
        {
            //플레이어가 물에 갇혔을 경우에는 플레이어가 물에 맞지 않음
            if(collision.GetComponentInParent<PlayerController>().isStuckWater == false)
            {
                //플레이어를 맞힐경우 StuckWaterBalloon함수를 실행시킴
                collision.GetComponentInParent<PlayerController>().StuckWaterBalloon();
            }
        }
        //else if(collision.tag == "WaterBalloon")
        //{
        //    WaterBalloonController waterBalloonFuc;
        //    waterBalloonFuc = collision.GetComponent<WaterBalloonController>(); //물풍선 스크립트 가져오기
        //    waterBalloonFuc.ExplosionFunc(); // 스크립트의 폭발 함수 실행
        //    Destroy(collision.gameObject); // 물풍선 삭제
        //}
        else if(collision.tag == "FixedBox") //FixedBox일경우 삭제
        {
            Destroy(collision.gameObject);
        }
        else if(collision.tag == "MoveBox") //MoveBox일경우 삭제
        {
            Destroy(collision.gameObject);
        }
        else if (collision.GetComponent<Item>()) // itemd일경우 체력을 1깎음
        {
            collision.GetComponent<Item>().itemHp--;
        }
    }//OnTriggerEnter2D()
}// Class WaterBomb

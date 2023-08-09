using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBalloonController : MonoBehaviour
{
    // 물풍선 파워
    public int power = 1;

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
    

    void Start()
    {
        // 물풍선 설치시 2.5초뒤 폭발
        StartCoroutine(Explosion());
    }//Start()

    // 2.5초 뒤 실행할 물풍선 폭발 관련 내용 전체
    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(2.5f);

        // X축 Horizontal, Z축(vector3에서는 Y축) Vertical방향으로 Power만큼 폭발
        for (float i = -power; i <= power;i = i+0.5f)
        {
            if(i== -power)
            {
                BombHorizontal(BombWater_Left_Last, i);
                BombVertical(BombWater_Down_Last, i);
            }
            else if(i == power)
            {
                BombHorizontal(BombWater_Right_Last, i);
                BombVertical(BombWater_Up_Last, i);
            }
            else if (i == 0)
            {
                BombHorizontal(BombWater_Center, i);
            }
            else if(i > 0)
            {
                BombHorizontal(BombWater_Right_Mid, i);
                BombVertical(BombWater_Up_Mid, i);
            }
            else if(i < 0)
            {
                BombHorizontal(BombWater_Left_Mid, i);
                BombVertical(BombWater_Down_Mid, i);
            }
        } //for

        // 오브젝트 삭제
        Destroy(gameObject);
    }//IEnumerator Explosion()

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

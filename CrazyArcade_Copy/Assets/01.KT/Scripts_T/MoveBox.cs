using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBox : MonoBehaviour
{
    
    public float range = 0.34f; //MoveBox가 Player가 어떤방향으로 들어오는지 감지하기 위한 범위

    public Vector2 boxSize = new Vector2(0.67f, 0.67f); //오버랩 변수
    private Vector2 collisionPosition; // 이동할 방향의 Tile Position을 저장할 변수
    private bool isMove; // 움직이는중인지 체크
    private bool isWall; // 벽이 있는지 체크

    // 시간 체크하는 변수 (상자 위치 일치에 사용)
    private float time = 0f;
    private float setTime = 1f;

    // Lerp를 이용하여 부드러운 박스 움직임을 표현하기 위해 Update에서 사용함
    private void Update()
    {
        if(isMove && !isWall) //움직이는중이거나 벽을 감지하지 않았다면
        {
            // MoveBox와 움직일 위치의 x,y좌표가 일치하면 움직이지않음을 알림(isMove = false)
            if(transform.position.x == collisionPosition.x && transform.position.y == collisionPosition.y)
            {
                isMove = false;
            }
            // isMove = true;라면 계속해서 원하는 위치로 Lerp이동
            else if(transform.position.x != collisionPosition.x || transform.position.y != collisionPosition.y)
            {
                transform.position = Vector2.Lerp(transform.position, collisionPosition, Time.deltaTime*4);
                Debug.Log("움직이는중");

                // 외부의 예측하지못한 상황으로 x,y좌표가 일치하지 않을경우를 대비, 일정시간 뒤 직접 일치시켜줌
                time += Time.deltaTime;
                if(time > setTime)
                {
                    time = 0f;
                    isMove = false;
                }
            }
        }
        //isWall = false;
    }//Update()

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") //Player는 MoveBox를 이동시킴
        {
            if (!isMove) //이미 움직이는 중이라면 밀수 없음
            {
                // 왼쪽에서 플레이어가 박스 Push (position.x값 비교를 통해 왼쪽체크 + y축 범위(range)에 들어올때 밀리도록 함)
                if (collision.transform.position.x - transform.position.x < 0 && Mathf.Abs(collision.transform.position.y - transform.position.y) < range)
                {
                    //overlap을 실행할 포인트를 설정
                    Vector2 overlapPoint_Vector2 = new Vector2(transform.position.x + 0.67f, transform.position.y);

                    // boxSize만큼의 범위의 collider를 감지하여 cols에 저장
                    Collider2D[] cols = Physics2D.OverlapBoxAll(overlapPoint_Vector2, boxSize * 0.1f, 0);

                    // cols에 어떤순서로 오브젝트가 들어갔을지 모르기 때문에 foreach를 2개로 나누었음
                    // 감지된 콜라이더중 박스와 물풍선 플레이어를 감지하여 해당방향으로 움직일 수 없도록 return함
                    foreach (Collider2D col in cols)
                    {
                        if (col.tag == "FixedBox" || col.tag == "MoveBox" || col.tag == "Wall"|| col.tag == "WaterBalloon" || col.tag == "Player")
                        {
                            isWall = true;
                            return;
                        }
                    }
                    // 다음 위치에 타일만 있을경우 collisionPosition에 타일의 포지션값을 입력하여 MoveBox를 이동시킬 준비를 함
                    foreach (Collider2D col in cols)
                    {
                        if (col.tag == "Tile")
                        {
                            isWall = false;
                            isMove = true;
                            collisionPosition = col.transform.position;                                  
                        }
                        else if (col.GetComponent<Item>()) //만약 해당자리에 아이템이 있을경우 아이템은 삭제시킴
                        {
                            Destroy(col.gameObject);
                        }
                    }
                }//if()
                // 오른쪽에서 플레이어가 박스 Push (position.x값 비교를 통해 오른쪽체크 + y축 범위(range)에 들어올때 밀리도록 함)
                else if (collision.transform.position.x - transform.position.x > 0 && Mathf.Abs(collision.transform.position.y - transform.position.y) < range)
                {
                    //overlap을 실행할 포인트를 설정
                    Vector2 overlapPoint_Vector2 = new Vector2(transform.position.x - 0.67f, transform.position.y);
                    // boxSize만큼의 범위의 collider를 감지하여 cols에 저장
                    Collider2D[] cols = Physics2D.OverlapBoxAll(overlapPoint_Vector2, boxSize * 0.1f, 0);

                    // cols에 어떤순서로 오브젝트가 들어갔을지 모르기 때문에 foreach를 2개로 나누었음
                    // 감지된 콜라이더중 박스와 물풍선 플레이어를 감지하여 해당방향으로 움직일 수 없도록 return함
                    foreach (Collider2D col in cols)
                    {
                        if (col.tag == "FixedBox" || col.tag == "MoveBox" || col.tag == "Wall" || col.tag == "WaterBalloon" || col.tag == "Player")
                        {
                            isWall = true;
                            return;
                        }
                    }
                    // 다음 위치에 타일만 있을경우 collisionPosition에 타일의 포지션값을 입력하여 MoveBox를 이동시킬 준비를 함
                    foreach (Collider2D col in cols)
                    {
                        if (col.tag == "Tile")
                        {
                            isWall = false;
                            isMove = true;
                            collisionPosition = col.transform.position;
                        }
                        else if (col.GetComponent<Item>()) //만약 해당자리에 아이템이 있을경우 아이템은 삭제시킴
                        {
                            Destroy(col.gameObject);
                        }
                    }
                }//else if()
                // 위쪽에서 플레이어가 박스 Push (position.y값 비교를 통해 오른쪽체크 + x축 범위(range)에 들어올때 밀리도록 함)
                else if (collision.transform.position.y - transform.position.y > 0 && Mathf.Abs(collision.transform.position.x - transform.position.x) < range)
                {
                    //overlap을 실행할 포인트를 설정
                    Vector2 overlapPoint_Vector2 = new Vector2(transform.position.x, transform.position.y - 0.67f);
                    // boxSize만큼의 범위의 collider를 감지하여 cols에 저장
                    Collider2D[] cols = Physics2D.OverlapBoxAll(overlapPoint_Vector2, boxSize * 0.1f, 0);

                    // cols에 어떤순서로 오브젝트가 들어갔을지 모르기 때문에 foreach를 2개로 나누었음
                    // 감지된 콜라이더중 박스와 물풍선 플레이어를 감지하여 해당방향으로 움직일 수 없도록 return함
                    foreach (Collider2D col in cols)
                    {
                        if (col.tag == "FixedBox" || col.tag == "MoveBox" || col.tag == "Wall" || col.tag == "WaterBalloon" || col.tag == "Player")
                        {
                            isWall = true;
                            return;
                        }
                    }
                    // 다음 위치에 타일만 있을경우 collisionPosition에 타일의 포지션값을 입력하여 MoveBox를 이동시킬 준비를 함
                    foreach (Collider2D col in cols)
                    {
                        if (col.tag == "Tile")
                        {
                            isWall = false;
                            isMove = true;
                            collisionPosition = col.transform.position;                            
                        }
                        else if (col.GetComponent<Item>())//만약 해당자리에 아이템이 있을경우 아이템은 삭제시킴
                        {
                            Destroy(col.gameObject);
                        }
                    }
                }//else if()
                // 아래쪽에서 플레이어가 박스 Push (position.y값 비교를 통해 아래쪽체크 + x축 범위(range)에 들어올때 밀리도록 함)
                else if (collision.transform.position.y - transform.position.y < 0 && Mathf.Abs(collision.transform.position.x - transform.position.x) < range)
                {
                    //overlap을 실행할 포인트를 설정
                    Vector2 overlapPoint_Vector2 = new Vector2(transform.position.x, transform.position.y + 0.67f);
                    // boxSize만큼의 범위의 collider를 감지하여 cols에 저장
                    Collider2D[] cols = Physics2D.OverlapBoxAll(overlapPoint_Vector2, boxSize * 0.1f, 0);

                    // cols에 어떤순서로 오브젝트가 들어갔을지 모르기 때문에 foreach를 2개로 나누었음
                    // 감지된 콜라이더중 박스와 물풍선 플레이어를 감지하여 해당방향으로 움직일 수 없도록 return함
                    foreach (Collider2D col in cols)
                    {
                        if (col.tag == "FixedBox" || col.tag == "MoveBox" || col.tag == "Wall" || col.tag == "WaterBalloon" || col.tag == "Player")
                        {
                            isWall = true;
                            return;
                        }
                    }
                    // 다음 위치에 타일만 있을경우 collisionPosition에 타일의 포지션값을 입력하여 MoveBox를 이동시킬 준비를 함
                    foreach (Collider2D col in cols)
                    {
                        if (col.tag == "Tile")
                        {
                            isWall = false;
                            isMove = true;
                            collisionPosition = col.transform.position;
                        }                        
                        else if (col.GetComponent<Item>())//만약 해당자리에 아이템이 있을경우 아이템은 삭제시킴
                        {
                            Destroy(col.gameObject);
                        }
                    }
                }//else if()
            }
        }
    }//OnCollisionEnter2D()
}// Class MoveBox

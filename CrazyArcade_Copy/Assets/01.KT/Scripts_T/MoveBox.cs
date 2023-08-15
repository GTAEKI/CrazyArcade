using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBox : MonoBehaviour
{    
    public float range = 0.34f;

    private Vector2 collisionPosition;
    private bool isMove;
    private bool isWall;

    //오버랩 변수
    public Vector2 boxSize = new Vector2(0.67f, 0.67f);

    // 시간 체크하는 변수 (상자 위치 일치에 사용)
    private float time = 0f;
    private float setTime = 1f;

    // Lerp를 이용하여 부드러운 박스 움직임을 표현하기 위해 Update에서 사용함
    private void Update()
    {
        if(isMove && !isWall) //움직이는중이거나 벽을 감지하지 않았다면
        {
            if(transform.position.x == collisionPosition.x && transform.position.y == collisionPosition.y) //움직이는 중을 체크
            {
                isMove = false;
            }
            else if(transform.position.x != collisionPosition.x || transform.position.y != collisionPosition.y)
            {
                transform.position = Vector2.Lerp(transform.position, collisionPosition, Time.deltaTime*4);
                Debug.Log("움직이는중");
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
        if (collision.gameObject.tag == "Player")
        {
            //움직이는 중이라면 밀수 없음
            if (!isMove)
            {
                // 왼쪽에서 플레이어가 박스 Push (position.x값 비교를 통해 왼쪽체크 + y축 범위(range)에 들어올때 밀리도록 함)
                if (collision.transform.position.x - transform.position.x < 0 && Mathf.Abs(collision.transform.position.y - transform.position.y) < range)
                {
                    Vector2 m_tr_Vector2 = new Vector2(transform.position.x + 0.67f, transform.position.y);
                    Collider2D[] cols = Physics2D.OverlapBoxAll(m_tr_Vector2, boxSize * 0.1f, 0);

                    foreach (Collider2D col in cols)
                    {
                        if (col.tag == "FixedBox" || col.tag == "MoveBox" || col.tag == "Wall")
                        {
                            isWall = true;
                            return;
                        }
                    }

                    foreach (Collider2D col in cols)
                    {
                        if (col.tag == "Tile")
                        {
                            isWall = false;
                            isMove = true;
                            collisionPosition = col.transform.position;                                  
                        }
                    }
                }//if()
                // 오른쪽에서 플레이어가 박스 Push (position.x값 비교를 통해 오른쪽체크 + y축 범위(range)에 들어올때 밀리도록 함)
                else if (collision.transform.position.x - transform.position.x > 0 && Mathf.Abs(collision.transform.position.y - transform.position.y) < range)
                {
                    Vector2 m_tr_Vector2 = new Vector2(transform.position.x - 0.67f, transform.position.y);
                    Collider2D[] cols = Physics2D.OverlapBoxAll(m_tr_Vector2, boxSize * 0.1f, 0);

                    foreach (Collider2D col in cols)
                    {
                        if (col.tag == "FixedBox" || col.tag == "MoveBox" || col.tag == "Wall")
                        {
                            isWall = true;
                            return;
                        }
                    }

                    foreach (Collider2D col in cols)
                    {
                        if (col.tag == "Tile")
                        {
                            isWall = false;
                            isMove = true;
                            collisionPosition = col.transform.position;
                        }
                    }
                }//else if()
                // 위쪽에서 플레이어가 박스 Push (position.y값 비교를 통해 오른쪽체크 + x축 범위(range)에 들어올때 밀리도록 함)
                else if (collision.transform.position.y - transform.position.y > 0 && Mathf.Abs(collision.transform.position.x - transform.position.x) < range)
                {
                    Vector2 m_tr_Vector2 = new Vector2(transform.position.x, transform.position.y - 0.67f);
                    Collider2D[] cols = Physics2D.OverlapBoxAll(m_tr_Vector2, boxSize * 0.1f, 0);

                    foreach (Collider2D col in cols)
                    {
                        if (col.tag == "FixedBox" || col.tag == "MoveBox" || col.tag == "Wall")
                        {
                            isWall = true;
                            return;
                        }
                    }

                    foreach (Collider2D col in cols)
                    {
                        if (col.tag == "Tile")
                        {
                            isWall = false;
                            isMove = true;
                            collisionPosition = col.transform.position;                            
                        }
                    }
                }//else if()
                // 아래쪽에서 플레이어가 박스 Push (position.y값 비교를 통해 아래쪽체크 + x축 범위(range)에 들어올때 밀리도록 함)
                else if (collision.transform.position.y - transform.position.y < 0 && Mathf.Abs(collision.transform.position.x - transform.position.x) < range)
                {
                    Vector2 m_tr_Vector2 = new Vector2(transform.position.x, transform.position.y + 0.67f);
                    Collider2D[] cols = Physics2D.OverlapBoxAll(m_tr_Vector2, boxSize * 0.1f, 0);

                    foreach (Collider2D col in cols)
                    {
                        if (col.tag == "FixedBox" || col.tag == "MoveBox" || col.tag == "Wall")
                        {
                            isWall = true;
                            return;
                        }
                    }

                    foreach (Collider2D col in cols)
                    {
                        if (col.tag == "Tile")
                        {
                            isWall = false;
                            isMove = true;
                            collisionPosition = col.transform.position;
                        }
                    }
                }//else if()

            }

        }//OnCollisionEnter2D()
    }
}

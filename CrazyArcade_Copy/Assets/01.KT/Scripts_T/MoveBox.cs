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

    private void Update()
    {
        if(isMove && !isWall)
        {
            if(transform.position.x == collisionPosition.x)
            {
                isMove = false;
            }
            else if(transform.position.x != collisionPosition.x)
            {
                transform.position = Vector2.Lerp(transform.position, collisionPosition, Time.deltaTime*3);
            }
        }
        //isWall = false;
    }//Update()

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
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

        }//OnCollisionEnter2D()
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBox : MonoBehaviour
{
    private Rigidbody2D MoveBoxRB;

    private Vector2 collisionPosition;
    // Start is called before the first frame update
    void Start()
    {
        MoveBoxRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Tile")
        {
            MoveBoxRB.velocity = Vector2.zero;
            collisionPosition = collision.transform.position;
            transform.position = Vector2.Lerp(transform.position, collision.transform.position,Time.deltaTime);
            StartCoroutine(CenterMatch());


            //TODO Lerp로 구현하기
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //TODO 플레이어 만나면 내 현재 좌표를 변수에 기억해두기
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //TODO 이동을 하지 않았다면 CollisionEnter2D에서 기억하고있는 현재 내 좌표로 다시 돌아감
        }
    }

    IEnumerator CenterMatch()
    {
        yield return new WaitForSeconds(0.5f);

        transform.position = collisionPosition;

    }
}

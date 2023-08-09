using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBalloonController : MonoBehaviour
{
    //public int rows = 5; // 타일맵의 행 수
    //public int columns = 5; // 타일맵의 열 수

    public GameObject tilePrefab; // 타일의 프리팹

    public int powerV = 1;

    void Start()
    {
        // Coroutine 이용하여 물풍선 2.5초뒤 폭발
        StartCoroutine(Explosion());
    }

    void Update()
    {
        
    }

    // 2.5초 뒤 실행할 물풍선 폭발 관련 내용 전체
    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(2.5f);

        // 폭발 콜라이더 생성
        for (int i = -powerV; i <= powerV; i++)
        {
            Vector3 horizontalPosition = new Vector3(transform.position.x + i, transform.position.y, 0);
            Vector3 verticalPosition = new Vector3(transform.position.x, transform.position.y + i, 0);

            GameObject horizontalTile = Instantiate(tilePrefab, horizontalPosition, Quaternion.identity);
            GameObject verticalTile = Instantiate(tilePrefab, verticalPosition, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Tile")
        {
            transform.position = collision.transform.position;
        }   
    }
}

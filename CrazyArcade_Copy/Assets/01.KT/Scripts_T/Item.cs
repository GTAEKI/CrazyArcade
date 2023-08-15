using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int itemHp = 2;
    // Start is called before the first frame update
    void Start()
    {
        //처음생성시에는 비활성화 했다가, 특정시점에 활성화 필요(ex_ 타일이 삭제 or 비행기에서 떨어질때)
        
        //gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(itemHp == 0)
        {
            Destroy(gameObject);
        }

    }
    //TODO 타일 삭제 시 gameObject.SetActive(true);

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "MoveBox")
        {
            transform.position = collision.transform.position;
        }
    }
}

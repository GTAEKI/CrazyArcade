using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    //아이템 기본 체력
    public int itemHp = 3;

    // Update is called once per frame
    void Update()
    {
        //아이템 체력이 0이 될경우 제거
        if(itemHp == 0)
        {
            Destroy(gameObject);
        }
    } // Update()

    //움직이는박스를 따라가도록 유지
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "MoveBox")
        {
            transform.position = collision.transform.position;
        }
    } // OnTriggerStay2D()
} // Class Item

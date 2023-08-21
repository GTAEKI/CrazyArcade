using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    //아이템 기본 체력
    public int itemHp = 2;

    // Update is called once per frame
    void Update()
    {
        
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

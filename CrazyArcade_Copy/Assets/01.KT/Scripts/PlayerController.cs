using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRB;
    public float speed = 4.0f;
    public GameObject waterBalloon;

    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        playerRB.velocity = new Vector2(speed * horizontal, speed * vertical);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(waterBalloon, transform.position, Quaternion.identity);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBalloonController : MonoBehaviour
{

    void Start()
    {
        //Destroy water balloon after 2.5 seconds
        Destroy(gameObject, 2.5f);
    }

    
    void Update()
    {
        
    }
}

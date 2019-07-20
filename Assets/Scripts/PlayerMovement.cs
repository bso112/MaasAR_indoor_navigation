using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject player;
    Vector3 newPosition;

    // Update is called once per frame
    void Update()
    {   
        //y값 고정
        newPosition = player.transform.position;
        newPosition.y = 0;
        player.transform.position = newPosition;
    }
}

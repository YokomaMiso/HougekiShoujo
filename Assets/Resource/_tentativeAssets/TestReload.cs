using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestReload : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    void Start()
    {
           spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        Color color = Color.clear;
        if (Player.instance.playerState == PLAYER_STATE.RELOADING) 
        {
            transform.position = Player.instance.transform.position + Vector3.up + Vector3.back * 3;
            color = Color.white;
        }

        spriteRenderer.color = color;   

    }
}

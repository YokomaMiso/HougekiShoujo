using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestReload : MonoBehaviour
{
    [SerializeField] Player player;
    SpriteRenderer spriteRenderer;

    void Start()
    {
           spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        Color color = Color.clear;
        if (player.playerState == PLAYER_STATE.RELOADING) 
        {
            transform.position = player.transform.position + Vector3.up + Vector3.back * 3;
            color = Color.white;
        }

        spriteRenderer.color = color;   

    }
}

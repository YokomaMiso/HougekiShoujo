using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Player ownerPlayer;
    public void SetPlayer(Player _player) { ownerPlayer = _player; }

    [SerializeField] float speed = 5;

    public Vector3 Move()
    {
        Vector3 movement = Vector3.zero;
        movement += Vector3.right * Input.GetAxis("Horizontal");
        movement += Vector3.forward * Input.GetAxis("Vertical");

        transform.position += movement * speed * Managers.instance.timeManager.GetDeltaTime();

        return movement;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCanvasBehavior : MonoBehaviour
{
    void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            Managers.instance.ChangeScene(GAME_STATE.IN_GAME);
            Managers.instance.ChangeState(GAME_STATE.IN_GAME);
            Destroy(gameObject);
        }
    }
}

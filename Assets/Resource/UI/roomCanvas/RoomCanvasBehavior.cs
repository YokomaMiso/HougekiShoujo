using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCanvasBehavior : MonoBehaviour
{
    void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            GAME_STATE sendState = GAME_STATE.IN_GAME;

            Managers.instance.ChangeScene(sendState);
            Managers.instance.ChangeState(sendState);
            Destroy(gameObject);
        }
    }
}

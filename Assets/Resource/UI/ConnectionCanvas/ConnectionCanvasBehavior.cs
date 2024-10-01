using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionCanvasBehavior : MonoBehaviour
{
    [SerializeField]
    Text stateText;

    void Update()
    {
        //ハンドシェイクが終了していればルームシーンへ移行する
        if(OSCManager.OSCinstance.GetIsFinishedHandshake())
        {
            stateText.text = "ルームシーンへ移行します";

            Managers.instance.ChangeScene(GAME_STATE.ROOM);
            Managers.instance.ChangeState(GAME_STATE.ROOM);
        }
    }
}

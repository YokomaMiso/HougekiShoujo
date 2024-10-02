using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionCanvasBehavior : MonoBehaviour
{
    [SerializeField]
    Text stateText;

    private void Start()
    {
        stateText = GetComponentInChildren<Text>();

        OSCManager.OSCinstance.CreateTempNet();
    }

    void Update()
    {
        //ハンドシェイクが終了していればルームシーンへ移行する
        if(OSCManager.OSCinstance.GetIsFinishedHandshake())
        {
            if (Managers.instance.playerID != 0)
            {
                stateText.text = "部屋を発見しました、参加します";
            }
            else
            {
                stateText.text = "部屋が無かったため新しく作成します";
            }

            Invoke("MoveToRoomScene", 2.0f);
        }
    }

    private void MoveToRoomScene()
    {
        Managers.instance.ChangeScene(GAME_STATE.ROOM);
        Managers.instance.ChangeState(GAME_STATE.ROOM);

        return;
    }
}

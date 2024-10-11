using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionCanvasBehavior : MonoBehaviour
{
    Text stateText;

    bool changed;
    float alphaValue = 0;
    int alphaPlus = 2;

    private void Start()
    {
        stateText = transform.GetChild(2).GetComponent<Text>();
        stateText.text = "部屋を探しています…";
        stateText.color = Color.clear;

     
        
        OSCManager.OSCinstance.CreateTempNet();
    }

    void Update()
    {
        //ハンドシェイクが終了していればルームシーンへ移行する
        if (OSCManager.OSCinstance.GetIsFinishedHandshake())
        {
            if (!changed)
            {
                if (Managers.instance.playerID != 0) { stateText.text = "部屋を発見しました\n参加します"; }
                else { stateText.text = "新しい部屋を作成します"; }
                Invoke("MoveToRoomScene", 2.0f);
            }
        }

        TextAlphaUpdate();
    }

    void TextAlphaUpdate()
    {
        alphaValue = Mathf.Clamp01(alphaValue + Time.deltaTime * alphaPlus);
        stateText.color = new Color(1, 1, 1, alphaValue);
    }

    private void MoveToRoomScene()
    {
        //stateText.text = "";
        changed = true;
        alphaPlus = -2;

        Managers.instance.ChangeScene(GAME_STATE.ROOM);
        Managers.instance.ChangeState(GAME_STATE.ROOM);

        return;
    }
}

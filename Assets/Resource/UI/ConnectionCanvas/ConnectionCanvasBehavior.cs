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

    float timeOutTimer;
    float timeOutTime = 4.0f;

    readonly string[] seachText = new string[4] 
    {
        "部屋を探しています…",
        "Searching for Room...",
        "正在搜索房间...",
        "正在搜索房間...",
    };
    readonly string[] foundText = new string[4]
    {
        "部屋を発見しました\n参加します",
        "Room has been found.\nProceeding to join.",
        "已找到房间\n即将加入",
        "已找到房間\n即將加入",
    };
    readonly string[] createText = new string[4]
    {
        "新しい部屋を作成します",
        "Creating a Room.",
        "已创建新的房间",
        "已創建新的房間",
    };
    readonly string[] timeOutText = new string[4]
    {
        "タイムアウトしました\nタイトルへ戻ります",
        "Timeout.\nReturning to Title.",
        "超时\n返回主页面",
        "超時\n返回主頁面",
    };
    private void Start()
    {
        stateText = transform.GetChild(2).GetComponent<Text>();
        stateText.text = seachText[(int)Managers.instance.nowLanguage];
        stateText.color = Color.clear;

        OSCManager.OSCinstance.CreateTempNet();
    }

    void Update()
    {
        timeOutTimer += Time.deltaTime;

        //ハンドシェイクが終了していればルームシーンへ移行する
        if (OSCManager.OSCinstance.GetIsFinishedHandshake())
        {
            if (!changed)
            {
                if (Managers.instance.playerID != 0) { stateText.text = foundText[(int)Managers.instance.nowLanguage]; }
                else { stateText.text = createText[(int)Managers.instance.nowLanguage]; }
                Invoke("MoveToRoomScene", 2.0f);
            }
        }
        else if (timeOutTimer > timeOutTime)
        {
            stateText.text = timeOutText[(int)Managers.instance.nowLanguage];
            Invoke("MoveToTitleScene", 2.0f);
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

    private void MoveToTitleScene()
    {
        //stateText.text = "";
        changed = true;
        alphaPlus = -2;

        Managers.instance.ChangeScene(GAME_STATE.TITLE);
        Managers.instance.ChangeState(GAME_STATE.TITLE);

        return;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultCanvasBehavior : MonoBehaviour
{
    float timer;
    const float canSubmitTimer = 1.5f;

    [SerializeField] GameObject exitButton;

    void Start()
    {
        exitButton.SetActive(false);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer < canSubmitTimer) { return; }

        exitButton.SetActive(true);

        if (Input.GetButtonDown("Submit"))
        {
            ScoreInit();
            ReturnRoom();
        }

    }

    void ScoreInit()
    {
        IngameData.GameData myIngameData = OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData;
        myIngameData.killCount = 0;
        myIngameData.deathCount = 0;
        myIngameData.friendlyFireCount = 0;
        OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData = myIngameData;
    }

    void ReturnRoom()
    {
        Managers.instance.ChangeScene(GAME_STATE.ROOM);
        Managers.instance.ChangeState(GAME_STATE.ROOM);
    }
}

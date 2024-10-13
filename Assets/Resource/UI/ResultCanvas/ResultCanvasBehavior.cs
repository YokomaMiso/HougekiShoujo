using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultCanvasBehavior : MonoBehaviour
{
    float timer;
    const float canSubmitTimer = 1.5f;

    [SerializeField] GameObject exitButtonPrefab;
    ResultExitButton exitButtonInstance;

    [SerializeField] ResultScoreBoard scoreBoard;

    void Start()
    {
    }

    void Update()
    {
        if (!scoreBoard.arriveToCenter) { return; }

        timer += Time.deltaTime;
        if (timer < canSubmitTimer) { return; }

        if (exitButtonInstance == null)
        {
            GameObject obj = Instantiate(exitButtonPrefab, transform);
            exitButtonInstance = obj.GetComponent<ResultExitButton>();
            exitButtonInstance.SetOwnerCanvas(this);
        }
    }

    public void ScoreInit()
    {
        IngameData.GameData myIngameData = OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData;
        myIngameData.killCount = 0;
        myIngameData.deathCount = 0;
        myIngameData.friendlyFireCount = 0;
        OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData = myIngameData;
    }

    public void ReturnRoom()
    {
        Managers.instance.ChangeScene(GAME_STATE.ROOM);
        Managers.instance.ChangeState(GAME_STATE.ROOM);
    }
}

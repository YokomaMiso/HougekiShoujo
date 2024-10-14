using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RESULT_STATE { DISPLAY_MVP, SHOW_SCORE_BOARD };

public class ResultCanvasBehavior : MonoBehaviour
{
    float timer;
    const float canSubmitTimer = 1.5f;

    [SerializeField] GameObject exitButtonPrefab;
    ResultExitButton exitButtonInstance;

    [SerializeField] GameObject scoreBoardPrefab;
    ResultScoreBoard scoreBoard;

    //MVP�Ƃ��̕\���n�����锠
    GameObject leaderBoards;
    [SerializeField] GameObject displayMVP;

    void Start()
    {
        GameObject scoreBoardInstance = Instantiate(scoreBoardPrefab, transform);
        scoreBoard = scoreBoardInstance.GetComponent<ResultScoreBoard>();
        scoreBoard.Init();

        leaderBoards = Instantiate(displayMVP, transform);
        leaderBoards.GetComponent<DisplayMVP>().SetMVPKDFData(scoreBoard.GetMVPKDF());
    }

    void Update()
    {
        //�����\������Ă���Ȃ瑁�����^�[��
        if (leaderBoards != null) { return; }

        scoreBoard.MoveToCenter();

        //�X�R�A�{�[�h�������\������ĂȂ��Ȃ烊�^�[��
        if (!scoreBoard.arriveToCenter) { return; }

        //�{�^�����\������Ă���Ȃ烊�^�[��
        if (exitButtonInstance != null) { return; }

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

        myIngameData.winner = -1;

        OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData = myIngameData;
    }

    public void ReturnRoom()
    {
        Managers.instance.ChangeScene(GAME_STATE.ROOM);
        Managers.instance.ChangeState(GAME_STATE.ROOM);
    }
}

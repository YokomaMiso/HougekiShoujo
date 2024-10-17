using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public enum RESULT_STATE { DISPLAY_MVP, SHOW_SCORE_BOARD };

public class ResultCanvasBehavior : MonoBehaviour
{
    float timer;
    const float canSubmitTimer = 1.5f;

    [SerializeField] GameObject exitButtonPrefab;
    ResultExitButton exitButtonInstance;

    [SerializeField] GameObject scoreBoardPrefab;
    ResultScoreBoard scoreBoard;

    //MVPとかの表示系を入れる箱
    GameObject leaderBoardInstance;
    [SerializeField] GameObject displayOtherAward;
    [SerializeField] GameObject displayMVP;
    GameObject[] leaderBoardPrefabs;
    int index = 0;
    const int indexLimit = (int)AWARD_ID.MAX_NUM;

    void Start()
    {
        GameObject scoreBoardInstance = Instantiate(scoreBoardPrefab, transform);
        scoreBoard = scoreBoardInstance.GetComponent<ResultScoreBoard>();
        scoreBoard.Init();

        leaderBoardPrefabs = new GameObject[indexLimit];
        leaderBoardPrefabs[0] = displayOtherAward;
        leaderBoardPrefabs[1] = displayMVP;

        LeaderBoardSpawn();
    }

    void Update()
    {
        //何か表示されているなら早期リターン
        if (leaderBoardInstance != null) { return; }
        else if (index < indexLimit) { LeaderBoardSpawn(); return; }

        scoreBoard.MoveToCenter();

        //スコアボードが中央表示されてないならリターン
        if (!scoreBoard.arriveToCenter) { return; }

        //ボタンが表示されているならリターン
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

    void LeaderBoardSpawn()
    {
        leaderBoardInstance = Instantiate(leaderBoardPrefabs[index], transform);
        switch (index)
        {
            case 0:
                leaderBoardInstance.GetComponent<DisplayOtherAward>().SetKDFData(scoreBoard.GetDeadRankerKDF(),AWARD_ID.DEATH);
                leaderBoardInstance.GetComponent<DisplayOtherAward>().SetKDFData(scoreBoard.GetCriminalKDF(),AWARD_ID.FF);
                break;

            case 1:
                leaderBoardInstance.GetComponent<DisplayMVP>().SetMVPKDFData(scoreBoard.GetMVPKDF());
                break;
        }

        index++;
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

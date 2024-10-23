using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultCanvasBehavior : MonoBehaviour
{
    float timer;
    const float displayAwardTime = 7.0f;
    const float moveToMVPTime = 7.5f;
    const float displayMVPTime = 14.0f;
    const float moveToScoreTime = 15.0f;

    [Header("動かすオブジェクト")]
    [SerializeField] Image newsPaperBG;
    [SerializeField] GameObject awards;
    [SerializeField] ResultScoreBoard scoreBoard;
    [SerializeField] DisplayMVP mvpGroup;

    [Header("子供のキャンバス")]
    [SerializeField] AwardCanvas awardCanvas;

    [Header("終了ボタン")]
    [SerializeField] ResultExitButton exitButton;

    Transform[] moveTransform = new Transform[4];
    Vector3[] defaultLocalPosition = new Vector3[4];
    readonly Vector3 displayAwardPos = Vector3.zero;
    readonly Vector3 displayMVPPos = Vector3.down * 1200;
    readonly Vector3 displayScorePos = Vector3.up * 650;

    void Start()
    {
        scoreBoard.Init();

        awardCanvas.SetResultCanvas(this);
        mvpGroup.SetResultCanvas(this);
        exitButton.SetOwnerCanvas(this);

        moveTransform[0] = newsPaperBG.transform;
        defaultLocalPosition[0] = moveTransform[0].localPosition;
        moveTransform[1] = awards.transform;
        defaultLocalPosition[1] = moveTransform[1].localPosition;
        moveTransform[2] = scoreBoard.transform;
        defaultLocalPosition[2] = moveTransform[2].localPosition;
        moveTransform[3] = mvpGroup.transform;
        defaultLocalPosition[3] = moveTransform[3].localPosition;

        OSCManager.OSCinstance.roomData.ready = false;
    }

    void Update()
    {
        timer += Time.deltaTime;
        float nowRate;
        if (displayAwardTime < timer && timer <= moveToMVPTime)
        {
            nowRate = Mathf.Sqrt((timer - displayAwardTime) / (moveToMVPTime - displayAwardTime));
            MoveItems(Vector3.Lerp(displayAwardPos, displayMVPPos, nowRate));
        }
        else if (displayMVPTime < timer && timer <= moveToScoreTime)
        {
            nowRate = Mathf.Sqrt((timer - displayMVPTime) / (moveToScoreTime - displayMVPTime));
            MoveItems(Vector3.Lerp(displayMVPPos, displayScorePos, nowRate));
        }
    }

    void MoveItems(Vector3 _pos)
    {
        for (int i = 0; i < moveTransform.Length; i++) { moveTransform[i].localPosition = defaultLocalPosition[i] + _pos; }
    }

    void ScoreInit()
    {
        IngameData.GameData myIngameData = OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData;
        myIngameData.killCount = 0;
        myIngameData.deathCount = 0;
        myIngameData.friendlyFireCount = 0;
        myIngameData.fireCount = 0;

        myIngameData.winner = -1;

        OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData = myIngameData;
    }

    public void ReturnRoom()
    {
        Managers.instance.roomManager.Init();
        Managers.instance.gameManager.Init();
        ScoreInit();
        Managers.instance.ChangeScene(GAME_STATE.ROOM);
        Managers.instance.ChangeState(GAME_STATE.ROOM);
    }

    public ResultScoreBoard.KDFData GetJunky() { return scoreBoard.GetJunkyKDF(); }
    public ResultScoreBoard.KDFData GetVictim() { return scoreBoard.GetVictimKDF(); }
    public ResultScoreBoard.KDFData GetDanger() { return scoreBoard.GetDangerKDF(); }
    public ResultScoreBoard.KDFData GetMVP() { return scoreBoard.GetMVPKDF(); }
}

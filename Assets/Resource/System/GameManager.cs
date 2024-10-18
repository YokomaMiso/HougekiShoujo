using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static MachingRoomData;


public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject otherPlayerPrefab;
    GameObject[] playerInstance;
    const int playerMaxNum = 6;
    public int allPlayerCount = playerMaxNum;

    public PlayerData[] playerDatas;

    [SerializeField] Material[] outLineMat;

    [SerializeField] GameObject ingameCanvasPrefab;
    public IngameCanvasBehavior ingameCanvas;

    [SerializeField] GameObject scoreBoardCanvasPrefab;
    GameObject scoreBoardCanvas;

    [SerializeField] public AllStageData allStageData;
    [SerializeField] GameObject suddenDeathAreaPrefab;
    SuddenDeathArea sdaInstance;

    [SerializeField] GameObject bgmAnnounceCanvas;


    const float startDelay = 4;
    const float endDelay = 3;

    //For Client
    int nowRound = 1;

    public void CreatePlayer()
    {
        //ホストのルームデータを読み取る
        MachingRoomData.RoomData roomData;
        if (Managers.instance.playerID == 0) { roomData = OSCManager.OSCinstance.roomData; }
        else { roomData = OSCManager.OSCinstance.GetRoomData(0); }

        //ステージ生成処理
        StageData nowStageData = allStageData.GetStageData(roomData.stageNum);
        GameObject stage = Instantiate(nowStageData.GetStagePrefab());

        //サドンデスエリアの生成
        GameObject sda = Instantiate(suddenDeathAreaPrefab, stage.transform);
        sda.transform.localScale = Vector3.one * nowStageData.GetStageRadius();
        sdaInstance = sda.GetComponent<SuddenDeathArea>();

        //ステージBGMの再生
        SoundManager.PlayBGMIntro(nowStageData.GetBGM().GetBGMIntro(), nowStageData.GetBGM().GetBGMLoop());
        //BGMラベルの生成
        GameObject bgmLabel = Instantiate(bgmAnnounceCanvas);
        bgmLabel.GetComponent<BGMAnnounceCanvasBehavior>().SetBGMText(nowStageData.GetBGM());

        //プレイヤーの生存をtrueにする
        OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData.alive = true;

        //プレイヤーの数を読み取る
        int playerCount = OSCManager.OSCinstance.GetRoomData(0).playerCount;
        RoomManager rm = Managers.instance.roomManager;

        //生成する数はプレイヤーの数
        playerInstance = new GameObject[playerMaxNum];

        int[] teamCount = new int[2] { 0, 0 };

        //仮のプレイヤー生成処理
        for (int i = 0; i < MachingRoomData.playerMaxCount; i++)
        {
            //ルームデータを読み取る
            MachingRoomData.RoomData oscRoomData = OSCManager.OSCinstance.GetRoomData(i);

            if (oscRoomData.myTeamNum == MachingRoomData.bannerEmpty) { continue; }

            //生成処理
            Vector3 spawnPos = nowStageData.GetDefaultPosition(oscRoomData.myTeamNum + teamCount[oscRoomData.myTeamNum] * 2);
            //自分の番号なら、自分用のプレハブを生成
            if (i == Managers.instance.playerID)
            {
                playerInstance[i] = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
                Camera.main.GetComponent<CameraMove>().SetPlayer(playerInstance[i].GetComponent<Player>());
            }
            //自分じゃないなら、他プレイヤー用のプレハブを生成
            else
            {
                playerInstance[i] = Instantiate(otherPlayerPrefab, spawnPos, Quaternion.identity);
            }

            Player nowPlayer = playerInstance[i].GetComponent<Player>();
            nowPlayer.SetPlayerID(oscRoomData.myID);
            nowPlayer.SetPlayerData(playerDatas[oscRoomData.selectedCharacterID]);
            nowPlayer.SetOutLineMat(outLineMat[oscRoomData.myTeamNum]);

            teamCount[oscRoomData.myTeamNum]++;
        }

        CreateIngameCanvas();
    }
    void CreateIngameCanvas()
    {
        GameObject ingameCanvasInstance = Instantiate(ingameCanvasPrefab);
        ingameCanvas = ingameCanvasInstance.GetComponent<IngameCanvasBehavior>();
    }

    public void AddKillLog(Player _player)
    {
        ingameCanvas.AddKillLog(_player);
    }

    public Player GetPlayer(int _num)
    {
        if (playerInstance == null) { return null; }
        if (_num >= playerInstance.Length) { return null; }
        if (playerInstance[_num] == null) { return null; }

        return playerInstance[_num].GetComponent<Player>();
    }

    public int GetPlayerCount()
    {
        return OSCManager.OSCinstance.GetRoomData(0).playerCount;
    }

    void Init()
    {
        nowRound = 1;
        sdaInstance.Init();

        if (Managers.instance.playerID == 0)
        {
            MachingRoomData.RoomData hostRoomData = OSCManager.OSCinstance.roomData;
            IngameData.GameData hostIngameData = OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData;

            hostRoomData.gameStart = false;
            hostIngameData.play = false;
            hostIngameData.start = false;
            hostIngameData.end = false;
            hostIngameData.startTimer = 0;
            hostIngameData.endTimer = 0;

            hostIngameData.roundCount = 1;
            hostIngameData.roundTimer = 60;
            hostIngameData.alivePlayerCountTeamA = hostRoomData.teamACount;
            hostIngameData.alivePlayerCountTeamB = hostRoomData.teamBCount;
            hostIngameData.winCountTeamA = 0;
            hostIngameData.winCountTeamB = 0;

            OSCManager.OSCinstance.roomData = hostRoomData;
            OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData = hostIngameData;
        }
    }

    void RoundInit()
    {
        MachingRoomData.RoomData hostRoomData;
        if (Managers.instance.playerID == 0)
        {
            IngameData.GameData hostIngameData = OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData;

            hostIngameData.play = false;
            hostIngameData.start = false;
            hostIngameData.end = false;
            hostIngameData.startTimer = 2;
            hostIngameData.endTimer = 0;

            hostIngameData.roundCount++;
            hostIngameData.roundTimer = 60;
            hostIngameData.alivePlayerCountTeamA = 0;
            hostIngameData.alivePlayerCountTeamB = 0;
            hostIngameData.winner = -1;

            OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData = hostIngameData;
            hostRoomData = OSCManager.OSCinstance.roomData;
        }
        else { hostRoomData = OSCManager.OSCinstance.GetRoomData(0); }

        StageData nowStageData = allStageData.GetStageData(hostRoomData.stageNum);
        int[] teamCount = new int[2] { 0, 0 };
        for (int i = 0; i < MachingRoomData.playerMaxCount; i++)
        {
            MachingRoomData.RoomData oscRoomData = OSCManager.OSCinstance.GetRoomData(i);

            if (GetPlayer(i) != null)
            {
                Vector3 spawnPos = nowStageData.GetDefaultPosition(oscRoomData.myTeamNum + teamCount[oscRoomData.myTeamNum] * 2);
                playerInstance[i].GetComponent<Player>().RoundInit();
                playerInstance[i].transform.position = spawnPos;

                teamCount[oscRoomData.myTeamNum]++;
            }
        }

        sdaInstance.Init();
        Camera.main.GetComponent<CameraMove>().ResetCameraFar();
    }

    void EndBehavior()
    {
        /*
        //Yボタンでゲームを抜ける
        if (Input.GetButtonDown("Y"))
        {
            Managers.instance.ChangeScene(GAME_STATE.RESULT);
            Managers.instance.ChangeState(GAME_STATE.RESULT);
            Managers.instance.roomManager.Init();
            Init();
        }
        */

        //ホストのデータ
        IngameData.GameData hostIngameData;
        bool gameEnd = false;

        //自分がホストなら
        if (Managers.instance.playerID == 0)
        {
            //自分のデータをホストとして格納
            hostIngameData = OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData;

            //endなら早期リターン
            if (!hostIngameData.end) { return; }

            //endTimerを足し算
            hostIngameData.endTimer += Managers.instance.timeManager.GetDeltaTime();

            //endDelay以下なら早期リターン
            if (hostIngameData.endTimer < endDelay)
            {
                OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData = hostIngameData;
                return;
            }

            //どちらかのチームのwinCountが一定数を超えているなら
            if (hostIngameData.winCountTeamA >= 3) { gameEnd = true; }
            if (hostIngameData.winCountTeamB >= 3) { gameEnd = true; }

            if (!gameEnd)
            {
                RoundInit();
            }
            else
            {
                Managers.instance.ChangeScene(GAME_STATE.RESULT);
                Managers.instance.ChangeState(GAME_STATE.RESULT);
                Managers.instance.roomManager.Init();
                Init();
            }
        }
        else
        {
            MachingRoomData.RoomData hostRoomData = OSCManager.OSCinstance.GetRoomData(0);
            hostIngameData = OSCManager.OSCinstance.GetIngameData(0).mainPacketData.inGameData;

            if (nowRound == hostIngameData.roundCount) { return; }

            nowRound = hostIngameData.roundCount;

            if (hostIngameData.winCountTeamA >= 3) { gameEnd = true; }
            if (hostIngameData.winCountTeamB >= 3) { gameEnd = true; }

            if (hostRoomData.gameStart)
            {
                RoundInit();
            }
            else
            {
                Managers.instance.ChangeScene(GAME_STATE.RESULT);
                Managers.instance.ChangeState(GAME_STATE.RESULT);
                Managers.instance.roomManager.Init();
                Init();
            }
        }

    }

    IngameData.GameData DeadCheck(IngameData.GameData _data)
    {
        //return _data;

        if (!_data.play) { return _data; }
        if (_data.winner != -1) { return _data; }

        //チームごとの生き残り数
        int[] aliveCount = new int[2] { 0, 0 };

        for (int i = 0; i < playerInstance.Length; i++)
        {
            MachingRoomData.RoomData oscRoomData;
            IngameData.GameData nowIngameData;
            if (Managers.instance.playerID == i)
            {
                oscRoomData = OSCManager.OSCinstance.roomData;
                nowIngameData = OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData;
            }
            else
            {
                oscRoomData = OSCManager.OSCinstance.GetRoomData(i);
                nowIngameData = OSCManager.OSCinstance.GetIngameData(i).mainPacketData.inGameData;
            }

            if (oscRoomData.myTeamNum == MachingRoomData.bannerEmpty) { continue; }

            if (nowIngameData.alive) { aliveCount[oscRoomData.myTeamNum]++; }
        }

        MachingRoomData.RoomData hostRoomData = OSCManager.OSCinstance.roomData;

        _data.alivePlayerCountTeamA = aliveCount[(int)TEAM_NUM.A];
        _data.alivePlayerCountTeamB = aliveCount[(int)TEAM_NUM.B];

        /*
        if (_data.roundTimer <= 0)
        {
            Debug.Log("時間切れだよ");

            //if(hostRoomData.teamACount== hostRoomData.teamBCount) { }
            if (_data.alivePlayerCountTeamA > _data.alivePlayerCountTeamB)
            {
                _data.winner = (int)TEAM_NUM.A;
                _data.winCountTeamA++;
            }
            else
            {
                _data.winner = (int)TEAM_NUM.B;
                _data.winCountTeamB++;
            }
        }
        else
        {
                if (aliveCount[(int)TEAM_NUM.A] <= 0)
                {
                    _data.winner = (int)TEAM_NUM.B;
                    _data.winCountTeamB++;
                    Debug.Log("Aチームの死亡数でチェック通ったよ");
                    Debug.Log("Bチームの勝利数 " + _data.winCountTeamB);
                }
                if (aliveCount[(int)TEAM_NUM.B] <= 0)
                {
                    _data.winner = (int)TEAM_NUM.A;
                    _data.winCountTeamA++;
                    Debug.Log("Bチームの死亡数でチェック通ったよ");
                    Debug.Log("Aチームの勝利数 " + _data.winCountTeamA);
                }
        }
        */

        if (aliveCount[(int)TEAM_NUM.A] <= 0)
        {
            _data.winner = (int)TEAM_NUM.B;
            _data.winCountTeamB++;
            Debug.Log("Aチームの死亡数でチェック通ったよ");
            Debug.Log("Bチームの勝利数 " + _data.winCountTeamB);

            for (int i = 0; i < playerInstance.Length; i++)
            {
                MachingRoomData.RoomData roomData;
                if (i == Managers.instance.playerID) { roomData = OSCManager.OSCinstance.roomData; }
                else { roomData = OSCManager.OSCinstance.GetRoomData(i); }

                if (roomData.myTeamNum != _data.winner) { continue; }

                AudioClip clip;
                PlayerVoiceData pvd = playerDatas[roomData.selectedCharacterID].GetPlayerVoiceData();
                if (_data.winCountTeamA >= 3) { clip = pvd.GetGameWin(); }
                else { clip = pvd.GetRoundWin(); }

                GetPlayer(i).PlayVoice(clip, Camera.main.transform);
            }
        }
        else if (aliveCount[(int)TEAM_NUM.B] <= 0)
        {
            _data.winner = (int)TEAM_NUM.A;
            _data.winCountTeamA++;
            Debug.Log("Bチームの死亡数でチェック通ったよ");
            Debug.Log("Aチームの勝利数 " + _data.winCountTeamA);

            for (int i = 0; i < playerInstance.Length; i++)
            {
                MachingRoomData.RoomData roomData;
                if (i == Managers.instance.playerID) { roomData = OSCManager.OSCinstance.roomData; }
                else { roomData = OSCManager.OSCinstance.GetRoomData(i); }

                if (roomData.myTeamNum != _data.winner) { continue; }

                AudioClip clip;
                PlayerVoiceData pvd = playerDatas[roomData.selectedCharacterID].GetPlayerVoiceData();
                if (_data.winCountTeamA >= 3) { clip = pvd.GetGameWin(); }
                else { clip = pvd.GetRoundWin(); }

                GetPlayer(i).PlayVoice(clip, Camera.main.transform);
            }
        }

        return _data;
    }

    void ShowScoreBoard()
    {
        IngameData.GameData hostIngameData;
        if (Managers.instance.playerID == 0) { hostIngameData = OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData; }
        else { hostIngameData = OSCManager.OSCinstance.GetIngameData(0).mainPacketData.inGameData; }

        //ゲームが終了してるなら
        if (hostIngameData.end)
        {
            //キャンバスの実体があるなら削除する
            if (scoreBoardCanvas != null) { Destroy(scoreBoardCanvas); }
            //早期リターン
            return;
        }

        //キャンバスの実体がないなら
        if (scoreBoardCanvas == null)
        {
            //RB押下時にキャンバスを生成
            if (Input.GetButtonDown("RB")) { scoreBoardCanvas = Instantiate(scoreBoardCanvasPrefab); }
        }
        else
        {
            //RBを離した時にキャンバスを削除
            if (Input.GetButtonUp("RB")) { Destroy(scoreBoardCanvas); }
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != (int)GAME_STATE.IN_GAME) { return; }

        ShowScoreBoard();
        EndBehavior();

        //I'm not host, return
        if (Managers.instance.playerID != 0) { return; }

        IngameData.GameData hostIngameData = OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData;

        if (!hostIngameData.play)
        {
            if (!hostIngameData.start)
            {
                hostIngameData.startTimer += Managers.instance.timeManager.GetDeltaTime();
                if (hostIngameData.startTimer > startDelay)
                {
                    hostIngameData.play = true;
                    hostIngameData.start = true;
                }
            }
        }
        else
        {
            hostIngameData.roundTimer -= Managers.instance.timeManager.GetDeltaTime();

            hostIngameData = DeadCheck(hostIngameData);
            if (hostIngameData.winner != -1)
            {
                hostIngameData.play = false;
                hostIngameData.end = true;
            }
        }

        OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData = hostIngameData;
    }
}

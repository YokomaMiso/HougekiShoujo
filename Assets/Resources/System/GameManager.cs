using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


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

    [SerializeField] Material defaultMat;
    public Material spriteDefaultMat;

    const float startDelay = 4;
    const float endDelay = 3;

    //For Client
    int nowRound = 1;

    void Awake() { spriteDefaultMat = defaultMat; }

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
        SoundManager.PlayBGM(nowStageData.GetBGMData().GetBGM());
        //BGMラベルの生成
        GameObject bgmLabel = Instantiate(bgmAnnounceCanvas);
        bgmLabel.GetComponent<BGMAnnounceCanvasBehavior>().SetBGMText(nowStageData.GetBGMData());

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

    public void Init()
    {
        nowRound = 1;
        if (sdaInstance) sdaInstance.Init();

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
        if (InputManager.GetKeyDown(BoolActions.RightShoulder))
        {
            OSCManager.OSCinstance.roomData.gameStart = false;
            OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData.roundCount = 0;
            OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData.winner = 0;

            Managers.instance.ChangeScene(GAME_STATE.ROOM);
            Managers.instance.ChangeState(GAME_STATE.ROOM);
            RoundInit();
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
                OSCManager.OSCinstance.roomData.gameStart = false;
                OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData.roundCount = 0;

                Managers.instance.ChangeScene(GAME_STATE.RESULT);
                Managers.instance.ChangeState(GAME_STATE.RESULT);
                //Managers.instance.roomManager.Init();
                //Init();
            }
        }
        else
        {
            MachingRoomData.RoomData hostRoomData = OSCManager.OSCinstance.GetRoomData(0);
            hostIngameData = OSCManager.OSCinstance.GetIngameData(0).mainPacketData.inGameData;

            if (nowRound == hostIngameData.roundCount) { return; }

            nowRound = hostIngameData.roundCount;

            //if (hostIngameData.winCountTeamA >= 3) { gameEnd = true; }
            //if (hostIngameData.winCountTeamB >= 3) { gameEnd = true; }

            if (hostRoomData.gameStart)
            {
                RoundInit();
            }
            else
            {
                Managers.instance.ChangeScene(GAME_STATE.RESULT);
                Managers.instance.ChangeState(GAME_STATE.RESULT);
            }
        }

    }

    int tentativeCount;
    const int tentativeMaxCount = 5;

    IngameData.GameData DeadCheck(IngameData.GameData _data)
    {
        //return _data;

        if (!_data.play) { return _data; }
        if (_data.winner != -1) { return _data; }

        //チームごとの生き残り数
        int[] aliveCount = new int[2] { 0, 0 };
        int[] ffCount = new int[2] { 0, 0 };

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
            else
            {
                if (OSCManager.OSCinstance.GetRoomData(nowIngameData.killPlayerID).myTeamNum == oscRoomData.myTeamNum)
                {
                    ffCount[oscRoomData.myTeamNum]++;
                }
            }
        }

        _data.alivePlayerCountTeamA = aliveCount[(int)TEAM_NUM.A];
        _data.alivePlayerCountTeamB = aliveCount[(int)TEAM_NUM.B];

        int tentativeWinner = -1;
        if (aliveCount[(int)TEAM_NUM.A] <= 0)
        {
            if (aliveCount[(int)TEAM_NUM.B] <= 0) { tentativeWinner = 2; }
            else { tentativeWinner = 1; }
        }
        else if (aliveCount[(int)TEAM_NUM.B] <= 0) { tentativeWinner = 0; }

        if (tentativeWinner == (int)TEAM_NUM.A || tentativeWinner == (int)TEAM_NUM.B) { tentativeCount++; }

        if (tentativeCount < tentativeMaxCount) { return _data; }

        tentativeCount = 0;
        if (tentativeWinner == 2)
        {
            MachingRoomData.RoomData hostRoomData;
            if (Managers.instance.playerID == 0) { hostRoomData = OSCManager.OSCinstance.roomData; }
            else { hostRoomData = OSCManager.OSCinstance.GetRoomData(0); }

            float[] deadTime = new float[2] { 60, 60 };
            for (int i = 0; i < hostRoomData.playerCount; i++)
            {
                MachingRoomData.RoomData nowRoomData = OSCManager.OSCinstance.GetRoomData(i);
                IngameData.GameData nowIngameData = OSCManager.OSCinstance.GetIngameData(i).mainPacketData.inGameData;
                if (deadTime[nowRoomData.myTeamNum] > nowIngameData.deadTime)
                {
                    deadTime[nowRoomData.myTeamNum] = nowIngameData.deadTime;
                }
            }

            if (deadTime[(int)TEAM_NUM.A] > deadTime[(int)TEAM_NUM.B]) { tentativeWinner = (int)TEAM_NUM.A; }
            else if (deadTime[(int)TEAM_NUM.B] > deadTime[(int)TEAM_NUM.A]) { tentativeWinner = (int)TEAM_NUM.B; }
            else
            {
                if (ffCount[(int)TEAM_NUM.A] > ffCount[(int)TEAM_NUM.B]) { tentativeWinner = (int)TEAM_NUM.B; }
                else if (ffCount[(int)TEAM_NUM.B] < ffCount[(int)TEAM_NUM.A]) { tentativeWinner = (int)TEAM_NUM.A; }
                else { tentativeWinner = Random.Range(0, 2); }
            }
        }

        if (tentativeWinner == (int)TEAM_NUM.A)
        {
            _data.winner = (int)TEAM_NUM.B;
            _data.winCountTeamB++;

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

                GetPlayer(i).PlayVoice(clip, Camera.main.transform, 2);
            }
        }
        else if (tentativeWinner == (int)TEAM_NUM.B)
        {
            _data.winner = (int)TEAM_NUM.A;
            _data.winCountTeamA++;

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

                GetPlayer(i).PlayVoice(clip, Camera.main.transform, 2);
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
            if (InputManager.GetKeyDown(BoolActions.RightShoulder)) { scoreBoardCanvas = Instantiate(scoreBoardCanvasPrefab); }
        }
        else
        {
            //RBを離した時にキャンバスを削除
            if (InputManager.GetKeyUp(BoolActions.RightShoulder)) { Destroy(scoreBoardCanvas); }
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

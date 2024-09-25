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


    //仮座標
    readonly int[] teamPosX = new int[2] { -10, 10 };
    readonly int[] playerPosZ = new int[3] { 3, 0, -3 };

    const int endWinCount = 3;
    const int endDeadCount = 1;//playerMaxNum / 2;

    const float startDelay = 4;
    public float startTimer;

    const float endDelay = 3;
    public float endTimer;


    public void CreatePlayer()
    {
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
            Vector3 spawnPos = new Vector3(teamPosX[oscRoomData.myTeamNum], 0, playerPosZ[teamCount[oscRoomData.myTeamNum]]);
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
        startTimer = 0;
        endTimer = 0;

        if (Managers.instance.playerID == 0)
        {
            MachingRoomData.RoomData hostRoomData = OSCManager.OSCinstance.roomData;
            IngameData.GameData hostIngameData = OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData;

            hostRoomData.gameStart = false;
            hostIngameData.play = false;
            hostIngameData.start = false;
            hostIngameData.end = false;

            hostIngameData.roundCount = 1;
            hostIngameData.roundTimer = 60;
            hostIngameData.deadPlayerCountTeamA = 0;
            hostIngameData.deadPlayerCountTeamB = 0;
            hostIngameData.winCountTeamA = 0;
            hostIngameData.winCountTeamB = 0;
            hostIngameData.winner = -1;

            OSCManager.OSCinstance.roomData = hostRoomData;
            OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData = hostIngameData;
        }
    }

    void RoundInit()
    {
        startTimer = 2;
        endTimer = 0;

        if (Managers.instance.playerID == 0)
        {
            IngameData.GameData hostIngameData = OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData;

            hostIngameData.play = false;
            hostIngameData.start = false;
            hostIngameData.end = false;

            hostIngameData.roundCount++;
            hostIngameData.roundTimer = 60;
            hostIngameData.deadPlayerCountTeamA = 0;
            hostIngameData.deadPlayerCountTeamB = 0;
            hostIngameData.winner = -1;

            OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData = hostIngameData;
        }

        int[] teamCount = new int[2] { 0, 0 };
        for (int i = 0; i < MachingRoomData.playerMaxCount; i++)
        {
            MachingRoomData.RoomData oscRoomData = OSCManager.OSCinstance.GetRoomData(i);

            if (GetPlayer(i) != null)
            {
                Vector3 spawnPos = new Vector3(teamPosX[oscRoomData.myTeamNum], 0, playerPosZ[teamCount[oscRoomData.myTeamNum]]);
                playerInstance[i].GetComponent<Player>().RoundInit();
                playerInstance[i].transform.position = spawnPos;

                teamCount[oscRoomData.myTeamNum]++;
            }
        }
    }

    void EndBehavior()
    {
        IngameData.GameData hostIngameData = OSCManager.OSCinstance.GetIngameData(0).mainPacketData.inGameData;

        if (!hostIngameData.end) { return; }

        endTimer += Managers.instance.timeManager.GetDeltaTime();
        if (endTimer < endDelay) { return; }

        bool gameEnd = false;

        if (hostIngameData.winCountTeamA >= 3) { gameEnd = true; }
        if (hostIngameData.winCountTeamB >= 3) { gameEnd = true; }

        if (!gameEnd)
        {
            RoundInit();
        }
        else
        {
            Managers.instance.ChangeScene(GAME_STATE.ROOM);
            Managers.instance.ChangeState(GAME_STATE.ROOM);
            Managers.instance.roomManager.Init();
            Init();
        }
    }

    bool DeadCheck()
    {
        //return false;
        IngameData.GameData hostIngameData = OSCManager.OSCinstance.GetIngameData(0).mainPacketData.inGameData;

        //if I'm not host, when return
        if (Managers.instance.playerID != 0) { return hostIngameData.winner != -1; }

        if (hostIngameData.winner != -1) { return true; }

        //チームごとの死亡カウント
        int[] deadCount = new int[2] { 0, 0 };

        for (int i = 0; i < playerInstance.Length; i++)
        {
            if (GetPlayer(i) == null) { continue; }

            MachingRoomData.RoomData oscRoomData = OSCManager.OSCinstance.GetRoomData(i);

            if (!playerInstance[i].GetComponent<Player>().GetAlive())
            {
                deadCount[oscRoomData.myTeamNum]++;
            }
        }

        MachingRoomData.RoomData hostRoomData = OSCManager.OSCinstance.GetRoomData(0);

        hostIngameData.deadPlayerCountTeamA = deadCount[(int)TEAM_NUM.A];
        hostIngameData.deadPlayerCountTeamB = deadCount[(int)TEAM_NUM.B];

        bool returnValue = false;

        if (hostIngameData.roundTimer <= 0)
        {
            //if(hostRoomData.teamACount== hostRoomData.teamBCount) { }
            if (hostIngameData.deadPlayerCountTeamA < hostIngameData.deadPlayerCountTeamB)
            {
                hostIngameData.winner = (int)TEAM_NUM.A;
                hostIngameData.winCountTeamA++;
                returnValue = true;
            }
            else
            {
                hostIngameData.winner = (int)TEAM_NUM.B;
                hostIngameData.winCountTeamB++;
                returnValue = true;
            }
        }
        else
        {
            if (deadCount[(int)TEAM_NUM.A] >= hostRoomData.teamACount)
            {
                hostIngameData.winner = (int)TEAM_NUM.A;
                hostIngameData.winCountTeamA++;
                returnValue = true;
            }
            if (deadCount[(int)TEAM_NUM.B] >= hostRoomData.teamBCount)
            {
                hostIngameData.winner = (int)TEAM_NUM.B;
                hostIngameData.winCountTeamB++;
                returnValue = true;
            }
        }

        OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData = hostIngameData;
        return returnValue;
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != (int)GAME_STATE.IN_GAME) { return; }

        IngameData.GameData hostIngameData = OSCManager.OSCinstance.GetIngameData(0).mainPacketData.inGameData;

        if (!hostIngameData.play)
        {
            if (!hostIngameData.start)
            {
                startTimer += Managers.instance.timeManager.GetDeltaTime();
                if (startTimer > startDelay)
                {
                    if (Managers.instance.playerID == 0)
                    {
                        hostIngameData.play = true;
                        hostIngameData.start = true;
                    }
                }
            }
        }
        else
        {
            if (Managers.instance.playerID == 0)
            {
                hostIngameData.roundTimer -= Managers.instance.timeManager.GetDeltaTime();
            }

            if (DeadCheck())
            {
                if (Managers.instance.playerID == 0)
                {
                    hostIngameData.play = false;
                    hostIngameData.end = true;
                }
            }
        }

        if (Managers.instance.playerID == 0)
        {
            OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData = hostIngameData;
        }

        EndBehavior();
    }

}

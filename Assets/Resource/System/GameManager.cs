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

    public bool play = false;
    public int roundCount = 1;
    public float roundTimer = 60;
    const int endWinCount = 3;
    const int endDeadCount = 1;//playerMaxNum / 2;
    public int[] deadPlayerCount = new int[2];
    public int[] roundWinCount = new int[2];

    public bool start;
    const float startDelay = 4;
    public float startTimer;

    public bool end;
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
        play = false;
        roundTimer = 60;
        roundCount = 1;
        for (int i = 0; i < roundWinCount.Length; i++) { roundWinCount[i] = 0; }

        start = false;
        startTimer = 0;

        end = false;
        endTimer = 0;

        if (Managers.instance.playerID == 0)
        {
            OSCManager.OSCinstance.roomData.gameStart = false;
        }
    }

    void RoundInit()
    {
        play = false;
        roundTimer = 60;

        start = false;
        startTimer = 2;

        end = false;
        endTimer = 0;

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
        if (!end) { return; }

        endTimer += Managers.instance.timeManager.GetDeltaTime();
        if (endTimer < endDelay) { return; }

        bool gameEnd = false;

        roundCount++;

        int state = 0;
        if (deadPlayerCount[0] >= endDeadCount) { state = 1; }
        if (deadPlayerCount[1] >= endDeadCount) { state += 2; }

        if (state == 1) { roundWinCount[1]++; }
        else if (state == 2) { roundWinCount[0]++; }

        for (int i = 0; i < 2; i++) { if (roundWinCount[i] >= endWinCount) { gameEnd = true; } }

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

        bool returnValue = false;
        MachingRoomData.RoomData hostRoomData = OSCManager.OSCinstance.GetRoomData(0);

        deadPlayerCount[0] = deadCount[0];
        if (deadPlayerCount[0] >= hostRoomData.teamACount) { returnValue = true; }
        deadPlayerCount[1] = deadCount[1];
        if (deadPlayerCount[1] >= hostRoomData.teamBCount) { returnValue = true; }

        return returnValue;
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != (int)GAME_STATE.IN_GAME) { return; }

        if (!play)
        {
            if (!start)
            {
                startTimer += Managers.instance.timeManager.GetDeltaTime();
                if (startTimer > startDelay)
                {
                    play = true;
                    start = true;
                }
            }
        }
        else
        {
            roundTimer -= Managers.instance.timeManager.GetDeltaTime();
            if (DeadCheck())
            {
                play = false;
                end = true;
            }
        }

        EndBehavior();
    }

}

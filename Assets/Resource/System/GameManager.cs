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
    Vector3[] pos = new Vector3[playerMaxNum]
    {
        new Vector3(-10,0,3),
        new Vector3(10,0,3),
        new Vector3(-10,0,0),
        new Vector3(10,0,0),
        new Vector3(-10,0,-3),
        new Vector3(10,0,-3),
    };

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
        //ゲームがスタートしたので、ルームデータのスタートは初期化する
        OSCManager.OSCinstance.roomData.gameStart = false;
        //プレイヤーの生存をtrueにする
        OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData.alive = true;

        //プレイヤーの数を読み取る
        int playerCount = 0;
        RoomManager rm = Managers.instance.roomManager;
        int[] allBannerNum = new int[8];
        for (int i = 0; i < 8; i++) { allBannerNum[i] = rm.GetBannerNumFromAllPlayer(i); }

        for (int i = 0; i < MachingRoomData.bannerMaxCount; i++)
        {
            if (allBannerNum[i] != MachingRoomData.bannerEmpty)
            {
                playerCount++;
            }
        }

        Debug.Log("playerCount" + playerCount);

        //生成する数はプレイヤーの数
        playerInstance = new GameObject[playerMaxNum];
        //int instantiateCount = 0;
        int myNum = OSCManager.OSCinstance.roomData.myBannerNum;

        //仮のプレイヤー生成処理
        for (int i = 0; i < MachingRoomData.bannerMaxCount; i++)
        {
            if (allBannerNum[i] == MachingRoomData.bannerEmpty) { continue; }

            //ルームデータを読み取る
            MachingRoomData.RoomData oscRoomData = OSCManager.OSCinstance.GetRoomData(allBannerNum[i]);

            //生成処理
            //自分の番号なら、自分用のプレハブを生成
            if (i == myNum)
            {
                playerInstance[i] = Instantiate(playerPrefab, pos[i], Quaternion.identity);
                Camera.main.GetComponent<CameraMove>().SetPlayer(playerInstance[i].GetComponent<Player>());
            }
            //自分じゃないなら、他プレイヤー用のプレハブを生成
            else
            {
                playerInstance[i] = Instantiate(otherPlayerPrefab, pos[i], Quaternion.identity);
            }

            Player nowPlayer = playerInstance[i].GetComponent<Player>();
            nowPlayer.SetPlayerID(oscRoomData.myID);
            nowPlayer.SetPlayerData(playerDatas[oscRoomData.GetSelectedCharacterID(oscRoomData.myID)]);
            nowPlayer.SetOutLineMat(outLineMat[i % 2]);

            //instantiateCount++;
        }

    }

    public GameObject GetPlayer(int _num)
    {
        if (playerInstance == null) { return null; }
        if (_num >= playerInstance.Length) { return null; }

        return playerInstance[_num];
    }

    public int GetPlayerCount()
    {
        int returnNum = 0;
        for(int i = 0; i < playerInstance.Length; i++)
        {
            if (playerInstance[i] == null) { continue; }
            returnNum++;
        }


        return returnNum;
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
    }

    void RoundInit()
    {
        play = false;
        roundTimer = 60;

        start = false;
        startTimer = 0;

        end = false;
        endTimer = 0;

        for (int i = 0; i < MachingRoomData.bannerMaxCount - 2; i++)
        {
            if (GetPlayer(i) != null)
            {
                playerInstance[i].GetComponent<Player>().RoundInit();
                playerInstance[i].transform.position = pos[i];
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
        //チームごとの死亡カウント
        int[] deadCount = new int[2] { 0, 0 };

        for (int i = 0; i < playerInstance.Length; i++)
        {
            if (GetPlayer(i) == null) { continue; }

            if (!playerInstance[i].GetComponent<Player>().GetAlive())
            {
                deadCount[i % 2]++;
            }
        }

        bool returnValue = false;

        for (int i = 0; i < deadPlayerCount.Length; i++)
        {
            deadPlayerCount[i] = deadCount[i];
            if (deadPlayerCount[i] >= Managers.instance.roomManager.nowPlayerCount / 2) { returnValue = true; break; }
        }

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

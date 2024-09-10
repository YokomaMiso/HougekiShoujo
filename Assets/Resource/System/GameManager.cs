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
    const int playerMaxNum = 2;

    public PlayerData[] playerDatas;

    //仮座標
    Vector3[] pos = new Vector3[playerMaxNum] 
    {
        //new Vector3(-3,0,-10), 
        //new Vector3(-3,0,10),
        new Vector3(0,0,-10),
        new Vector3(0,0,10),
        //new Vector3(3,0,-10),
        //new Vector3(3,0,10),
    };

    public bool play = false;
    public int roundCount = 1;
    public float roundTimer = 100;
    public int[] roundWinCount = new int[2];

    public bool start;
    const float startDelay = 3;
    public float startTimer;

    public bool end;
    const float endDelay = 3;
    public float endTimer;

    public void CreatePlayer()
    {
        playerInstance = new GameObject[playerMaxNum];
        //仮のプレイヤー生成処理
        for (int i = 0; i < playerMaxNum; i++)
        {
            if (i == Managers.instance.playerID)
            {
                playerInstance[i] = Instantiate(playerPrefab, pos[i], Quaternion.identity);
                Camera.main.GetComponent<CameraMove>().SetPlayer(playerInstance[i].GetComponent<Player>());
            }
            else { playerInstance[i] = Instantiate(otherPlayerPrefab, pos[i], Quaternion.identity); }

            Player nowPlayer = playerInstance[i].GetComponent<Player>();
            nowPlayer.SetPlayerID(i);
            nowPlayer.SetPlayerData(playerDatas[Managers.instance.roomManager.selectedCharacterID[i]]);
        }
    }

    public GameObject GetPlayer(int _num)
    {
        if (playerInstance == null) { return null; }
        if (_num >= playerInstance.Length) { return null; }

        return playerInstance[_num];
    }

    void Init()
    {
        play = false;
        roundCount = 1;
        roundTimer = 100;
        for (int i = 0; i < roundWinCount.Length; i++) { roundWinCount[i] = 0; }

        start = false;
        startTimer = 0;

        end = false;
        endTimer = 0;
    }

    void EndBehavior()
    {
        if (!end) { return; }

        endTimer += Managers.instance.timeManager.GetDeltaTime();
        if (endTimer < endDelay) { return; }

        Managers.instance.ChangeScene(GAME_STATE.ROOM);
        Managers.instance.ChangeState(GAME_STATE.ROOM);
        Init();
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

            for (int i = 0; i < playerMaxNum; i++)
            {
                if (!playerInstance[i].GetComponent<Player>().GetAlive())
                {
                    play = false;
                    end = true;
                }
            }
        }

        EndBehavior();
    }

}

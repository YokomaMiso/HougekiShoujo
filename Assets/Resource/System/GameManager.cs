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
    [SerializeField] GameObject killLogCanvasPrefab;
    KillLogCanvas killLogCanvas;

    [SerializeField] GameObject scoreBoardCanvasPrefab;
    GameObject scoreBoardCanvas;

    //�����W
    readonly int[] teamPosX = new int[2] { -10, 10 };
    readonly int[] playerPosZ = new int[3] { 3, 0, -3 };

    const float startDelay = 4;
    const float endDelay = 3;

    //For Client
    int nowRound = 1;

    public void CreatePlayer()
    {
        //�v���C���[�̐�����true�ɂ���
        OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData.alive = true;

        //�v���C���[�̐���ǂݎ��
        int playerCount = OSCManager.OSCinstance.GetRoomData(0).playerCount;
        RoomManager rm = Managers.instance.roomManager;

        //�������鐔�̓v���C���[�̐�
        playerInstance = new GameObject[playerMaxNum];

        int[] teamCount = new int[2] { 0, 0 };

        //���̃v���C���[��������
        for (int i = 0; i < MachingRoomData.playerMaxCount; i++)
        {
            //���[���f�[�^��ǂݎ��
            MachingRoomData.RoomData oscRoomData = OSCManager.OSCinstance.GetRoomData(i);

            if (oscRoomData.myTeamNum == MachingRoomData.bannerEmpty) { continue; }

            //��������
            Vector3 spawnPos = new Vector3(teamPosX[oscRoomData.myTeamNum], 0, playerPosZ[teamCount[oscRoomData.myTeamNum]]);
            //�����̔ԍ��Ȃ�A�����p�̃v���n�u�𐶐�
            if (i == Managers.instance.playerID)
            {
                playerInstance[i] = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
                Camera.main.GetComponent<CameraMove>().SetPlayer(playerInstance[i].GetComponent<Player>());
            }
            //��������Ȃ��Ȃ�A���v���C���[�p�̃v���n�u�𐶐�
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

        CreateKillLogCanvas();
    }

    void CreateKillLogCanvas()
    {
        GameObject killLogCanvasInstance = Instantiate(killLogCanvasPrefab);
        killLogCanvas = killLogCanvasInstance.GetComponent<KillLogCanvas>();
    }

    public void AddKillLog(Player _player)
    {
        killLogCanvas.AddKillLog(_player);
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
            hostIngameData.winner = -1;

            OSCManager.OSCinstance.roomData = hostRoomData;
            OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData = hostIngameData;
        }
    }

    void RoundInit()
    {
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
        //�z�X�g�̃f�[�^
        IngameData.GameData hostIngameData;
        bool gameEnd = false;

        //�������z�X�g�Ȃ�
        if (Managers.instance.playerID == 0)
        {
            //�����̃f�[�^���z�X�g�Ƃ��Ċi�[
            hostIngameData = OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData;

            //end�Ȃ瑁�����^�[��
            if (!hostIngameData.end) { return; }

            //endTimer�𑫂��Z
            hostIngameData.endTimer += Managers.instance.timeManager.GetDeltaTime();

            //endDelay�ȉ��Ȃ瑁�����^�[��
            if (hostIngameData.endTimer < endDelay)
            {
                OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData = hostIngameData;
                return;
            }

            //�ǂ��炩�̃`�[����winCount����萔�𒴂��Ă���Ȃ�
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
        IngameData.GameData hostIngameData;

        hostIngameData = OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData;

        if (!hostIngameData.play) { return _data; }

        if (hostIngameData.winner != -1) { return _data; }

        //�`�[�����Ƃ̐����c�萔
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

        hostIngameData.alivePlayerCountTeamA = aliveCount[(int)TEAM_NUM.A];
        hostIngameData.alivePlayerCountTeamB = aliveCount[(int)TEAM_NUM.B];

        if (hostIngameData.roundTimer <= 0)
        {
            Debug.Log("���Ԑ؂ꂾ��");

            //if(hostRoomData.teamACount== hostRoomData.teamBCount) { }
            if (hostIngameData.alivePlayerCountTeamA > hostIngameData.alivePlayerCountTeamB)
            {
                hostIngameData.winner = (int)TEAM_NUM.A;
                hostIngameData.winCountTeamA++;
            }
            else
            {
                hostIngameData.winner = (int)TEAM_NUM.B;
                hostIngameData.winCountTeamB++;
            }
        }
        else
        {

            if (aliveCount[(int)TEAM_NUM.A] <= 0)
            {
                hostIngameData.winner = (int)TEAM_NUM.B;
                hostIngameData.winCountTeamB++;
                Debug.Log("A�`�[���̎��S���Ń`�F�b�N�ʂ�����");
                Debug.Log("B�`�[���̏����� " + hostIngameData.winCountTeamB);
            }
            if (aliveCount[(int)TEAM_NUM.B] <= 0)
            {
                hostIngameData.winner = (int)TEAM_NUM.A;
                hostIngameData.winCountTeamA++;
                Debug.Log("B�`�[���̎��S���Ń`�F�b�N�ʂ�����");
                Debug.Log("A�`�[���̏����� " + hostIngameData.winCountTeamA);
            }
        }

        _data = hostIngameData;
        return _data;
    }

    void ShowScoreBoard()
    {
        IngameData.GameData hostIngameData = OSCManager.OSCinstance.GetIngameData(0).mainPacketData.inGameData;

        //�Q�[�����I�����Ă�Ȃ�
        if (hostIngameData.end)
        {
            //�L�����o�X�̎��̂�����Ȃ�폜���đ������^�[��
            if (scoreBoardCanvas != null)
            {
                Destroy(scoreBoardCanvas);
                return;
            }
        }

        //�L�����o�X�̎��̂��Ȃ��Ȃ�
        if (scoreBoardCanvas == null)
        {
            //RB�������ɃL�����o�X�𐶐�
            if (Input.GetButtonDown("RB")) { scoreBoardCanvas = Instantiate(scoreBoardCanvasPrefab); }
        }
        else
        {
            //RB�𗣂������ɃL�����o�X���폜
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

using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using static MachingRoomData;

public class RoomManager : MonoBehaviour
{
    /*内部的な処理*/
    public readonly int empty = -1;

    public void Init()
    {
        RoomData oscRoomData = OSCManager.OSCinstance.roomData;
        oscRoomData.gameStart = false;
        oscRoomData.ready = false;

        OSCManager.OSCinstance.roomData = oscRoomData;
    }

    public void Update()
    {
        if (Managers.instance.state != GAME_STATE.ROOM)
        {
            return;
        }

        if (OSCManager.OSCinstance.roomData.myID != 0) { return; }

        int cnt = 0;
        int[] teamCount = new int[2];

        for (int i = 0; i < MachingRoomData.playerMaxCount; i++)
        {
            RoomData roomData = OSCManager.OSCinstance.GetRoomData(i);
            if (roomData.myID != MachingRoomData.bannerEmpty)
            {
                cnt++;
                if (roomData.myTeamNum >= 0) { teamCount[roomData.myTeamNum]++; }
            }
        }
        OSCManager.OSCinstance.roomData.teamACount = teamCount[0];
        OSCManager.OSCinstance.roomData.teamBCount = teamCount[1];
        OSCManager.OSCinstance.roomData.playerCount = cnt;
    }

    //チームの振り分けを行う関数
    public void PlayerBannerDivider()
    {
        RoomData myRoomData = OSCManager.OSCinstance.GetRoomData(Managers.instance.playerID);
        RoomData hostRoomData = OSCManager.OSCinstance.GetRoomData(0);

        int applyTeam = 0;
        if (hostRoomData.teamACount > hostRoomData.teamBCount) { applyTeam = 1; }

        myRoomData.myTeamNum = applyTeam;
        OSCManager.OSCinstance.roomData = myRoomData;
    }
    //チーム移動を行う関数
    public void PlayerBannerChanger(int _num)
    {
        RoomData myRoomData = OSCManager.OSCinstance.GetRoomData(Managers.instance.playerID);

        //自分のチームを呼び出そうとしたら早期リターン
        if (myRoomData.myTeamNum == _num) { return; }

        RoomData hostRoomData = OSCManager.OSCinstance.GetRoomData(0);

        int[] teamCount = new int[2] { hostRoomData.teamACount, hostRoomData.teamBCount };
        if (teamCount[_num] >= 4) { return; }

        myRoomData.myTeamNum = _num;
        OSCManager.OSCinstance.roomData = myRoomData;
    }

    public void CharaSelect(int value)
    {
        RoomData myRoomData = OSCManager.OSCinstance.roomData;
        if (myRoomData.ready) { return; }

        int calc = myRoomData.selectedCharacterID;

        int maxCharaCount = Managers.instance.gameManager.playerDatas.Length;

        if (value > 0) { calc = (calc + 1) % maxCharaCount; }
        else { calc = (calc + (maxCharaCount - 1)) % maxCharaCount; }

        myRoomData.selectedCharacterID = calc;

        OSCManager.OSCinstance.roomData = myRoomData;
    }

    public void PressSubmit()
    {
        RoomData myRoomData = OSCManager.OSCinstance.roomData;

        //自分がホストなら
        if (myRoomData.myID == 0)
        {
            if (myRoomData.teamACount >= 4) { return; }
            if (myRoomData.teamBCount >= 4) { return; }

            int readyCount = 0;
            for (int i = 1; i < MachingRoomData.playerMaxCount; i++)
            {
                RoomData otherData = OSCManager.OSCinstance.GetRoomData(i);
                if (otherData.ready) { readyCount++; }
            }

            //自分以外の全プレイヤーがREADY中なら
            if (readyCount >= myRoomData.playerCount - 1)
            {
                myRoomData.gameStart = true;
                myRoomData.stageNum = Random.Range(0, Managers.instance.gameManager.allStageData.GetStageLength());
            }
        }
        else
        {
            if (!myRoomData.ready)
            {
                RoomData hostRoomData = OSCManager.OSCinstance.GetRoomData(0);
                int[] teamCount = new int[2] { hostRoomData.teamACount, hostRoomData.teamBCount };

                if (teamCount[myRoomData.myTeamNum] < 4)
                {
                    myRoomData.ready = true;
                    PlayerData pd = Managers.instance.gameManager.playerDatas[myRoomData.selectedCharacterID];
                    SoundManager.PlayVoice(pd.GetPlayerVoiceData().GetReady());
                }
            }
        }

        OSCManager.OSCinstance.roomData = myRoomData;
    }

    public void PressCancel()
    {
        RoomData myRoomData = OSCManager.OSCinstance.GetRoomData(Managers.instance.playerID);

        int myID = Managers.instance.playerID;
        bool host = (myID == 0);

        if (host)
        {

        }
        else
        {
            if (myRoomData.ready) { myRoomData.ready = false; }
        }

        OSCManager.OSCinstance.roomData = myRoomData;
    }

    public void BackToTitle()
    {
        OSCManager.OSCinstance.ExitToRoom();
        Managers.instance.ChangeScene(GAME_STATE.TITLE);
        Managers.instance.ChangeState(GAME_STATE.TITLE);
        Init();
    }
}

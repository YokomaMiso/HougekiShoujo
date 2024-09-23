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
    public int nowPlayerCount = 1;

    public void Init()
    {
        RoomData oscRoomData = OSCManager.OSCinstance.roomData;
        for (int i = 0; i < MachingRoomData.playerMaxCount; i++) { oscRoomData.SetReadyPlayers(i, false); }
        oscRoomData.gameStart = false;

        OSCManager.OSCinstance.roomData = oscRoomData;
    }

    public void Update()
    {
        TidyUpPlayerBanner();

        int cnt = 0;
        for (int i = 0; i < MachingRoomData.playerMaxCount; i++)
        {
            RoomData roomData = OSCManager.OSCinstance.GetRoomData(i);
            if (roomData.myBannerNum != MachingRoomData.bannerEmpty) { cnt++; }
        }
        nowPlayerCount = cnt;
    }

    public int GetBannerNumFromAllPlayer(int _num)
    {
        int[] allBannerNum = new int[8];
        for (int i = 0; i < MachingRoomData.bannerMaxCount; i++) { allBannerNum[i] = -1; }

        //移動したいチームに空きがあれば番号を振る
        for (int i = 0; i < MachingRoomData.playerMaxCount; i++)
        {
            //全プレイヤーのbannerの番号を読み取る
            RoomData oscRoomData = OSCManager.OSCinstance.GetRoomData(i);
            if (oscRoomData.myBannerNum != MachingRoomData.bannerEmpty)
            {
                allBannerNum[oscRoomData.myBannerNum] = i;
            }
        }

        return allBannerNum[_num];
    }

    //チームの振り分けを行う関数
    public void PlayerBannerDivider()
    {
        int[] allBannerNum = new int[8];
        for (int i = 0; i < 8; i++) { allBannerNum[i] = GetBannerNumFromAllPlayer(i); }

        RoomData myRoomData = OSCManager.OSCinstance.GetRoomData(Managers.instance.playerID);
        for (int i = 0; i < MachingRoomData.bannerMaxCount; i++)
        {
            if (allBannerNum[i] == MachingRoomData.bannerEmpty)
            {
                //myRoomData.SetBannerNum(i, Managers.instance.playerID);
                myRoomData.myBannerNum = i;
                break;
            }
        }

        OSCManager.OSCinstance.roomData = myRoomData;
    }
    //チーム移動を行う関数
    public void PlayerBannerChanger(int _num)
    {
        RoomData myRoomData = OSCManager.OSCinstance.GetRoomData(Managers.instance.playerID);

        //自分のチームを呼び出そうとしたら早期リターン
        if (myRoomData.myBannerNum % 2 == _num) { return; }

        int myNowBannerNum = myRoomData.myBannerNum;
        int[] allBannerNum = new int[8];
        for (int i = 0; i < 8; i++) { allBannerNum[i] = GetBannerNumFromAllPlayer(i); }

        bool canMove = false;
        int nextNum = 0;
        //移動したいチームに空きがあれば番号を振る
        for (int i = 0; i < MachingRoomData.bannerMaxCount; i++)
        {
            if (i % 2 == _num && allBannerNum[i] == MachingRoomData.bannerEmpty)
            {
                //myRoomData.SetBannerNum(i, Managers.instance.playerID);
                canMove = true;
                nextNum = i;

                break;
            }
        }

        //移動しようとしたチームに空きがなければ何もせず早期リターン
        if (canMove)
        {
            //自分の位置の番号を更新
            myRoomData.myBannerNum = nextNum;
        }

        OSCManager.OSCinstance.roomData = myRoomData;
    }

    void TidyUpPlayerBanner()
    {
        RoomData myRoomData = OSCManager.OSCinstance.roomData;

        //自分がチームの一番上に存在するなら早期リターン
        if (myRoomData.myBannerNum < 2) { return; }

        int[] allPlayerNum = new int[8];
        for (int i = 0; i < 8; i++) { allPlayerNum[i] = Managers.instance.roomManager.GetBannerNumFromAllPlayer(i); }

        //自分の１つ上が空いてるなら
        int targetNum = myRoomData.myBannerNum - 2;
        if (allPlayerNum[targetNum] == MachingRoomData.bannerEmpty)
        {
            //移動する
            myRoomData.myBannerNum = targetNum;
            OSCManager.OSCinstance.roomData = myRoomData;
        }

        return;
    }

    public void CharaSelect(int value)
    {
        RoomData myRoomData = OSCManager.OSCinstance.roomData;
        if (myRoomData.GetReadyPlayers(myRoomData.myID)) { return; }

        int calc = myRoomData.GetSelectedCharacterID(Managers.instance.playerID);

        int maxCharaCount = Managers.instance.gameManager.playerDatas.Length;

        if (value > 0) { calc = (calc + 1) % maxCharaCount; }
        else { calc = (calc + (maxCharaCount - 1)) % maxCharaCount; }

        myRoomData.SetSelectedCharacterID(Managers.instance.playerID, calc);

        OSCManager.OSCinstance.roomData = myRoomData;
    }

    public void PressSubmit()
    {
        int myID = Managers.instance.playerID;
        bool host = (myID == 0);

        RoomData myRoomData = OSCManager.OSCinstance.GetRoomData(Managers.instance.playerID);

        //自分がホストなら
        if (host)
        {
            int readyCount = 0;
            for (int i = 0; i < MachingRoomData.playerMaxCount; i++)
            {
                RoomData otherData = OSCManager.OSCinstance.GetRoomData(i);
                if (otherData.GetReadyPlayers(i)) { readyCount++; }
            }

            //自分以外の全プレイヤーがREADY中なら
            if (readyCount >= nowPlayerCount - 1)
            {
                myRoomData.gameStart = true;
            }
        }
        else
        {
            if (!myRoomData.GetReadyPlayers(myID))
            {
                myRoomData.SetReadyPlayers(myID, true);
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
            if (myRoomData.GetReadyPlayers(myID))
            {
                myRoomData.SetReadyPlayers(myID, false);
            }
        }

        OSCManager.OSCinstance.roomData = myRoomData;
    }
}

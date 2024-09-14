using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using static MachingRoomData;

public class RoomManager : MonoBehaviour
{
    /*内部的な処理*/
    public readonly int maxCharaCount = 3;
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
        int cnt = 0;
        for (int i = 0; i < MachingRoomData.playerMaxCount; i++)
        {
            RoomData roomData = OSCManager.OSCinstance.GetRoomData(i);
            if (roomData.myBannerNum != MachingRoomData.bannerEmpty) { cnt++; }
        }
        nowPlayerCount = cnt;
    }

    public int[] GetAllBannerNum()
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

        return allBannerNum;
    }

    //チームの振り分けを行う関数
    public void PlayerBannerDivider()
    {
        int[] allBannerNum = GetAllBannerNum();

        RoomData myRoomData = OSCManager.OSCinstance.GetRoomData(Managers.instance.playerID);
        for (int i = 0; i < MachingRoomData.bannerMaxCount; i++) 
        {
            if(allBannerNum[i] == MachingRoomData.bannerEmpty)
            {
                myRoomData.SetBannerNum(i, Managers.instance.playerID);
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

        int[] allBannerNum = GetAllBannerNum();

        bool canMove = false;
        int nextNum = 0;
        //移動したいチームに空きがあれば番号を振る
        for (int i = 0; i < MachingRoomData.bannerMaxCount; i++)
        {
            if (i % 2 == _num && allBannerNum[i] == MachingRoomData.bannerEmpty)
            {
                myRoomData.SetBannerNum(i, Managers.instance.playerID);
                canMove = true;
                nextNum = i;
                break;
            }
        }

        //移動しようとしたチームに空きがなければ何もせず早期リターン
        if (canMove)
        {
            //チームの移動に成功したら、前居た自分の位置をクリアする
            myRoomData.SetBannerNum(myRoomData.myBannerNum, MachingRoomData.bannerEmpty);
            //自分の位置の番号を更新
            myRoomData.myBannerNum = nextNum;

            while (true)
            {
                //配列の整理整頓
                if (!TidyUpPlayerBanner(myRoomData)) { break; }
            }
        }

        OSCManager.OSCinstance.roomData = myRoomData;
    }

    bool TidyUpPlayerBanner(RoomData _roomData)
    {
        bool isTidied = false;
        RoomData myRoomData = OSCManager.OSCinstance.GetRoomData(Managers.instance.playerID);

        for (int i = 0; i < MachingRoomData.bannerMaxCount - 2; i++)
        {
            //中身が空なら
            if (_roomData.GetBannerNum(i) == empty)
            {
                //１つ下の中身が空じゃないなら
                if (_roomData.GetBannerNum(i + 2) != empty)
                {
                    //１つ下の情報を自分の中身に入れ替える
                    _roomData.SetBannerNum(i, _roomData.GetBannerNum(i + 2));
                    _roomData.SetBannerNum(i + 2, MachingRoomData.bannerEmpty);
                    //自分の番号だった場合、番号を更新する
                    if (myRoomData.myBannerNum == i) { myRoomData.myBannerNum = i - 2; }

                    isTidied = true;
                }
            }
        }

        return isTidied;
    }

    public void CharaSelect(int value)
    {
        RoomData myRoomData = OSCManager.OSCinstance.GetRoomData(Managers.instance.playerID);
        int calc = myRoomData.GetSelectedCharacterID(Managers.instance.playerID);

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

            //if (readyCount >= Managers.instance.gameManager.allPlayerCount - 1)
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

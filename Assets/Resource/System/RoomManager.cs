using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MachingRoomData;

public class RoomManager : MonoBehaviour
{
    /*内部的な処理*/
    public int myNum = -1;
    public readonly int maxCharaCount = 3;
    public readonly int empty = -1;

    void SendDataToServer()
    {
        if (Managers.instance.onDebug) { return; }
        OSCManager.OSCinstance.SendRoomData();
    }
    public void Init()
    {
        RoomData oscRoomData = OSCManager.OSCinstance.roomData;
        for (int i = 0; i < MachingRoomData.playerMaxCount; i++) { oscRoomData.SetReadyPlayers(i, false); }
        oscRoomData.gameStart = false;

        OSCManager.OSCinstance.roomData = oscRoomData;
    }

    //チームの振り分けを行う関数
    public void PlayerBannerDivider()
    {
        RoomData oscRoomData;
        int id = Managers.instance.playerID;
        if (id == 0) { oscRoomData = OSCManager.OSCinstance.roomData; }
        else { oscRoomData = OSCManager.OSCinstance.receiveRoomData; }

        //移動したいチームに空きがあれば番号を振る
        for (int i = 0; i < MachingRoomData.bannerMaxCount; i++)
        {
            if (oscRoomData.GetBannerNum(i) == empty)
            {
                oscRoomData.SetBannerNum(i, Managers.instance.playerID);
                myNum = i;
                break;
            }
        }

        OSCManager.OSCinstance.roomData = oscRoomData;

        int myID = Managers.instance.playerID;
        SendDataToServer();
    }
    //チーム移動を行う関数
    public void PlayerBannerChanger(int _num)
    {
        RoomData oscRoomData = OSCManager.OSCinstance.roomData;

        //自分のチームを呼び出そうとしたら早期リターン
        if (myNum % 2 == _num) { return; }

        bool canMove = false;
        int nextNum = 0;
        //移動したいチームに空きがあれば番号を振る
        for (int i = 0; i < MachingRoomData.bannerMaxCount; i++)
        {
            if (i % 2 == _num && oscRoomData.GetBannerNum(i) == empty)
            {
                oscRoomData.SetBannerNum(i, Managers.instance.playerID);
                canMove = true;
                nextNum = i;
                break;
            }
        }

        //移動しようとしたチームに空きがなければ何もせず早期リターン
        if (canMove)
        {

            //チームの移動に成功したら、前居た自分の位置をクリアする
            oscRoomData.SetBannerNum(myNum, MachingRoomData.bannerEmpty);
            //自分の位置の番号を更新
            myNum = nextNum;

            while (true)
            {
                //配列の整理整頓
                if (!TidyUpPlayerBanner(oscRoomData)) { break; }
            }
        }

        OSCManager.OSCinstance.roomData = oscRoomData;

        int myID = Managers.instance.playerID;
        SendDataToServer();
    }

    bool TidyUpPlayerBanner(RoomData _roomData)
    {
        bool isTidied = false;

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
                    if (myNum == i) { myNum = i - 2; }

                    isTidied = true;
                }
            }
        }

        return isTidied;
    }

    public void CharaSelect(int _playerID, int value)
    {
        RoomData oscRoomData = OSCManager.OSCinstance.roomData;
        int calc = oscRoomData.GetSelectedCharacterID(_playerID);

        if (value > 0) { calc = (calc + 1) % maxCharaCount; }
        else { calc = (calc + (maxCharaCount - 1)) % maxCharaCount; }

        oscRoomData.SetSelectedCharacterID(_playerID, calc);

        OSCManager.OSCinstance.roomData = oscRoomData;
        SendDataToServer();
    }

    public void PressSubmit()
    {
        //DEBUG
        if (Managers.instance.onDebug)
        {
            OSCManager.OSCinstance.roomData.gameStart = true;
            OSCManager.OSCinstance.receiveRoomData.gameStart = true;
            return;
        }

        RoomData oscRoomData;
        int myID = Managers.instance.playerID;

        //自分がホストなら
        if (0 == myID)
        {
            oscRoomData = OSCManager.OSCinstance.receiveRoomData;

            int readyCount = 0;
            for (int i = 0; i < MachingRoomData.playerMaxCount; i++)
            {
                if (oscRoomData.GetReadyPlayers(i)) { readyCount++; }
            }

            Debug.Log(readyCount);
            
            if (readyCount >= Managers.instance.gameManager.allPlayerCount - 1)
            {
                OSCManager.OSCinstance.roomData.gameStart = true;
                Debug.Log("シーン変えるよ");
            }
        }
        else
        {
            oscRoomData = OSCManager.OSCinstance.roomData;

            if (!oscRoomData.GetReadyPlayers(myID))
            {
                oscRoomData.SetReadyPlayers(myID, true);
                OSCManager.OSCinstance.roomData = oscRoomData;
            }
        }

        SendDataToServer();
    }
    public void PressCancel()
    {
        RoomData oscRoomData = OSCManager.OSCinstance.roomData;

        int myID = Managers.instance.playerID;

        if (oscRoomData.hostPlayer == myID)
        {

        }
        else
        {
            if (oscRoomData.GetReadyPlayers(myID))
            {
                oscRoomData.SetReadyPlayers(myID, false);

            }
        }

        OSCManager.OSCinstance.roomData = oscRoomData;
        SendDataToServer();
    }
}

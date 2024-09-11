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
        OSCManager.OSCinstance.SendRoomData();
    }
    public unsafe void Init()
    {
        RoomData oscRoomData = OSCManager.OSCinstance.receiveRoomData;
        for (int i = 0; i < MachingRoomData.playerMaxCount; i++) { oscRoomData.readyPlayers[i] = false; }
        oscRoomData.gameStart = false;

        OSCManager.OSCinstance.roomData = oscRoomData;
    }

    //チームの振り分けを行う関数
    public unsafe void PlayerBannerDivider()
    {
        RoomData oscRoomData = OSCManager.OSCinstance.receiveRoomData;

        //移動したいチームに空きがあれば番号を振る
        for (int i = 0; i < MachingRoomData.bannerMaxCount; i++)
        {
            if (oscRoomData.bannerNum[i] == empty)
            {
                oscRoomData.bannerNum[i] = Managers.instance.playerID;
                myNum = i;
                break;
            }
        }

        OSCManager.OSCinstance.roomData = oscRoomData;

        int myID = Managers.instance.playerID;
        SendDataToServer();
    }
    //チーム移動を行う関数
    public unsafe void PlayerBannerChanger(int _num)
    {
        RoomData oscRoomData = OSCManager.OSCinstance.receiveRoomData;

        //自分のチームを呼び出そうとしたら早期リターン
        if (myNum % 2 == _num) { return; }

        bool canMove = false;
        int nextNum = 0;
        //移動したいチームに空きがあれば番号を振る
        for (int i = 0; i < MachingRoomData.bannerMaxCount; i++)
        {
            if (i % 2 == _num && oscRoomData.bannerNum[i] == empty)
            {
                oscRoomData.bannerNum[i] = Managers.instance.playerID;
                canMove = true;
                nextNum = i;
                break;
            }
        }

        //移動しようとしたチームに空きがなければ何もせず早期リターン
        if (canMove)
        {

            //チームの移動に成功したら、前居た自分の位置をクリアする
            oscRoomData.bannerNum[myNum] = empty;
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

    unsafe bool TidyUpPlayerBanner(RoomData _roomData)
    {
        bool isTidied = false;

        for (int i = 0; i < MachingRoomData.bannerMaxCount - 2; i++)
        {
            //中身が空なら
            if (_roomData.bannerNum[i] == empty)
            {
                //１つ下の中身が空じゃないなら
                if (_roomData.bannerNum[i + 2] != empty)
                {
                    //１つ下の情報を自分の中身に入れ替える
                    _roomData.bannerNum[i] = _roomData.bannerNum[i + 2];
                    _roomData.bannerNum[i + 2] = empty;
                    //自分の番号だった場合、番号を更新する
                    if (myNum == i) { myNum = i - 2; }

                    isTidied = true;
                }
            }
        }

        return isTidied;
    }

    public unsafe void CharaSelect(int _playerID, int value)
    {
        RoomData oscRoomData = OSCManager.OSCinstance.receiveRoomData;

        int calc = oscRoomData.selectedCharacterID[_playerID];

        if (value > 0) { calc = (calc + 1) % maxCharaCount; }
        else { calc = (calc + (maxCharaCount - 1)) % maxCharaCount; }

        oscRoomData.selectedCharacterID[_playerID] = calc;

        OSCManager.OSCinstance.roomData = oscRoomData;
        SendDataToServer();
    }

    public unsafe void PressSubmit()
    {
        RoomData oscRoomData = OSCManager.OSCinstance.receiveRoomData;

        //DEBUG
        //oscRoomData.gameStart = true;
        //return;

        int myID = Managers.instance.playerID;

        //自分がホストなら
        if (oscRoomData.hostPlayer == myID)
        {
            int readyCount = 0;
            for (int i = 0; i < MachingRoomData.playerMaxCount; i++)
            {
                if (oscRoomData.readyPlayers[i] && i != myID) { readyCount++; }
            }

            if (readyCount >= Managers.instance.gameManager.allPlayerCount)
            {
                oscRoomData.gameStart = true;
            }
        }
        else
        {
            if (!oscRoomData.readyPlayers[myID])
            {
                oscRoomData.readyPlayers[myID] = true;
            }
        }

        OSCManager.OSCinstance.roomData = oscRoomData;
        SendDataToServer();
    }
    public unsafe void PressCancel()
    {
        RoomData oscRoomData = OSCManager.OSCinstance.receiveRoomData;

        int myID = Managers.instance.playerID;

        if (oscRoomData.hostPlayer == myID)
        {

        }
        else
        {
            if (oscRoomData.readyPlayers[myID])
            {
                oscRoomData.readyPlayers[myID] = false;
            }
        }

        OSCManager.OSCinstance.roomData = oscRoomData;
        SendDataToServer();
    }
}

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

    public void Init()
    {
        RoomData oscRoomData = OSCManager.OSCinstance.roomData;
        for (int i = 0; i < MachingRoomData.playerMaxCount; i++) { oscRoomData.SetReadyPlayers(i, false); }
        oscRoomData.gameStart = false;

        OSCManager.OSCinstance.roomData = oscRoomData;
    }

    public RoomData ReadRoomData(bool _isMine)
    {
        RoomData returnData;
        string debugText;

        if (_isMine) { returnData= OSCManager.OSCinstance.roomData; debugText = "自分"; }
        else { returnData= OSCManager.OSCinstance.receiveRoomData; debugText = "相手"; }

        Debug.Log(debugText + "0番目" + returnData.GetBannerNum(0));

        return returnData;
    }

    //チームの振り分けを行う関数
    public void PlayerBannerDivider()
    {
        bool host = Managers.instance.playerID == 0;
        RoomData oscRoomData = ReadRoomData(host);

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
    }
    //チーム移動を行う関数
    public void PlayerBannerChanger(int _num)
    {
        RoomData oscRoomData = ReadRoomData(false);

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
        RoomData oscRoomData = ReadRoomData(true);
        int calc = oscRoomData.GetSelectedCharacterID(_playerID);

        if (value > 0) { calc = (calc + 1) % maxCharaCount; }
        else { calc = (calc + (maxCharaCount - 1)) % maxCharaCount; }

        oscRoomData.SetSelectedCharacterID(_playerID, calc);

        OSCManager.OSCinstance.roomData = oscRoomData;
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

        int myID = Managers.instance.playerID;
        bool host = (myID == 0);

        RoomData oscRoomData = ReadRoomData(!host);

        //自分がホストなら
        if (host)
        {
            int readyCount = 0;
            for (int i = 0; i < MachingRoomData.playerMaxCount; i++)
            {
                if (oscRoomData.GetReadyPlayers(i)) { readyCount++; }
            }

            if (readyCount >= Managers.instance.gameManager.allPlayerCount - 1)
            {
                OSCManager.OSCinstance.roomData.gameStart = true;
            }
        }
        else
        {
            if (!oscRoomData.GetReadyPlayers(myID))
            {
                oscRoomData.SetReadyPlayers(myID, true);
            }
        }

        OSCManager.OSCinstance.roomData = oscRoomData;
    }

    public void PressCancel()
    {
        RoomData oscRoomData = ReadRoomData(true);

        int myID = Managers.instance.playerID;
        bool host = (myID == 0);

        if (host)
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
    }
}

using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    /*サーバーと同期させる変数*/
    public int[] bannerNum = new int[8] { -1, -1, -1, -1, -1, -1, -1, -1 };
    public int[] selectedCharacterID = new int[6] { 0, 0, 0, 0, 0, 0 };
    public bool[] readyPlayers = new bool[6] { false, false, false, false, false, false };
    public int hostPlayer;
    public bool gameStart;

    /*内部的な処理*/
    public int myNum = -1;
    public readonly int maxCharaCount = 3;
    public readonly int empty = -1;

    void ReceiveDataFromServer()
    {
        //受信処理
    }
    void SendDataToServer(int _playerID, int _bannerID, int _characterID)
    {
        //送信処理を行う
    }

    public void Init()
    {
        for (int i = 0; i < readyPlayers.Length; i++) { readyPlayers[i] = false; }
        gameStart = false;
    }

    //チームの振り分けを行う関数
    public void PlayerBannerDivider()
    {
        //移動したいチームに空きがあれば番号を振る
        for (int i = 0; i < bannerNum.Length; i++)
        {
            if (bannerNum[i] == empty)
            {
                bannerNum[i] = Managers.instance.playerID;
                myNum = i;
                break;
            }
        }

        int myID = Managers.instance.playerID;
        SendDataToServer(myID, myNum, selectedCharacterID[myID]);
    }
    //チーム移動を行う関数
    public void PlayerBannerChanger(int _num)
    {
        //自分のチームを呼び出そうとしたら早期リターン
        if (myNum % 2 == _num) { return; }

        bool canMove = false;
        int nextNum = 0;
        //移動したいチームに空きがあれば番号を振る
        for (int i = 0; i < bannerNum.Length; i++)
        {
            if (i % 2 == _num && bannerNum[i] == empty)
            {
                bannerNum[i] = Managers.instance.playerID;
                canMove = true;
                nextNum = i;
                break;
            }
        }

        //移動しようとしたチームに空きがなければ何もせず早期リターン
        if (!canMove) { return; }

        //チームの移動に成功したら、前居た自分の位置をクリアする
        bannerNum[myNum] = empty;
        //自分の位置の番号を更新
        myNum = nextNum;

        while (true)
        {
            //配列の整理整頓
            if (!TidyUpPlayerBanner()) { break; }
        }

        int myID = Managers.instance.playerID;
        SendDataToServer(myID, myNum, selectedCharacterID[myID]);
    }

    bool TidyUpPlayerBanner()
    {
        bool isTidied = false;

        for (int i = 0; i < bannerNum.Length - 2; i++)
        {
            //中身が空なら
            if (bannerNum[i] == empty)
            {
                //１つ下の中身が空じゃないなら
                if (bannerNum[i + 2] != empty)
                {
                    //１つ下の情報を自分の中身に入れ替える
                    bannerNum[i] = bannerNum[i + 2];
                    bannerNum[i + 2] = empty;
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
        int calc = selectedCharacterID[_playerID];

        if (value > 0) { calc = (calc + 1) % maxCharaCount; }
        else { calc = (calc + (maxCharaCount - 1)) % maxCharaCount; }

        selectedCharacterID[_playerID] = calc;
    }

    public void PressSubmit()
    {
        //DEBUG
        gameStart = true;
        return;

        int myID = Managers.instance.playerID;

        //自分がホストなら
        if (hostPlayer == myID)
        {
            int readyCount = 0;
            for (int i = 0; i < readyPlayers.Length; i++)
            {
                if (readyPlayers[i] && i != myID) { readyCount++; }
            }

            if (readyCount >= Managers.instance.gameManager.allPlayerCount)
            {
                gameStart = true;
            }
        }
        else
        {
            if (!readyPlayers[myID])
            {
                readyPlayers[myID] = true;
            }
        }
    }
    public void PressCancel()
    {
        int myID = Managers.instance.playerID;

        if (hostPlayer == myID)
        {

        }
        else
        {
            if (readyPlayers[myID])
            {
                readyPlayers[myID] = false;
            }
        }
    }
}

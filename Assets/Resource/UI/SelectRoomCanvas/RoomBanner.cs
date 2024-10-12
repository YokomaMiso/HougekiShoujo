using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomBanner : MonoBehaviour
{
    Text roomNumText;
    Text isPlayingText;
    Text memberCountText;
    Image hostIcon;
    Text[] memberTexts;

    void AssignChild()
    {
        roomNumText = transform.GetChild(0).GetComponent<Text>();
        isPlayingText = transform.GetChild(1).GetComponent<Text>();
        memberCountText = transform.GetChild(3).GetComponent<Text>();
        hostIcon = transform.GetChild(4).GetComponent<Image>();
        for (int i = 0; i < MachingRoomData.playerMaxCount; i++)
        {
            memberTexts[i] = transform.GetChild(5 + i).GetComponent<Text>();
        }
    }

    public void SetData(AllGameData.AllData[] _data, int _num = 0)
    {
        //部屋番号
        roomNumText.text = "Room " + (_num + 1).ToString();

        //プレイ中なら表示
        if (_data[0].rData.gameStart) {  }
        //プレイしていないなら透明に
        else { isPlayingText.color = Color.clear; }

        //メンバー数
        memberCountText.text = _data[0].rData.playerCount.ToString() + "/6";

        //メンバーの名前
        for (int i = 0; i < MachingRoomData.playerMaxCount; i++)
        {
            //居なければ透明にする
            if (_data[i].rData.myID == -1) 
            {
                memberTexts[i].color = Color.clear;
                continue;
            }

            //名前の適用
            memberTexts[i].text = _data[i].rData.playerName;
        }
    }

}

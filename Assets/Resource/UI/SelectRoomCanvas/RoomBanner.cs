using System;
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
    Text memberText;

    void AssignChild()
    {
        roomNumText = transform.GetChild(0).GetComponent<Text>();
        isPlayingText = transform.GetChild(1).GetComponent<Text>();
        memberCountText = transform.GetChild(3).GetComponent<Text>();
        hostIcon = transform.GetChild(4).GetComponent<Image>();
        memberText = transform.GetChild(5).GetComponent<Text>(); 
    }

    public void InitBannerText()
    {
        //部屋番号
        roomNumText.text = "Room ";
        //プレイ中かどうか
        isPlayingText.color=Color.clear;
        //メンバー数
        memberCountText.text = "0/6";
        //ホストアイコン
        hostIcon.color = Color.clear;
        //名前の適用
        memberText.text = "Empty";
    }

    public void SetData(AllGameData.AllData _data, int _num = 0)
    {
        //子オブジェクトのポインタに実体を指定
        AssignChild();

        //部屋番号
        roomNumText.text = "Room " + (_num + 1).ToString();

        //プレイ中なら表示
        if (_data.rData.gameStart) { isPlayingText.color = Color.red; }
        //プレイしていないなら透明に
        else { isPlayingText.color = Color.clear; }

        //メンバー数
        memberCountText.text = _data.rData.playerCount.ToString() + "/6";

        //ホストアイコン
        if (_data.rData.playerCount == 0) { hostIcon.color = Color.clear; }
        else { hostIcon.color = Color.white; }

        //名前の適用
        if (_data.rData.playerCount == 0) { memberText.text = "Empty"; }
        else { memberText.text = _data.rData.playerName; }
    }

}

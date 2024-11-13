using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomBanner : MonoBehaviour
{
    [SerializeField] Sprite[] bannerSprites;

    public int roomNum;

    Text roomNumText;
    Image isPlaying;
    Text memberCountText;
    Text hostText;
    Text hostName;

    SelectRoomCanvasBehavior parent;
    public void SetParent(SelectRoomCanvasBehavior _parent) { parent = _parent; }

    public void PressThisBanner()
    {
        parent.DecideButtonSelectFromUI(roomNum);
    }

    void AssignChild()
    {
        roomNumText = transform.GetChild(0).GetComponent<Text>();
        isPlaying = transform.GetChild(1).GetComponent<Image>();
        memberCountText = transform.GetChild(2).GetComponent<Text>();
        hostText = transform.GetChild(3).GetComponent<Text>();
        hostName = transform.GetChild(4).GetComponent<Text>();
    }

    public void InitBannerText()
    {
        //部屋番号
        roomNumText.text = "Room ";
        //プレイ中かどうか
        isPlaying.color = Color.clear;
        //メンバー数
        memberCountText.text = "0/6";
        //ホストアイコン
        hostText.color = Color.clear;
        //名前の適用
        hostName.text = "Empty";

        GetComponent<Image>().sprite = bannerSprites[UnityEngine.Random.Range(0, bannerSprites.Length)];
    }

    public void SetData(AllGameData.AllData _data, int _num = 0)
    {
        //子オブジェクトのポインタに実体を指定
        AssignChild();

        //部屋番号
        roomNum = _num;
        roomNumText.text = (_num + 1).ToString();

        //プレイ中なら表示
        if (_data.rData.gameStart) { isPlaying.color = Color.white; }
        //プレイしていないなら透明に
        else { isPlaying.color = Color.clear; }

        //メンバー数
        memberCountText.text = _data.rData.playerCount.ToString() + "/6";

        //ホストアイコン
        if (_data.rData.playerCount == 0) { hostText.text = ""; }
        else { hostText.text = "Host"; }

        //名前の適用
        if (_data.rData.playerCount == 0) { hostName.text = "Empty"; }
        else { hostName.text = _data.rData.playerName; }
    }

}

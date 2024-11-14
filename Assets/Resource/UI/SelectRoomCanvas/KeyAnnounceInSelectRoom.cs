using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static MachingRoomData;

public class KeyAnnounceInSelectRoom : MonoBehaviour
{
    [SerializeField] SelectRoomCanvasBehavior srcb;

    [SerializeField] GameObject horizon;
    [SerializeField] GameObject vertical;
    [SerializeField] GameObject submit;
    [SerializeField] GameObject cancel;
    [SerializeField] GameObject leftShoulder;
    GameObject[] keys;
    const int maxKeyCount = 5;

    void Start()
    {
        keys = new GameObject[maxKeyCount];
        keys[0] = horizon;
        keys[1] = vertical;
        keys[2] = submit;
        keys[3] = cancel;
        keys[4] = leftShoulder;

        keys[1].transform.GetChild(1).GetComponent<Text>().text = "カーソル移動";
        keys[2].transform.GetChild(1).GetComponent<Text>().text = "部屋を作成";
        keys[3].transform.GetChild(1).GetComponent<Text>().text = "タイトルに戻る";
        keys[4].transform.GetChild(1).GetComponent<Text>().text = "部屋の更新";

        ChangeDisplayButtons();
    }

    void Update()
    {
        if (InputManager.isChangedController) { ChangeDisplayButtons(); }

        //カーソルがルーム内に居る時の挙動
        if (srcb.selectRoomNum < srcb.maxRoomNum)
        {
            //0人なら
            if (srcb.roomBannerList[srcb.selectRoomNum].memberCount == 0)
            {
                keys[2].transform.GetChild(1).GetComponent<Text>().text = "部屋を作成";
            }
            else
            {
                keys[2].transform.GetChild(1).GetComponent<Text>().text = "部屋に参加";
            }
        }
        //カーソルが上方向のボタンに居る時
        else if (srcb.selectRoomNum < srcb.maxRoomNum + srcb.headerButonNum)
        {
            //タイトルに戻る
            if (srcb.selectRoomNum == srcb.maxRoomNum)
            {
                keys[2].transform.GetChild(1).GetComponent<Text>().text = "タイトルに戻る";
            }
            //ルーム情報の更新
            else
            {
                keys[2].transform.GetChild(1).GetComponent<Text>().text = "部屋の更新";
            }
        }
        //カーソルが下方向のボタンに居る時
        else
        {
            keys[2].transform.GetChild(1).GetComponent<Text>().text = "ランダムマッチ";
        }

        MachingRoomData.RoomData roomData = OSCManager.OSCinstance.roomData;
    }

    void ChangeDisplayButtons()
    {
        Sprite[] applySprites = InputManager.nowButtonSpriteData.GetAllSprites();
        for (int i = 0; i < maxKeyCount; i++) { keys[i].transform.GetChild(0).GetComponent<Image>().sprite = applySprites[i]; }
    }

    bool ReadyChecker(MachingRoomData.RoomData _myData)
    {
        int readyCount = 0;
        for (int i = 1; i < MachingRoomData.playerMaxCount; i++)
        {
            RoomData otherData = OSCManager.OSCinstance.GetRoomData(i);
            if (otherData.ready) { readyCount++; }
        }

        //自分以外の全プレイヤーがREADY中なら
        if (readyCount >= _myData.playerCount - 1)
        {
#if UNITY_EDITOR
            return true;
#else
            if(readyCount <= 0){ return false; }
            else{ return true; }
#endif
        }

        return false;
    }
}

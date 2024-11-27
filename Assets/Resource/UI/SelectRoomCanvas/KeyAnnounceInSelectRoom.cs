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

    readonly string[] cursorMoveText = new string[(int)LANGUAGE_NUM.MAX_NUM]
    {
        "カーソル移動",
        "Cursor Move",
        "鼠标移动",
        "鼠標移動",
    };
    readonly string[] createRoomText = new string[(int)LANGUAGE_NUM.MAX_NUM]
    {
        "部屋を作成",
        "Create Room",
        "创建房间",
        "創建房間",
    };
    readonly string[] backToTitleText = new string[(int)LANGUAGE_NUM.MAX_NUM]
    {
        "タイトルに戻る",
        "Back to Title",
        "返回主页面",
        "返回主頁面",
    };
    readonly string[] refreshText = new string[(int)LANGUAGE_NUM.MAX_NUM]
    {
        "部屋の更新",
        "Refresh Room",
        "刷新房间",
        "刷新房間",
    };
    readonly string[] joinToRoomText = new string[(int)LANGUAGE_NUM.MAX_NUM]
    {
        "部屋に参加",
        "Join to Room",
        "加入房间",
        "加入房間",
    };
    readonly string[] randomMatchText = new string[(int)LANGUAGE_NUM.MAX_NUM]
    {
        "ランダムマッチ",
        "Random Match",
        "随机参加",
        "隨機參加",
    };

    void Start()
    {
        if (Managers.instance.GetSmartPhoneFlag())
        {
            Destroy(gameObject);
            return;
        }

        keys = new GameObject[maxKeyCount];
        keys[0] = horizon;
        keys[1] = vertical;
        keys[2] = submit;
        keys[3] = cancel;
        keys[4] = leftShoulder;

        keys[1].transform.GetChild(1).GetComponent<Text>().text = cursorMoveText[(int)Managers.instance.nowLanguage];
        keys[2].transform.GetChild(1).GetComponent<Text>().text = createRoomText[(int)Managers.instance.nowLanguage];
        keys[3].transform.GetChild(1).GetComponent<Text>().text = backToTitleText[(int)Managers.instance.nowLanguage];
        keys[4].transform.GetChild(1).GetComponent<Text>().text = refreshText[(int)Managers.instance.nowLanguage];

        ChangeDisplayButtons();
    }

    void Update()
    {
        if (InputManager.isChangedController) { ChangeDisplayButtons(); }

        //カーソルがルーム内に居る時の挙動
        if (srcb.selectRoomNum < srcb.maxRoomNum)
        {
            if (srcb.roomBannerList[srcb.selectRoomNum].memberCount >= 6)
            {
                keys[2].transform.GetChild(1).GetComponent<Text>().text = "";
            }
            else if (srcb.roomBannerList[srcb.selectRoomNum].isPlaying)
            {
                keys[2].transform.GetChild(1).GetComponent<Text>().text = "";
            }
            //0人なら
            else if (srcb.roomBannerList[srcb.selectRoomNum].memberCount == 0)
            {
                keys[2].transform.GetChild(1).GetComponent<Text>().text = createRoomText[(int)Managers.instance.nowLanguage];
            }
            else
            {
                keys[2].transform.GetChild(1).GetComponent<Text>().text = joinToRoomText[(int)Managers.instance.nowLanguage]; ;
            }
        }
        //カーソルが上方向のボタンに居る時
        else if (srcb.selectRoomNum < srcb.maxRoomNum + srcb.headerButonNum)
        {
            //タイトルに戻る
            if (srcb.selectRoomNum == srcb.maxRoomNum)
            {
                keys[2].transform.GetChild(1).GetComponent<Text>().text = backToTitleText[(int)Managers.instance.nowLanguage]; ;
            }
            //ルーム情報の更新
            else
            {
                keys[2].transform.GetChild(1).GetComponent<Text>().text = refreshText[(int)Managers.instance.nowLanguage]; ;
            }
        }
        //カーソルが下方向のボタンに居る時
        else
        {
            keys[2].transform.GetChild(1).GetComponent<Text>().text = randomMatchText[(int)Managers.instance.nowLanguage]; ;
        }
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

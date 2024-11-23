using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static MachingRoomData;

public class KeyAnnounceInRoom : MonoBehaviour
{
    [SerializeField] GameObject stageSelectCanvas;

    [SerializeField] GameObject horizon;
    [SerializeField] GameObject vertical;
    [SerializeField] GameObject submit;
    [SerializeField] GameObject cancel;
    [SerializeField] GameObject leftShoulder;
    GameObject[] keys;
    const int maxKeyCount = 5;

    readonly string[] characterChangeText = new string[(int)LANGUAGE_NUM.MAX_NUM]
    {
        "キャラクター変更",
        "Change Character",
        "人物选择",
        "人物選擇",
    };
    readonly string[] teamChangeText = new string[(int)LANGUAGE_NUM.MAX_NUM]
    {
        "チーム変更",
        "Change Team",
        "换队",
        "換隊",
    };
    readonly string[] startText = new string[(int)LANGUAGE_NUM.MAX_NUM]
    {
        "スタート",
        "START",
        "开始",
        "開始",
    };
    readonly string[] dissolutionText = new string[(int)LANGUAGE_NUM.MAX_NUM]
    {
        "解散",
        "Disband",
        "解散",
        "解散",
    };
    readonly string[] stageChangeText = new string[(int)LANGUAGE_NUM.MAX_NUM]
    {
        "ステージ変更",
        "Change Stage",
        "地图选择",
        "地圖選擇",
    };
    readonly string[] stageDecideText = new string[(int)LANGUAGE_NUM.MAX_NUM]
{
        "ステージ決定",
        "Decide Stage",
        "确定地图",
        "確定地圖",
};
    readonly string readyText ="READY";
    readonly string[] exitText = new string[(int)LANGUAGE_NUM.MAX_NUM]
    {
        "退出",
        "Exit",
        "退出",
        "退出",
    };
    readonly string[] cancelText = new string[(int)LANGUAGE_NUM.MAX_NUM]
    {
        "READY解除",
        "Cancel",
        "解除READY",
        "解除READY",
    };

    void Start()
    {
        keys = new GameObject[maxKeyCount];
        keys[0] = horizon;
        keys[1] = vertical;
        keys[2] = submit;
        keys[3] = cancel;
        keys[4] = leftShoulder;

        if (Managers.instance.playerID == 0)
        {
            keys[0].transform.GetChild(1).GetComponent<Text>().text = characterChangeText[(int)Managers.instance.nowLanguage];
            keys[1].transform.GetChild(1).GetComponent<Text>().text=teamChangeText[(int)Managers.instance.nowLanguage];
            keys[2].transform.GetChild(1).GetComponent<Text>().text = startText[(int)Managers.instance.nowLanguage];
            keys[2].SetActive(false);
            keys[3].transform.GetChild(1).GetComponent<Text>().text = dissolutionText[(int)Managers.instance.nowLanguage];
            keys[4].transform.GetChild(1).GetComponent<Text>().text = startText[(int)Managers.instance.nowLanguage];
        }
        else
        {
            keys[0].transform.GetChild(1).GetComponent<Text>().text = characterChangeText[(int)Managers.instance.nowLanguage];
            keys[1].transform.GetChild(1).GetComponent<Text>().text = teamChangeText[(int)Managers.instance.nowLanguage];
            keys[2].transform.GetChild(1).GetComponent<Text>().text = readyText;
            keys[3].transform.GetChild(1).GetComponent<Text>().text = exitText[(int)Managers.instance.nowLanguage];
            keys[4].SetActive(false);
        }

        ChangeDisplayButtons();
    }

    void Update()
    {
        if (InputManager.isChangedController) { ChangeDisplayButtons(); }

        MachingRoomData.RoomData roomData = OSCManager.OSCinstance.roomData;

        //ホストなら
        if (Managers.instance.playerID == 0)
        {
            if (stageSelectCanvas.activeInHierarchy)
            {
                keys[0].transform.GetChild(1).GetComponent<Text>().text = stageChangeText[(int)Managers.instance.nowLanguage];
                keys[1].SetActive(false);
                keys[2].transform.GetChild(1).GetComponent<Text>().text = stageDecideText[(int)Managers.instance.nowLanguage];
                keys[2].SetActive(true);
                keys[3].SetActive(false);
                keys[4].SetActive(false);
            }
            else
            {
                keys[0].transform.GetChild(1).GetComponent<Text>().text = characterChangeText[(int)Managers.instance.nowLanguage];
                keys[1].transform.GetChild(1).GetComponent<Text>().text = teamChangeText[(int)Managers.instance.nowLanguage];
                keys[1].SetActive(true);
                keys[2].transform.GetChild(1).GetComponent<Text>().text = startText[(int)Managers.instance.nowLanguage];
                keys[2].SetActive(ReadyChecker(roomData));
                keys[3].transform.GetChild(1).GetComponent<Text>().text = dissolutionText[(int)Managers.instance.nowLanguage];
                keys[3].SetActive(true);
                keys[4].transform.GetChild(1).GetComponent<Text>().text = stageChangeText[(int)Managers.instance.nowLanguage];
                keys[4].SetActive(true);
            }
        }
        else
        {
            if (roomData.ready)
            {
                keys[0].SetActive(false);
                keys[1].SetActive(false);
                keys[2].SetActive(false);
                keys[3].transform.GetChild(1).GetComponent<Text>().text = cancelText[(int)Managers.instance.nowLanguage];
            }
            else
            {
                keys[0].SetActive(true);
                keys[1].SetActive(true);
                keys[2].SetActive(true);
                keys[3].transform.GetChild(1).GetComponent<Text>().text = exitText[(int)Managers.instance.nowLanguage];
            }
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

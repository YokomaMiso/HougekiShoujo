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

    [SerializeField] Sprite[] gamePadButtons;
    [SerializeField] Sprite[] dualSenseButtons;
    [SerializeField] Sprite[] keyButtons;

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
            keys[0].transform.GetChild(1).GetComponent<Text>().text = "キャラクター変更";
            keys[1].transform.GetChild(1).GetComponent<Text>().text = "チーム変更";
            keys[2].transform.GetChild(1).GetComponent<Text>().text = "START";
            keys[2].SetActive(false);
            keys[3].transform.GetChild(1).GetComponent<Text>().text = "解散";
            keys[4].transform.GetChild(1).GetComponent<Text>().text = "マップ変更";
        }
        else
        {
            keys[0].transform.GetChild(1).GetComponent<Text>().text = "キャラクター変更";
            keys[1].transform.GetChild(1).GetComponent<Text>().text = "チーム変更";
            keys[2].transform.GetChild(1).GetComponent<Text>().text = "READY";
            keys[3].transform.GetChild(1).GetComponent<Text>().text = "退出";
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
                keys[0].transform.GetChild(1).GetComponent<Text>().text = "ステージ変更";
                keys[1].SetActive(false);
                keys[2].transform.GetChild(1).GetComponent<Text>().text = "ステージ決定";
                keys[2].SetActive(true);
                keys[3].SetActive(false);
                keys[4].SetActive(false);
            }
            else
            {
                keys[0].transform.GetChild(1).GetComponent<Text>().text = "キャラクター変更";
                keys[1].transform.GetChild(1).GetComponent<Text>().text = "チーム変更";
                keys[1].SetActive(true);
                keys[2].transform.GetChild(1).GetComponent<Text>().text = "START";
                keys[2].SetActive(ReadyChecker(roomData));
                keys[3].transform.GetChild(1).GetComponent<Text>().text = "解散";
                keys[3].SetActive(true);
                keys[4].transform.GetChild(1).GetComponent<Text>().text = "マップ変更";
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
                keys[3].transform.GetChild(1).GetComponent<Text>().text = "READY解除";
            }
            else
            {
                keys[0].SetActive(true);
                keys[1].SetActive(true);
                keys[2].SetActive(true);
                keys[3].transform.GetChild(1).GetComponent<Text>().text = "退出";
            }
        }
    }

    void ChangeDisplayButtons()
    {
        Sprite[] applySprites;
        switch (InputManager.currentController)
        {
            case ControllerType.XInput:
                applySprites = gamePadButtons;
                break;
            case ControllerType.DirectInput:
                applySprites = dualSenseButtons;
                break;
            default:
                applySprites = keyButtons;
                break;
        }

        for (int i = 0; i < maxKeyCount; i++)
        {
            keys[i].transform.GetChild(0).GetComponent<Image>().sprite = applySprites[i];
        }
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

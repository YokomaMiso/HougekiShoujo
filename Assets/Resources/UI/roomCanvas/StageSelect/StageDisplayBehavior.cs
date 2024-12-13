using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class StageDisplayBehavior : MonoBehaviour
{
    string[] stageAnnounceText = new string[(int)LANGUAGE_NUM.MAX_NUM]
    {
        "ステージ",
        "Stage",
        "地图",
        "地圖",
    };

    void Update()
    {
        int stageNum = OSCManager.OSCinstance.GetRoomData(0).stageNum;

        StageData sd = Managers.instance.gameManager.allStageData.GetStageData(stageNum);
        transform.GetChild(0).GetComponent<Image>().sprite = sd.GetScreenShot();
        transform.GetChild(1).GetComponent<Text>().text = stageAnnounceText[(int)Managers.instance.nowLanguage] + "\n" + sd.GetStageName();
    }
}

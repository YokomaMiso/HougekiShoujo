using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInGameTimer : MonoBehaviour
{
    Text timerText;

    void Start()
    {
        timerText = transform.GetChild(0).GetComponent<Text>();
    }

    void Update()
    {
        IngameData.GameData hostIngameData = OSCManager.OSCinstance.GetIngameData(0).mainPacketData.inGameData;

        float timer = hostIngameData.roundTimer;
        if (timer > 60) { timer = 60; }
        if (timer <= 0) { timer = 0; }

        string second = Mathf.FloorToInt(timer % 60).ToString("f0").PadLeft(2, '0');
        timerText.text = second;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInGameTimer : MonoBehaviour
{
    Image timeNiddle;

    void Start()
    {
        timeNiddle = transform.GetChild(0).GetComponent<Image>();
    }

    void Update()
    {
        IngameData.GameData hostIngameData = OSCManager.OSCinstance.GetIngameData(0).mainPacketData.inGameData;

        float timer = hostIngameData.roundTimer;
        if (timer > 60) { timer = 60; }
        if (timer <= 0) { timer = 0; }

        float nowAngle = timer - 60.0f;
        timeNiddle.transform.localRotation = Quaternion.Euler(0, 0, nowAngle * 6);
    }
}

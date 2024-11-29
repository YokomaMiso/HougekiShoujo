using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RoundCheckerSubWeapon : MonoBehaviour
{
    int nowRound;
    bool isHost;

    void Start()
    {
        isHost = (Managers.instance.playerID == 0);

        IngameData.GameData hostIngameData;
        if (isHost) { hostIngameData = OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData; }
        else { hostIngameData = OSCManager.OSCinstance.GetIngameData(0).mainPacketData.inGameData; }
        nowRound = hostIngameData.roundCount;
    }

    void Update()
    {
        IngameData.GameData hostIngameData;
        if (isHost) { hostIngameData = OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData; }
        else { hostIngameData = OSCManager.OSCinstance.GetIngameData(0).mainPacketData.inGameData; }
        if (nowRound != hostIngameData.roundCount) { Destroy(gameObject); }
    }
}

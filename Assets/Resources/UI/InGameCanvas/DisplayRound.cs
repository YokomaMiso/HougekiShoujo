using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayRound : MonoBehaviour
{
    Text roundText;
    int roundNumber;

    void Start()
    {
        roundText = GetComponent<Text>();
    }
    void Update()
    {
        IngameData.GameData hostIngameData = OSCManager.OSCinstance.GetIngameData(0).mainPacketData.inGameData;

        if (roundNumber != hostIngameData.roundCount)
        {
            roundNumber = hostIngameData.roundCount;
            roundText.text = roundNumber.ToString();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayWinCount : MonoBehaviour
{
    [SerializeField] int teamNum = 0;
    [SerializeField] Sprite[] scoreSprites;

    void Update()
    {
        IngameData.GameData hostIngameData = OSCManager.OSCinstance.GetIngameData(0).mainPacketData.inGameData;
        int score;
        if (teamNum == (int)TEAM_NUM.A) { score = hostIngameData.winCountTeamA; }
        else { score = hostIngameData.winCountTeamB; }

        for (int i = 0; i < transform.childCount; i++)
        {
            int spriteNum = 0;
            if (i < score) { spriteNum = 1; }

            transform.GetChild(i).GetComponent<Image>().sprite = scoreSprites[spriteNum];
        }
    }
}

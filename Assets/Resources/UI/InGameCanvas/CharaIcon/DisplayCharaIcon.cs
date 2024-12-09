using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCharaIcon : MonoBehaviour
{
    readonly Vector2[] iconStartPos = new Vector2[6]
    {
        new Vector2(-1100,0),
        new Vector2(1100,0),

        new Vector2(-1228,0),
        new Vector2(1228,0),

        new Vector2(-1356,0),
        new Vector2(1356,0),
    };
    readonly Vector2[] iconLineUpPos = new Vector2[6]
    {
        new Vector2(-160,0),
        new Vector2(160,0),

        new Vector2(-288,0),
        new Vector2(288,0),

        new Vector2(-416,0),
        new Vector2(416,0),
    };
    readonly Vector2[] iconPos = new Vector2[6]
    {
        new Vector2(-100,436),
        new Vector2(100,436),

        new Vector2(-228,436),
        new Vector2(228,436),

        new Vector2(-356,436),
        new Vector2(356,436),
    };

    readonly Color[] iconBGColor = new Color[2];

    int[] iconNums = new int[6] { -1, -1, -1, -1, -1, -1 };

    bool attachedToPlacement = false;
    bool[] sfxPlayed = new bool[6] { false, false, false, false, false, false };

    [SerializeField] AudioClip iconPlace;

    void Start()
    {
        int[] teamCount = new int[2] { 0, 0 };
        for (int i = 0; i < transform.childCount; i++)
        {
            MachingRoomData.RoomData oscRoomData;
            if (Managers.instance.playerID == i) { oscRoomData = OSCManager.OSCinstance.roomData; }
            else { oscRoomData = OSCManager.OSCinstance.GetRoomData(i); }

            if (oscRoomData.myTeamNum == MachingRoomData.bannerEmpty) { continue; }

            iconNums[i] = oscRoomData.myTeamNum + teamCount[oscRoomData.myTeamNum] * 2;
            transform.GetChild(iconNums[i]).gameObject.SetActive(true);

            Sprite icon = Managers.instance.gameManager.playerDatas[oscRoomData.selectedCharacterID].GetCharacterAnimData().GetCharaIcon();
            transform.GetChild(iconNums[i]).GetChild(0).GetComponent<Image>().sprite = icon;
            transform.GetChild(iconNums[i]).GetComponent<Image>().color = Managers.instance.ColorCordToRGB(oscRoomData.myTeamNum);
            transform.GetChild(iconNums[i]).transform.localPosition = iconStartPos[iconNums[i]];

            teamCount[oscRoomData.myTeamNum]++;
        }
    }
    void Update()
    {
        IngameData.GameData hostIngameData = OSCManager.OSCinstance.GetIngameData(0).mainPacketData.inGameData;

        if (!attachedToPlacement)
        {
            float timer = hostIngameData.startTimer;

            if (0.5f <= timer && timer < 0.65f) { FirstBehavior(Mathf.Clamp01((timer - 0.5f) / 0.15f), 0); }
            if (0.65f <= timer && timer < 0.8f) { FirstBehavior(Mathf.Clamp01((timer - 0.65f) / 0.15f), 2); FirstBehavior(1, 0); }
            if (0.8f <= timer && timer < 0.95f) { FirstBehavior(Mathf.Clamp01((timer - 0.8f) / 0.15f), 4); FirstBehavior(1, 2); }
            if (0.95f <= timer && timer < 1.5f) { FirstBehavior(1, 4); }

            if (1.5f <= timer && timer < 1.65f) { SecondBehavior(Mathf.Clamp01((timer - 1.5f) / 0.15f), 0); }
            if (1.65f <= timer && timer < 1.8f) { SecondBehavior(Mathf.Clamp01((timer - 1.65f) / 0.15f), 2); SecondBehavior(1, 0); }
            if (1.8f <= timer && timer < 1.95f) { SecondBehavior(Mathf.Clamp01((timer - 1.8f) / 0.15f), 4); SecondBehavior(1, 2); }
            if (1.95f <= timer && timer < 2.5f) { SecondBehavior(1, 4); }

            if (2.5f <= timer)
            {
                InPlacement();
                attachedToPlacement = true;
            }
        }
        DisplayIconUpdate();
    }

    void DisplayIconUpdate()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            MachingRoomData.RoomData oscRoomData = OSCManager.OSCinstance.GetRoomData(i);

            if (oscRoomData.myID == MachingRoomData.bannerEmpty) { continue; }

            IngameData.GameData gameData;
            if (i == Managers.instance.playerID) { gameData = OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData; }
            else { gameData = OSCManager.OSCinstance.GetIngameData(i).mainPacketData.inGameData; }
            if (gameData.alive)
            {
                transform.GetChild(iconNums[i]).GetChild(0).GetComponent<Image>().color = Color.white;
                transform.GetChild(iconNums[i]).GetComponent<Image>().color = iconBGColor[oscRoomData.myTeamNum];
            }
            else
            {
                transform.GetChild(iconNums[i]).GetChild(0).GetComponent<Image>().color = Color.gray * 0.5f;
                transform.GetChild(iconNums[i]).GetComponent<Image>().color = Color.gray;
            }
        }
    }

    void FirstBehavior(float _rate, int _num)
    {
        for (int i = _num; i < _num + 2; i++)
        {
            if (iconNums[i] == -1) { continue; }
            Vector2 pos = Vector2.Lerp(iconStartPos[iconNums[i]], iconLineUpPos[iconNums[i]], _rate);
            transform.GetChild(iconNums[i]).transform.localPosition = pos;
        }
    }

    void SecondBehavior(float _rate, int _num)
    {
        for (int i = _num; i < _num + 2; i++)
        {
            if (iconNums[i] == -1) { continue; }
            Vector2 pos = Vector2.Lerp(iconLineUpPos[iconNums[i]], iconPos[iconNums[i]], _rate);
            transform.GetChild(iconNums[i]).transform.localPosition = pos;
            if (!sfxPlayed[i]) { SoundManager.PlaySFXForUI(iconPlace); }
        }

        for (int i = _num; i < _num + 2; i++) { sfxPlayed[i] = true; }
    }

    void InPlacement()
    {
        for (int i = 0; i < MachingRoomData.playerMaxCount; i++)
        {
            if (iconNums[i] == -1) { continue; }
            Vector2 pos = iconPos[iconNums[i]];
            transform.GetChild(iconNums[i]).transform.localPosition = pos;
        }
    }
}

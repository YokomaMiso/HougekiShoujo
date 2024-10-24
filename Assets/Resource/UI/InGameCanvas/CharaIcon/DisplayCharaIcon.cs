using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCharaIcon : MonoBehaviour
{
    readonly Vector2[] iconStartPos = new Vector2[6]
    {
        new Vector2(-1000,0),
        new Vector2(1000,0),

        new Vector2(-1150,0),
        new Vector2(1150,0),

        new Vector2(-1300,0),
        new Vector2(1300,0),
    };
    readonly Vector2[] iconLineUpPos = new Vector2[6]
    {
        new Vector2(-250,0),
        new Vector2(250,0),

        new Vector2(-400,0),
        new Vector2(400,0),

        new Vector2(-550,0),
        new Vector2(550,0),
    };
    readonly Vector2[] iconPos = new Vector2[6]
    {
        new Vector2(-765,460),
        new Vector2(765,460),

        new Vector2(-660,380),
        new Vector2(660,380),

        new Vector2(-775,290),
        new Vector2(775,290),
    };

    readonly Color[] iconBGColor = new Color[2];

    RADIO_CHAT_ID[] prevRadioChatID = new RADIO_CHAT_ID[6] { RADIO_CHAT_ID.NONE, RADIO_CHAT_ID.NONE, RADIO_CHAT_ID.NONE, RADIO_CHAT_ID.NONE, RADIO_CHAT_ID.NONE, RADIO_CHAT_ID.NONE };
    [SerializeField] GameObject serifWindow;

    int[] iconNums = new int[6] { -1, -1, -1, -1, -1, -1 };

    bool attachedToPlacement = false;

    void Start()
    {
        iconBGColor[0] = ColorCordToRGB("#8fdee5");
        iconBGColor[1] = ColorCordToRGB("#ff1f1f");

        int[] teamCount = new int[2] { 0, 0 };
        for (int i = 0; i < transform.childCount; i++)
        {
            MachingRoomData.RoomData oscRoomData = OSCManager.OSCinstance.GetRoomData(i);

            if (oscRoomData.myTeamNum == MachingRoomData.bannerEmpty)
            {
                transform.GetChild(i).gameObject.SetActive(false);
                continue;
            }

            Sprite icon = Managers.instance.gameManager.playerDatas[oscRoomData.selectedCharacterID].GetCharacterAnimData().GetCharaIcon();
            transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = icon;
            transform.GetChild(i).GetComponent<Image>().color = iconBGColor[oscRoomData.myTeamNum];
            iconNums[i] = oscRoomData.myTeamNum + teamCount[oscRoomData.myTeamNum] * 2;
            transform.GetChild(i).transform.localPosition = iconStartPos[iconNums[i]];
            teamCount[oscRoomData.myTeamNum]++;
        }
    }
    void Update()
    {
        IngameData.GameData hostIngameData = OSCManager.OSCinstance.GetIngameData(0).mainPacketData.inGameData;

        if (!attachedToPlacement)
        {
            float timer = hostIngameData.startTimer;

            if (0.5f <= timer && timer < 0.65f) { FirstBehavior((timer - 0.5f) / 0.15f, 0); }
            if (0.65f <= timer && timer < 0.8f) { FirstBehavior((timer - 0.65f) / 0.15f, 2); FirstBehavior(1, 0); }
            if (0.8f <= timer && timer < 0.95f) { FirstBehavior((timer - 0.8f) / 0.15f, 4); FirstBehavior(1, 2); }
            if (0.95f <= timer && timer < 1.5f) { FirstBehavior(1, 4); }

            if (1.5f <= timer && timer < 1.65f) { SecondBehavior((timer - 1.5f) / 0.15f, 0); }
            if (1.65f <= timer && timer < 1.8f) { SecondBehavior((timer - 1.65f) / 0.15f, 2); SecondBehavior(1, 0); }
            if (1.8f <= timer && timer < 1.95f) { SecondBehavior((timer - 1.8f) / 0.15f, 4); SecondBehavior(1, 2); }
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
                transform.GetChild(i).GetChild(0).GetComponent<Image>().color = Color.white;
                transform.GetChild(i).GetComponent<Image>().color = iconBGColor[oscRoomData.myTeamNum];
                if (prevRadioChatID[i] != gameData.playerChatID)
                {
                    if(gameData.playerChatID == RADIO_CHAT_ID.NONE)
                    {

                    }
                    else if (gameData.playerChatID <= RADIO_CHAT_ID.SUPPORT)
                    {
                        GameObject window = Instantiate(serifWindow, transform.GetChild(i).GetChild(0));
                        window.GetComponent<CharaSerifBehavior>().SetSerif(oscRoomData.myTeamNum, (int)gameData.playerChatID);
                    }
                    else
                    {
                        Managers.instance.gameManager.GetPlayer(i).GetComponent<Player>().PlayEmote(gameData.playerChatID);
                    }
                    prevRadioChatID[i] = gameData.playerChatID;
                }
            }
            else
            {
                transform.GetChild(i).GetChild(0).GetComponent<Image>().color = Color.gray * 0.5f;
                transform.GetChild(i).GetComponent<Image>().color = Color.gray;
            }
        }
    }

    void FirstBehavior(float _rate, int _num)
    {
        for (int i = _num; i < _num + 2; i++)
        {
            if (iconNums[i] == -1) { continue; }
            Vector2 pos = Vector2.Lerp(iconStartPos[iconNums[i]], iconLineUpPos[iconNums[i]], _rate);
            transform.GetChild(i).transform.localPosition = pos;
        }
    }

    void SecondBehavior(float _rate, int _num)
    {
        for (int i = _num; i < _num + 2; i++)
        {
            if (iconNums[i] == -1) { continue; }
            Vector2 pos = Vector2.Lerp(iconLineUpPos[iconNums[i]], iconPos[iconNums[i]], _rate);
            transform.GetChild(i).transform.localPosition = pos;
        }
    }

    void InPlacement()
    {
        for (int i = 0; i < MachingRoomData.playerMaxCount; i++)
        {
            if (iconNums[i] == -1) { continue; }
            Vector2 pos =  iconPos[iconNums[i]];
            transform.GetChild(i).transform.localPosition = pos;
        }
    }

    Color ColorCordToRGB(string hex)
    {
        if (ColorUtility.TryParseHtmlString(hex, out Color color)) return color;
        else return Color.black;
    }
}

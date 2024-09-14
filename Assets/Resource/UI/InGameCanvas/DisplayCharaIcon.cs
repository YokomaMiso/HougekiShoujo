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

    void Start()
    {
        iconBGColor[0] = ColorCordToRGB("#8fdee5");
        iconBGColor[1] = ColorCordToRGB("#ff1f1f");

        RoomManager rm = Managers.instance.roomManager;
        int[] allBannerNum = rm.GetAllBannerNum();

        for (int i = 0; i < transform.childCount; i++)
        {
            if (allBannerNum[i] == MachingRoomData.bannerEmpty) 
            {
                transform.GetChild(i).gameObject.SetActive(false);
                continue; 
            }
            MachingRoomData.RoomData roomData = OSCManager.OSCinstance.GetRoomData(allBannerNum[i]);

            int nowPlayerID = roomData.GetBannerNum(i);

                
                int charaID = roomData.GetSelectedCharacterID(nowPlayerID);
                Sprite icon = Managers.instance.gameManager.playerDatas[charaID].GetCharacterAnimData().GetCharaIcon();
                transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = icon;
                transform.GetChild(i).GetComponent<Image>().color = iconBGColor[i % 2];
                transform.GetChild(i).transform.localPosition = iconStartPos[i];
        }
    }
    void Update()
    {
        float timer = Managers.instance.gameManager.startTimer;

        if (0.5f <= timer && timer < 0.65f) { FirstBehavior((timer - 0.5f) / 0.15f, 0); }
        if (0.65f <= timer && timer < 0.8f) { FirstBehavior((timer - 0.65f) / 0.15f, 2); FirstBehavior(1, 0); }
        if (0.8f <= timer && timer < 0.95f) { FirstBehavior((timer - 0.8f) / 0.15f, 4); FirstBehavior(1, 2); }
        if (0.95f <= timer && timer < 1.5f) { FirstBehavior(1, 4); }

        if (1.5f <= timer && timer < 1.65f) { SecondBehavior((timer - 1.5f) / 0.15f, 0); }
        if (1.65f <= timer && timer < 1.8f) { SecondBehavior((timer - 1.65f) / 0.15f, 2); SecondBehavior(1, 0); }
        if (1.8f <= timer && timer < 1.95f) { SecondBehavior((timer - 1.8f) / 0.15f, 4); SecondBehavior(1, 2); }
        if (1.95f <= timer && timer < 2.5f) { SecondBehavior(1, 4); }
    }
    void FirstBehavior(float _rate, int _num)
    {
        for (int i = _num; i < _num + 2; i++)
        {
            Vector2 pos = Vector2.Lerp(iconStartPos[i], iconLineUpPos[i], _rate);
            transform.GetChild(i).transform.localPosition = pos;
        }
    }

    void SecondBehavior(float _rate, int _num)
    {
        for (int i = _num; i < _num + 2; i++)
        {
            Vector2 pos = Vector2.Lerp(iconLineUpPos[i], iconPos[i], _rate);
            transform.GetChild(i).transform.localPosition = pos;
        }
    }
    Color ColorCordToRGB(string hex)
    {
        if (ColorUtility.TryParseHtmlString(hex, out Color color)) return color;
        else return Color.black;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharaSerifEmote : CharaSerifBehavior
{
    [SerializeField] Sprite[] emotes;

    public override void SetSerif(MachingRoomData.RoomData _roomData, RADIO_CHAT_ID _chatID)
    {
        if (_chatID >= RADIO_CHAT_ID.MAX_NUM || _chatID <= RADIO_CHAT_ID.APOLOGIZE) { _chatID = RADIO_CHAT_ID.APOLOGIZE; }

        chatBG = transform.GetComponent<Image>();
        playerName = transform.GetChild(0).GetComponent<Text>();
        charaIcon = transform.GetChild(1).GetComponent<Image>();

        chatBG.sprite = bgSprite[_roomData.myTeamNum];

        playerName.text = _roomData.playerName;

        charaIcon.sprite = emotes[_chatID - RADIO_CHAT_ID.APOLOGIZE];

        transform.localPosition = startPos;
    }
}

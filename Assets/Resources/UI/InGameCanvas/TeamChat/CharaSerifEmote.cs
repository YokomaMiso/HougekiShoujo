using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharaSerifEmote : CharaSerifBehavior
{
    [SerializeField] Sprite[] emotes;
    readonly Vector3 overrideStartPos = new Vector3(-1220, 330);
    readonly Vector3 overrideEndPos = new Vector3(-800, 330);

    public override void SetSerif(MachingRoomData.RoomData _roomData, RADIO_CHAT_ID _chatID)
    {
        startPos = overrideStartPos;
        endPos = overrideEndPos;

        if (_chatID >= RADIO_CHAT_ID.MAX_NUM || _chatID <= RADIO_CHAT_ID.APOLOGIZE) { _chatID = RADIO_CHAT_ID.APOLOGIZE; }

        chatBG = transform.GetComponent<Image>();
        playerName = transform.GetChild(0).GetComponent<Text>();
        charaIcon = transform.GetChild(1).GetComponent<Image>();

        chatBG.sprite = bgSprite[_roomData.myTeamNum];

        playerName.text = _roomData.playerName;

        charaIcon.sprite = emotes[_chatID - RADIO_CHAT_ID.APOLOGIZE];

        transform.localPosition = startPos;
    }

    protected override void Update()
    {
        timer += Time.deltaTime;
        float nowRate = Mathf.Clamp01(timer / arriveTime);
        transform.localPosition = Vector3.Lerp(startPos, endPos, nowRate) + Vector3.down * (chatNum * posAdd);

        if (timer > fadeStart)
        {
            float fadeRate = 1.0f - (timer - fadeStart) / (lifeTime - fadeStart);
            Color fadeColor = new Color(1, 1, 1, fadeRate);
            chatBG.color = chatBG.color * fadeColor;
            playerName.color = playerName.color * fadeColor;
            charaIcon.color = charaIcon.color * fadeColor;
        }

        if (timer > lifeTime) { Destroy(gameObject); }
    }
}

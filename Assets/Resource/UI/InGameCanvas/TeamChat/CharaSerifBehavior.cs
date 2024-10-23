using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharaSerifBehavior : MonoBehaviour
{
    Image chatBG;
    Text playerName;
    Image charaIcon;

    [SerializeField] Sprite[] bgSprite;

    readonly static Vector3 startPos = new Vector3(-1170, 330);
    readonly static Vector3 endPos = new Vector3(-750, 330);

    int chatNum = 0;
    const int posAdd = 128;

    const float arriveTime = 0.25f;
    const float lifeTime = 5;
    const float fadeStart = 4.5f;

    float timer = 0;

    public void SetSerif(MachingRoomData.RoomData _roomData, RADIO_CHAT_ID _chatID)
    {
        chatBG = transform.GetComponent<Image>();
        playerName = transform.GetChild(0).GetComponent<Text>();
        charaIcon = transform.GetChild(1).GetComponent<Image>();

        chatBG.sprite = bgSprite[_roomData.myTeamNum * 4 + (int)_chatID - 1];

        playerName.text = _roomData.playerName;

        charaIcon.sprite = Managers.instance.gameManager.playerDatas[_roomData.selectedCharacterID].GetCharacterAnimData().GetCharaIcon();

        transform.localPosition = startPos;
    }

    public void ChatNumAdd() { chatNum++; }

    void Update()
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

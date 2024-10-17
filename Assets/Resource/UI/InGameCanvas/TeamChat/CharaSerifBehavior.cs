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

    readonly static Vector3 startPos = Vector3.left * 420;
    readonly static Vector3 endPos = Vector3.zero;

    int chatNum = 0;
    const int posAdd = 128;

    const float arriveTime = 0.25f;
    const float lifeTime = 3;

    float timer = 0;

    public void SetSerif(MachingRoomData.RoomData _roomData, RADIO_CHAT_ID _chatID)
    {
        chatBG = transform.GetComponent<Image>();
        playerName = transform.GetChild(0).GetComponent<Text>();
        charaIcon = transform.GetChild(1).GetComponent<Image>();

        chatBG.sprite = bgSprite[_roomData.myTeamNum * 4 + (int)_chatID - 1];

        playerName.text = _roomData.playerName;
        Outline[] outLines = playerName.GetComponents<Outline>();
        outLines[outLines.Length - 1].effectColor = Managers.instance.ColorCordToRGB(_roomData.myTeamNum);

        charaIcon.sprite = Managers.instance.gameManager.playerDatas[_roomData.selectedCharacterID].GetCharacterAnimData().GetCharaIcon();

        transform.localPosition = startPos;
    }

    public void ChatNumAdd() { chatNum++; }

    void Update()
    {
        timer += Time.deltaTime;
        float nowRate = Mathf.Clamp01(timer / arriveTime);
        transform.localPosition = Vector3.Lerp(startPos, endPos, nowRate) + Vector3.down * (chatNum * posAdd);
        if (timer > lifeTime) { Destroy(gameObject); }
    }
}

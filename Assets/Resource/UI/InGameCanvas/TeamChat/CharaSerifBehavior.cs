using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharaSerifBehavior : MonoBehaviour
{
    Image chatBG;
    Text playerName;
    Image charaIcon;
    Text chatText;

    readonly static Vector3 startPos = Vector3.left * 420;
    readonly static Vector3 endPos = Vector3.zero;

    int chatNum = 0;
    const int posAdd = 128;

    const float arriveTime = 0.25f;
    const float lifeTime = 3;
    string[] serifs = new string[(int)RADIO_CHAT_ID.HELP + 1] { "", "前に出ます！", "援護します！", "後退します！", "ピンチ！助けて！" };
    readonly Color[] constColor = new Color[2] { new Color(0.1255f, 0.3137f, 0.8941f), new Color(1, 0.125f, 0.125f, 1) };

    float timer = 0;

    public void SetSerif(MachingRoomData.RoomData _roomData, RADIO_CHAT_ID _chatID)
    {
        chatBG = transform.GetComponent<Image>();
        playerName = transform.GetChild(0).GetComponent<Text>();
        charaIcon = transform.GetChild(1).GetComponent<Image>();
        chatText = transform.GetChild(2).GetComponent<Text>();

        playerName.text = _roomData.playerName;
        playerName.color = constColor[_roomData.myTeamNum];

        charaIcon.sprite = Managers.instance.gameManager.playerDatas[_roomData.selectedCharacterID].GetCharacterAnimData().GetCharaIcon();

        chatText.text = serifs[(int)_chatID];

        transform.localPosition = startPos;
    }

    public void ChatNumAdd() { chatNum++; }

    void Update()
    {
        timer += Time.deltaTime;
        float nowRate = Mathf.Clamp01(timer / arriveTime);
        transform.localPosition = Vector3.Lerp(startPos, endPos, nowRate) + Vector3.down * posAdd;
        if (timer > lifeTime) { Destroy(gameObject); }
    }
}

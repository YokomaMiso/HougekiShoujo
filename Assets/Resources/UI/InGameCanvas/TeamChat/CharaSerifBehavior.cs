using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharaSerifBehavior : MonoBehaviour
{
    protected Image chatBG;
    protected Text playerName;
    protected Image charaIcon;
    protected Text teamChatText;

    [SerializeField] protected Sprite[] bgSprite;
    [SerializeField] AudioClip[] clips;

    public readonly static Vector3 startPos = new Vector3(-1170, 330);
    public readonly static Vector3 endPos = new Vector3(-750, 330);

    protected int chatNum = 0;
    public readonly int posAdd = 128;

    public readonly float arriveTime = 0.25f;
    public readonly float lifeTime = 5;
    public readonly float fadeStart = 4.5f;

    protected float timer = 0;

    [SerializeField] string[] teamChatTextsJPN;
    [SerializeField] string[] teamChatTextsENG;
    [SerializeField] string[] teamChatTextsSCHN;
    [SerializeField] string[] teamChatTextsTCHN;

    public virtual void SetSerif(MachingRoomData.RoomData _roomData, RADIO_CHAT_ID _chatID)
    {
        chatBG = transform.GetComponent<Image>();
        playerName = transform.GetChild(0).GetComponent<Text>();
        charaIcon = transform.GetChild(1).GetComponent<Image>();
        teamChatText = transform.GetChild(2).GetComponent<Text>();

        chatBG.sprite = bgSprite[_roomData.myTeamNum];

        string applyText;
        switch (Managers.instance.nowLanguage)
        {
            default:
                applyText = teamChatTextsJPN[(int)_chatID - 1];
                break;
            case LANGUAGE_NUM.ENGLISH:
                applyText = teamChatTextsENG[(int)_chatID - 1];
                break;
            case LANGUAGE_NUM.SIMPLE_CHINESE:
                applyText = teamChatTextsSCHN[(int)_chatID - 1];
                break;
            case LANGUAGE_NUM.TRADITIONAL_CHINESE:
                applyText = teamChatTextsTCHN[(int)_chatID - 1];
                break;
        }
        teamChatText.text = applyText;


        playerName.text = _roomData.playerName;

        charaIcon.sprite = Managers.instance.gameManager.playerDatas[_roomData.selectedCharacterID].GetCharacterAnimData().GetCharaIcon();

        transform.localPosition = startPos;

        SoundManager.PlaySFXForUI(clips[(int)_chatID - 1]);
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
            teamChatText.color = teamChatText.color * fadeColor;
        }

        if (timer > lifeTime) { Destroy(gameObject); }
    }
}

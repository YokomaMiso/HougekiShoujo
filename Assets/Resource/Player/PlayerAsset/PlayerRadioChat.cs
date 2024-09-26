using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RADIO_CHAT_ID { NONE = 0, HELP, BLITZ, SUPPORT, RETREAT, APOLOGIZE, LAUGH, WHAT, AAA, MAX_NUM }

public class PlayerRadioChat : MonoBehaviour
{
    Player ownerPlayer;
    public void SetPlayer(Player _player) { ownerPlayer = _player; }

    [SerializeField] GameObject emotePrefab;
    [SerializeField] GameObject fastChatCanvasPrefab;
    GameObject fastChatInstance = null;

    RADIO_CHAT_ID radioChatID;

    const float resetTime = 5.0f;
    float timer;

    void Update()
    {
        if (!ownerPlayer.IsMine()) { return; }
        if (!ownerPlayer.GetAlive()) { return; }

        if (radioChatID == RADIO_CHAT_ID.NONE)
        {
            ButtonCheck();
            /*
            float horizontal = Input.GetAxis("HorizontalArrow");
            float vertical = Input.GetAxis("VerticalArrow");

            if (horizontal < 0) { radioChatID = RADIO_CHAT_ID.HELP; }
            else if (horizontal > 0) { radioChatID = RADIO_CHAT_ID.SUPPORT; }
            else if (vertical > 0) { radioChatID = RADIO_CHAT_ID.BLITZ; }
            else if (vertical < 0) { radioChatID = RADIO_CHAT_ID.APOLOGIZE; }
            else { return; }
            */
        }
        else
        {
            timer += Time.deltaTime;
            if (timer > resetTime)
            {
                timer = 0;
                radioChatID = RADIO_CHAT_ID.NONE;
            }
        }
        OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData.playerChatID = radioChatID;
    }

    public void DisplayEmote(RADIO_CHAT_ID _ID)
    {
        GameObject obj = Instantiate(emotePrefab, transform);
        obj.GetComponent<EmoteBehavior>().SetSpriteNum(_ID);
    }

    void ButtonCheck()
    {
        if (Input.GetButtonDown("LB"))
        {
            if (fastChatInstance == null)
            {
                fastChatInstance = Instantiate(fastChatCanvasPrefab);
                fastChatInstance.GetComponent<FastChat>().ReceiverFromRadioChat(true);
            }
        }

        if (Input.GetButtonUp("LB"))
        {
            if (fastChatInstance != null)
            {
                int nowRegion = fastChatInstance.GetComponent<FastChat>().GetJoystickRegion();
                if (nowRegion >= 0) { radioChatID = (RADIO_CHAT_ID)(nowRegion + 1); }
                Destroy(fastChatInstance);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RADIO_CHAT_ID { NONE = 0, HELP, BLITZ, SUPPORT, APOLOGIZE, MAX_NUM }

public class PlayerRadioChat : MonoBehaviour
{
    Player ownerPlayer;
    public void SetPlayer(Player _player) { ownerPlayer = _player; }

    [SerializeField] GameObject emotePrefab;

    RADIO_CHAT_ID radioChatID;

    const float resetTime = 5.0f;
    float timer;

    void Update()
    {
        if (!ownerPlayer.IsMine()) { return; }
        if (!ownerPlayer.GetAlive()) { return; }

        if (radioChatID == RADIO_CHAT_ID.NONE)
        {
            float horizontal = Input.GetAxis("HorizontalArrow");
            float vertical = Input.GetAxis("VerticalArrow");

            if (Input.GetKeyDown(KeyCode.Space)) { horizontal = 1; }

            if (horizontal < 0) { radioChatID = RADIO_CHAT_ID.HELP; }
            else if (horizontal > 0) { radioChatID = RADIO_CHAT_ID.SUPPORT; }
            else if (vertical > 0) { radioChatID = RADIO_CHAT_ID.BLITZ; }
            else if (vertical < 0) { radioChatID = RADIO_CHAT_ID.APOLOGIZE; }
            else { return; }
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
}

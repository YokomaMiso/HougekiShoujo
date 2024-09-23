using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RADIO_CHAT_ID { NONE = 0, HELP, BLITZ, SUPPORT ,MAX_NUM}

public class PlayerRadioChat : MonoBehaviour
{
    Player ownerPlayer;
    public void SetPlayer(Player _player) { ownerPlayer = _player; }

    RADIO_CHAT_ID radioChatID;

    void Update()
    {
        if (!ownerPlayer.IsMine()) { return; }
        if (!ownerPlayer.GetAlive()) { return; }

        float horizontal = Input.GetAxis("HorizontalArrow");
        float vertical = Input.GetAxis("VerticalArrow");

        if (Input.GetKeyDown(KeyCode.Space)) { horizontal = 1; }

        if (horizontal < 0) { radioChatID = RADIO_CHAT_ID.HELP; }
        else if (horizontal > 0) { radioChatID = RADIO_CHAT_ID.SUPPORT; }
        else if (vertical > 0) { radioChatID = RADIO_CHAT_ID.BLITZ; }
        else { return; }

        OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData.playerChatID = radioChatID;
    }
}
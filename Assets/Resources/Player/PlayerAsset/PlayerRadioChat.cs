using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RADIO_CHAT_ID { NONE = 0, BLITZ, SUPPORT, RETREAT, HELP, APOLOGIZE, LAUGH, WHAT, PROVOC, POP_CORN, FACE_PALM, ANGRY, TEE_HEE, GOOD_JOB, MAX_NUM }
public enum RADIO_TYPE { NONE = 0, EMOTE = 9, TEXT = 4 }

public class PlayerRadioChat : MonoBehaviour
{
    Player ownerPlayer;
    public void SetPlayer(Player _player) { ownerPlayer = _player; }

    [SerializeField] GameObject emotePrefab;
    [SerializeField] GameObject TeamChatCanvasPrefab;
    [SerializeField] GameObject EmoteChatCanvasPrefab;
    [SerializeField] AudioClip openRadioChat;

    [SerializeField] Texture[] chatMainTex;
    [SerializeField] Texture[] chatColorTex;

    GameObject fastChatInstance = null;

    RADIO_CHAT_ID radioChatID;
    RADIO_TYPE radioType = RADIO_TYPE.NONE;

    const float resetTime = 1.0f;
    float timer;

    void Update()
    {
        /*
        if (!ownerPlayer.GetAlive()) 
        {
            if (fastChatInstance != null) { Destroy(fastChatInstance); }
            return; 
        }
        */

        if (ownerPlayer.IsMine())
        {
            if (radioChatID == RADIO_CHAT_ID.NONE)
            {
                ButtonCheck();
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
        else
        {
            CheckRadioChat();
        }

    }

    public void DisplayEmote(RADIO_CHAT_ID _ID)
    {
        GameObject obj = Instantiate(emotePrefab, transform);
        obj.GetComponent<EmoteBehavior>().SetSpriteNum(_ID);
    }

    void ButtonCheck()
    {
        CheckShoulderButton();
        CheckChatButton();
    }

    void CheckShoulderButton()
    {
        if (radioChatID != RADIO_CHAT_ID.NONE) { return; }

        if (InputManager.GetKeyDown(BoolActions.LeftShoulder))
        {
            if (fastChatInstance == null)
            {
                radioType = RADIO_TYPE.EMOTE;
                fastChatInstance = Instantiate(EmoteChatCanvasPrefab);
                fastChatInstance.GetComponent<FastChat>().SetChatType((int)RADIO_TYPE.EMOTE);
                if (chatMainTex[0] != null && chatColorTex[0] != null)
                {
                    fastChatInstance.GetComponent<FastChat>().GetFastChatMat().SetTexture("_MainTex", chatMainTex[0]);
                    fastChatInstance.GetComponent<FastChat>().GetFastChatMat().SetTexture("_ColorTex", chatColorTex[0]);
                    fastChatInstance.GetComponent<FastChat>().GetFastChatMat().SetFloat("_AngleOffset", 5.0f);
                }
                fastChatInstance.GetComponent<FastChat>().ReceiverFromRadioChat(true);
                SoundManager.PlaySFXForUI(openRadioChat);
            }
        }

        if (InputManager.GetKeyUp(BoolActions.LeftShoulder))
        {
            if (fastChatInstance != null)
            {
                int nowRegion = fastChatInstance.GetComponent<FastChat>().GetRegion();

                if (nowRegion >= 0)
                {
                    nowRegion += 5;
                    radioChatID = (RADIO_CHAT_ID)(nowRegion + 1);
                }

                MachingRoomData.RoomData oscRoomData = OSCManager.OSCinstance.roomData;
                SpawnSerifOrEmote(oscRoomData, radioChatID);

                Destroy(fastChatInstance);
                fastChatInstance.GetComponent<FastChat>().ResetShaderValues();
            }
        }

        if (InputManager.GetKeyDown(BoolActions.LeftTrigger))
        {
            if (fastChatInstance == null)
            {
                radioType = RADIO_TYPE.TEXT;
                fastChatInstance = Instantiate(TeamChatCanvasPrefab);
                if (chatMainTex[1] != null && chatColorTex[1] != null)
                {
                    fastChatInstance.GetComponent<FastChat>().GetFastChatMat().SetTexture("_MainTex", chatMainTex[1]);
                    fastChatInstance.GetComponent<FastChat>().GetFastChatMat().SetTexture("_ColorTex", chatColorTex[1]);
                    fastChatInstance.GetComponent<FastChat>().GetFastChatMat().SetFloat("_AngleOffset", 1.0f);
                }
                fastChatInstance.GetComponent<FastChat>().SetChatType((int)RADIO_TYPE.TEXT);
                fastChatInstance.GetComponent<FastChat>().ReceiverFromRadioChat(true);
                SoundManager.PlaySFXForUI(openRadioChat);
            }
        }

        if (InputManager.GetKeyUp(BoolActions.LeftTrigger))
        {
            if (fastChatInstance != null)
            {
                int nowRegion = fastChatInstance.GetComponent<FastChat>().GetRegion();
                if (nowRegion >= 0) { radioChatID = (RADIO_CHAT_ID)(nowRegion + 1); }

                MachingRoomData.RoomData oscRoomData = OSCManager.OSCinstance.roomData;
                SpawnSerifOrEmote(oscRoomData, radioChatID);

                Destroy(fastChatInstance);
                fastChatInstance.GetComponent<FastChat>().ResetShaderValues();
            }
        }
    }

    void CheckChatButton()
    {
        if (radioChatID != RADIO_CHAT_ID.NONE) { return; }

        int buttonMaxCount = (BoolActions.RadioChat13 + 1) - BoolActions.RadioChat1;
        for (int i = 0; i < buttonMaxCount; i++)
        {
            if (InputManager.GetKeyDown(BoolActions.RadioChat1 + (i)))
            {
                radioChatID = (RADIO_CHAT_ID)((i >= 0 && i < 9) ? (i + 5) : (i >= 9 && i < 13) ? (i - 8) : i);
                MachingRoomData.RoomData oscRoomData = OSCManager.OSCinstance.roomData;
                SpawnSerifOrEmote(oscRoomData, radioChatID);

                if (fastChatInstance != null) { Destroy(fastChatInstance); }

                break;
            }
        }
    }

    public void ReceiveChatButtonFromUI(RADIO_CHAT_ID _id)
    {
        radioChatID = _id;
        MachingRoomData.RoomData oscRoomData = OSCManager.OSCinstance.roomData;
        SpawnSerifOrEmote(oscRoomData, radioChatID);
    }

    void CheckRadioChat()
    {
        IngameData.GameData gameData = OSCManager.OSCinstance.GetIngameData(ownerPlayer.GetPlayerID()).mainPacketData.inGameData;
        MachingRoomData.RoomData oscRoomData = OSCManager.OSCinstance.GetRoomData(ownerPlayer.GetPlayerID());

        if (radioChatID != gameData.playerChatID)
        {
            SpawnSerifOrEmote(oscRoomData, gameData.playerChatID);
            radioChatID = gameData.playerChatID;
        }
    }

    void SpawnSerifOrEmote(MachingRoomData.RoomData _roomData, RADIO_CHAT_ID _chatID)
    {
        if (_chatID == RADIO_CHAT_ID.NONE)
        {

        }
        else if (_chatID <= RADIO_CHAT_ID.HELP)
        {
            Managers.instance.gameManager.ingameCanvas.AddSerif(_roomData, _chatID);
        }
        else
        {
            Managers.instance.gameManager.ingameCanvas.AddSerif(_roomData, _chatID);
            ownerPlayer.GetComponent<Player>().PlayEmote(_chatID);
        }
    }
}

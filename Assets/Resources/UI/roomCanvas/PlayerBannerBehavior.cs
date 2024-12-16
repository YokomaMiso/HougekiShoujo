using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static MachingRoomData;

public class PlayerBannerBehavior : MonoBehaviour
{
    [SerializeField] Sprite buttonSpriteStart;
    [SerializeField] Sprite buttonSpriteReady;
    [SerializeField] Sprite buttonSpriteCancel;
    Image buttonImage = null;

    public int num = -1;

    int prevChara = 0;
    int prevTeamNum = 0;
    bool prevReady = false;

    public void SetNum(int _num)
    {
        num = _num;
        if (num == Managers.instance.playerID)
        {
            transform.GetChild(0).GetComponent<Image>().color = Color.white;
            transform.GetChild(3).GetComponent<Text>().color = Color.black;
            buttonImage = transform.GetChild(5).GetComponent<Image>();

            if (num == 0) { buttonImage.sprite = buttonSpriteStart; }
            else { buttonImage.sprite = buttonSpriteReady; }
        }
        else
        {
            transform.GetChild(5).gameObject.SetActive(false);
        }
    }
    public void BannerIconUpdate(RoomData _roomData)
    {
        if (_roomData.myID == MachingRoomData.bannerEmpty) { return; }

        int charaID = _roomData.selectedCharacterID;
        Sprite icon = Managers.instance.gameManager.playerDatas[charaID].GetCharacterAnimData().GetCharaIcon();
        transform.GetChild(2).GetComponent<Image>().sprite = icon;

        transform.GetChild(3).GetComponent<Text>().text = _roomData.playerName;
        transform.GetChild(4).gameObject.SetActive(_roomData.ready);

        if (buttonImage)
        {
            if (num == 0)
            {
                int readyCount = 0;
                for (int i = 1; i < MachingRoomData.playerMaxCount; i++)
                {
                    RoomData otherData = OSCManager.OSCinstance.GetRoomData(i);
                    if (otherData.ready) { readyCount++; }
                }

                if (readyCount >= _roomData.playerCount - 1)
                {
#if UNITY_EDITOR
                    buttonImage.color = Color.white;
#else
                    if (_roomData.teamACount == _roomData.teamBCount) { buttonImage.color = Color.white; }
                    else { buttonImage.color = Color.clear; }
#endif
                }
                else
                {
                    buttonImage.color = Color.clear;
                }
            }
            else
            {
                if (_roomData.ready) { buttonImage.sprite = buttonSpriteCancel; }
                else { buttonImage.sprite = buttonSpriteReady; }
            }
        }

        if (prevReady != _roomData.ready)
        {
            if (_roomData.ready) { Managers.instance.PlaySFXForUI(4); }
            else { Managers.instance.PlaySFXForUI(1); }
            prevReady = _roomData.ready;
        }
        else if (prevTeamNum != _roomData.myTeamNum)
        {
            Managers.instance.PlaySFXForUI(2);
            prevTeamNum = _roomData.myTeamNum;
        }
        else if (prevChara != _roomData.selectedCharacterID)
        {
            Managers.instance.PlaySFXForUI(2);
            prevChara = _roomData.selectedCharacterID;
        }
    }
}

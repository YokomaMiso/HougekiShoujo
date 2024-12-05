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
    public void SetNum(int _num)
    {
        num = _num;
        if (num == Managers.instance.playerID)
        {
            transform.GetChild(0).GetComponent<Image>().color = Color.white;
            transform.GetChild(3).GetComponent<Text>().color = Color.black;
            buttonImage = transform.GetChild(5).GetComponent<Image>();

            if (num == 0)
            {
                buttonImage.sprite = buttonSpriteStart;
            }
            else
            {
                buttonImage.sprite = buttonSpriteReady;
            }
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

        if (!buttonImage) { return; }

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
                if(readyCount <= 0){ buttonImage.color = Color.clear; }
                else{ buttonImage.color = Color.white; }
#endif
            }
            else
            {
                buttonImage.color = Color.clear;
            }
        }
        else
        {
            if (_roomData.ready)
            {
                buttonImage.sprite = buttonSpriteCancel;
            }
            else
            {
                buttonImage.sprite = buttonSpriteReady;
            }

        }
    }
}

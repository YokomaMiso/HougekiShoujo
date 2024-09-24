using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static MachingRoomData;

public class PlayerBannerBehavior : MonoBehaviour
{
    public int num = -1;
    public void SetNum(int _num)
    {
        num = _num;
    }
    public void BannerIconUpdate(RoomData _roomData)
    {
        if (_roomData.myID==MachingRoomData.bannerEmpty) { return; }

        int charaID = _roomData.selectedCharacterID;
        Sprite icon = Managers.instance.gameManager.playerDatas[charaID].GetCharacterAnimData().GetCharaIcon();
        transform.GetChild(2).GetComponent<Image>().sprite = icon;
        
        string text = "Player " + (_roomData.myID + 1).ToString();
        transform.GetChild(3).GetComponent<Text>().text = text;
        transform.GetChild(4).gameObject.SetActive(_roomData.ready);
    }
}

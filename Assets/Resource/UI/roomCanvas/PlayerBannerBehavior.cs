using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static MachingRoomData;

public class PlayerBannerBehavior : MonoBehaviour
{
    public int num = -1;
    public void SetNum(int _num)
    {
        num = _num;
        if (num == Managers.instance.playerID)
        {
            transform.GetChild(0).GetComponent<Image>().color = Color.white;
            transform.GetChild(3).GetComponent<Text>().color = Color.black;
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
    }
}

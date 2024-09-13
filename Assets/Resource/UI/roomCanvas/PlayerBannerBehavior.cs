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
        if (num < 0) { return; }

        int playerID = _roomData.GetBannerNum(num);
        
        int charaID = _roomData.GetSelectedCharacterID(playerID);
        Sprite icon = Managers.instance.gameManager.playerDatas[charaID].GetCharacterAnimData().GetCharaIcon();
        transform.GetChild(2).GetComponent<Image>().sprite = icon;
        
        string text = "Player " + (playerID + 1).ToString();
        transform.GetChild(3).GetComponent<Text>().text = text;
        transform.GetChild(4).gameObject.SetActive(_roomData.GetReadyPlayers(playerID));
    }
}

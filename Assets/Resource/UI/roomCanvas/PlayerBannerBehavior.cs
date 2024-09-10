using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBannerBehavior : MonoBehaviour
{
    public int num = -1;
    public void SetNum(int _num)
    {
        num = _num;
        string text = "Player " + (num + 1).ToString();
        transform.GetChild(2).GetComponent<Text>().text = text;
    }
    public void BannerIconUpdate()
    {
        if (num < 0) { return; } 

        int charaID = Managers.instance.roomManager.selectedCharacterID[num];
        Sprite icon = Managers.instance.gameManager.playerDatas[charaID].GetCharacterAnimData().GetCharaIcon();
        transform.GetChild(1).GetComponent<Image>().sprite = icon;
    }
}

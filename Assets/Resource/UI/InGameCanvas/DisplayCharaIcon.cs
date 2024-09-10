using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCharaIcon : MonoBehaviour
{
    readonly Vector3[] iconPos = new Vector3[6]
    {
        new Vector3(-150,70,0),
        new Vector3(150,70,0),

        new Vector3(-300,70,0),
        new Vector3(300,70,0),

        new Vector3(-450,70,0),
        new Vector3(450,70,0),
    };

    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            int nowPlayerID = Managers.instance.roomManager.bannerNum[i];

            if (nowPlayerID == -1)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            else
            {
                int charaID = Managers.instance.roomManager.selectedCharacterID[nowPlayerID];
                Sprite icon = Managers.instance.gameManager.playerDatas[charaID].GetCharacterAnimData().GetCharaIcon();
                transform.GetChild(i).GetComponent<Image>().sprite = icon;
                transform.GetChild(i).transform.localPosition = iconPos[i];
            }
        }
    }

}

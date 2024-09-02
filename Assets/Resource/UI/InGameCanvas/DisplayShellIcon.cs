using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DisplayShellIcon : MonoBehaviour
{
    Player ownerPlayer;
    [SerializeField] Sprite iconFrame;

    Image[] icons;
    Vector3[] iconPos;

    const int iconFrameNum = PlayerData.maxShellCount;

    void Start()
    {
        icons = new Image[PlayerData.maxShellCount + 1];
        iconPos = new Vector3[PlayerData.maxShellCount];

        //仮の処理　プレイヤーを探す
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            ownerPlayer = player.GetComponent<Player>();
            if (ownerPlayer.GetPlayerID() == 0)
            {
                for (int i = 0; i < PlayerData.maxShellCount; i++)
                {
                    icons[i] = transform.GetChild(i).GetComponent<Image>();
                    icons[i].sprite = ownerPlayer.GetPlayerData().GetShell(i).GetSprite();
                    iconPos[i] = icons[i].transform.position;
                }
                break;
            }
        }

        icons[iconFrameNum] = transform.GetChild(iconFrameNum).GetComponent<Image>();
        icons[iconFrameNum].sprite = null;
        icons[iconFrameNum].color = Color.clear;
    }

    void Update()
    {
        CheckPlayerShell();
    }

    void CheckPlayerShell()
    {
       int canonState = ownerPlayer.GetCanonState();

        switch (canonState)
        {
            case -1:
                icons[iconFrameNum].sprite = null;
                icons[iconFrameNum].color = Color.clear;
                break;
            default:
                icons[iconFrameNum].sprite = iconFrame;
                icons[iconFrameNum].color = Color.white;
                icons[iconFrameNum].transform.position = iconPos[canonState];
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DisplayShellIcon : MonoBehaviour
{
    Player ownerPlayer;
    Image shellIcon;
    Image iconFrame;

    void Start()
    {
        shellIcon = transform.GetChild(0).GetComponent<Image>();
        iconFrame = transform.GetChild(2).GetComponent<Image>();

        //仮の処理　プレイヤーを探す
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (!player.GetComponent<Player>()) { continue; }

            ownerPlayer = player.GetComponent<Player>();
            if (ownerPlayer.GetPlayerID() == 0)
            {
                shellIcon.sprite = ownerPlayer.GetPlayerData().GetShell().GetSprite();
                break;
            }
        }

        iconFrame.color = Color.clear;
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
                iconFrame.color = Color.clear;
                break;
            default:
                iconFrame.color = Color.white;
                break;
        }
    }
}

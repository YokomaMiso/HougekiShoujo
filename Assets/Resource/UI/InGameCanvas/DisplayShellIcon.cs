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

        ownerPlayer = Managers.instance.gameManager.GetPlayer(Managers.instance.playerID).GetComponent<Player>();
        shellIcon.sprite = ownerPlayer.GetPlayerData().GetShell().GetSprite();

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

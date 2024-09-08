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
    Image subIcon;
    Text subText;

    void Start()
    {
        shellIcon = transform.GetChild(0).GetComponent<Image>();
        subIcon = transform.GetChild(1).GetComponent<Image>();
        subText = subIcon.transform.GetChild(0).GetComponent<Text>();
        iconFrame = transform.GetChild(2).GetComponent<Image>();

        ownerPlayer = Managers.instance.gameManager.GetPlayer(Managers.instance.playerID).GetComponent<Player>();
        shellIcon.sprite = ownerPlayer.GetPlayerData().GetShell().GetShellIcon();
        subIcon.sprite = ownerPlayer.GetPlayerData().GetSubWeapon().GetIcon();

        iconFrame.color = Color.clear;
    }

    void Update()
    {
        CheckPlayerShell();
        CheckPlayerSubWeapon();
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

    void CheckPlayerSubWeapon()
    {
        float reloadTime = ownerPlayer.GetSubWeaponReload();

        if (reloadTime <= 0)
        {
            subIcon.color = Color.white;
            subText.color = Color.clear;
        }
        else
        {
            subIcon.color = Color.black;
            subText.color = Color.white;
            subText.text = reloadTime.ToString("f0");
        }




    }
}

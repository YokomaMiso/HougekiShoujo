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
    Image cancelIcon;

    Sprite shellIconSprite;
    Sprite subIconSprite;

    void Start()
    {
        ownerPlayer = Managers.instance.gameManager.GetPlayer(Managers.instance.playerID).GetComponent<Player>();

        shellIcon = transform.GetChild(0).GetComponent<Image>();
        subIcon = transform.GetChild(1).GetComponent<Image>();
        subText = subIcon.transform.GetChild(0).GetComponent<Text>();
        cancelIcon = subIcon.transform.GetChild(1).GetComponent<Image>();
        cancelIcon.gameObject.SetActive(false);
        iconFrame = transform.GetChild(2).GetComponent<Image>();

        shellIconSprite = ownerPlayer.GetPlayerData().GetShell().GetShellIcon();
        shellIcon.sprite = shellIconSprite;
        subIconSprite = ownerPlayer.GetPlayerData().GetSubWeapon().GetIcon();
        subIcon.sprite = subIconSprite;

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
        switch (ownerPlayer.playerState)
        {
            case PLAYER_STATE.AIMING:
                subText.color = Color.clear;
                subIcon.color = Color.gray;

                subIcon.sprite = shellIconSprite;
                cancelIcon.gameObject.SetActive(true);
                break;

            default:
                subIcon.sprite = subIconSprite;
                cancelIcon.gameObject.SetActive(false);
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
                break;
        }
    }
}

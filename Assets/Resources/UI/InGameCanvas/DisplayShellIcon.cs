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

    Image shellButton;
    Image subButton;

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

        shellButton = shellIcon.transform.GetChild(0).GetComponent<Image>();
        subButton = subIcon.transform.GetChild(2).GetComponent<Image>();
        if (Managers.instance.GetSmartPhoneFlag())
        {
            shellButton.color = Color.clear;
            subButton.color = Color.clear;
        }

        shellIconSprite = ownerPlayer.GetPlayerData().GetShell().GetShellIcon();
        shellIcon.sprite = shellIconSprite;
        subIconSprite = ownerPlayer.GetPlayerData().GetSubWeapon().GetIcon();
        subIcon.sprite = subIconSprite;

        iconFrame.color = Color.clear;

        ChangeDisplayButtons();
    }

    void Update()
    {
        if (!Managers.instance.unlockFlag[(int)UNLOCK_ITEM.UI_DELETE])
        {
            CheckPlayerShell();
            CheckPlayerSubWeapon();
        }
        if (InputManager.isChangedController) { ChangeDisplayButtons(); }
    }

    void ChangeDisplayButtons()
    {
        shellButton.sprite = InputManager.nowButtonSpriteData.GetSubmit();
        subButton.sprite = InputManager.nowButtonSpriteData.GetCancel();
    }

    void CheckPlayerShell()
    {
        if (ownerPlayer.GetPlayerData().GetShell().GetShellType() != SHELL_TYPE.SPECIAL)
        {
            int canonState = ownerPlayer.GetCanonState();

            switch (canonState)
            {
                case -1:
                    iconFrame.color = Color.clear;
                    shellIcon.color = Color.gray;
                    break;
                default:
                    iconFrame.color = Color.white;
                    shellIcon.color = Color.white;
                    break;
            }
        }
        else
        {
            iconFrame.color = Color.clear;
            shellIcon.color = Color.white;
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

    public void SendButtonToPlayer(int _num)
    {
        ownerPlayer.SetInputFromUI(_num);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTwoBehavior : TutorialBehavior
{
    [SerializeField] Sprite shell;
    [SerializeField] Sprite shellCancel;
    [SerializeField] Sprite stick;

    protected override void ChangeDisplayButtons()
    {
        if (Managers.instance.GetSmartPhoneFlag())
        {
            transform.GetChild(0).GetComponent<Image>().sprite = shell;
            transform.GetChild(1).GetComponent<Image>().sprite = shell;
            transform.GetChild(2).GetComponent<Image>().sprite = shell;
            transform.GetChild(3).GetComponent<Image>().sprite = stick;
            transform.GetChild(4).GetComponent<Image>().sprite = shellCancel;
        }
        else
        {
            Sprite[] applySprites = InputManager.nowButtonSpriteData.GetAllSprites();
            transform.GetChild(0).GetComponent<Image>().sprite = applySprites[2];
            transform.GetChild(1).GetComponent<Image>().sprite = applySprites[2];
            transform.GetChild(2).GetComponent<Image>().sprite = applySprites[2];
            transform.GetChild(3).GetComponent<Image>().sprite = applySprites[5];
            transform.GetChild(4).GetComponent<Image>().sprite = applySprites[3];
        }
    }
}

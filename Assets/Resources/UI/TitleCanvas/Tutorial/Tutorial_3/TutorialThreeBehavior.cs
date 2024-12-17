using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TutorialThreeBehavior : TutorialBehavior
{
    [SerializeField] Sprite sub;
    [SerializeField] Sprite stick;

    protected override void ChangeDisplayButtons()

    {
        if (Managers.instance.GetSmartPhoneFlag())
        {
            transform.GetChild(0).GetComponent<Image>().sprite = stick;
            transform.GetChild(1).GetComponent<Image>().sprite = sub;
        }
        else
        {
            Sprite[] applySprites = InputManager.nowButtonSpriteData.GetAllSprites();
            transform.GetChild(0).GetComponent<Image>().sprite = applySprites[5];
            transform.GetChild(1).GetComponent<Image>().sprite = applySprites[3];
        }
    }
}

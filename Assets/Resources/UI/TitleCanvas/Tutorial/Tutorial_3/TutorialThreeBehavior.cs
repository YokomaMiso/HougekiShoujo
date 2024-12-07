using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialThreeBehavior : TutorialBehavior
{
    protected override void ChangeDisplayButtons()

    {
        Sprite[] applySprites = InputManager.nowButtonSpriteData.GetAllSprites();
        transform.GetChild(0).GetComponent<Image>().sprite = applySprites[5];
        transform.GetChild(1).GetComponent<Image>().sprite = applySprites[3];
    }
}

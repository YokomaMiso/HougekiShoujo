using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialFourBehavior : TutorialBehavior
{
    protected override void ChangeDisplayButtons()
    {
        Sprite[] applySprites = InputManager.nowButtonSpriteData.GetAllSprites();

        Color color;
        if (InputManager.currentController == ControllerType.Keyboard)
        {
            color = Color.clear;
            transform.GetChild(0).GetComponent<Image>().color = color;
            transform.GetChild(1).GetComponent<Image>().color = color;
            transform.GetChild(2).GetComponent<Image>().color = color;
            transform.GetChild(3).GetComponent<Image>().color = color;

            color = Color.white;
            transform.GetChild(4).GetComponent<Image>().color = color;
            transform.GetChild(5).GetComponent<Image>().color = color;
        }
        else
        {
            transform.GetChild(0).GetComponent<Image>().sprite = applySprites[4];
            transform.GetChild(1).GetComponent<Image>().sprite = applySprites[6];
            transform.GetChild(2).GetComponent<Image>().sprite = applySprites[7];
            transform.GetChild(3).GetComponent<Image>().sprite = applySprites[6];

            color = Color.white;
            transform.GetChild(0).GetComponent<Image>().color = color;
            transform.GetChild(1).GetComponent<Image>().color = color;
            transform.GetChild(2).GetComponent<Image>().color = color;
            transform.GetChild(3).GetComponent<Image>().color = color;

            color = Color.clear;
            transform.GetChild(4).GetComponent<Image>().color = color;
            transform.GetChild(5).GetComponent<Image>().color = color;
        }
    }
}

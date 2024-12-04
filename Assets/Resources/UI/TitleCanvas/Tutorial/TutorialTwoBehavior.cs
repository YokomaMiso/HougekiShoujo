using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTwoBehavior : MonoBehaviour
{
    void OnEnable()
    {
        ChangeDisplayButtons();
    }

    void Update()
    {
        if (InputManager.isChangedController) { ChangeDisplayButtons(); }
    }

    void ChangeDisplayButtons()
    {
        Sprite[] applySprites = InputManager.nowButtonSpriteData.GetAllSprites();
        transform.GetChild(0).GetComponent<Image>().sprite = applySprites[2]; 
        transform.GetChild(1).GetComponent<Image>().sprite = applySprites[2]; 
        transform.GetChild(2).GetComponent<Image>().sprite = applySprites[2]; 
        transform.GetChild(3).GetComponent<Image>().sprite = applySprites[5]; 
        transform.GetChild(4).GetComponent<Image>().sprite = applySprites[3]; 
    }
}

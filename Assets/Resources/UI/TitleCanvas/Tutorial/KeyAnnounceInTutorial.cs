using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyAnnounceInTutorial : MonoBehaviour
{
    [SerializeField] TutorialWindow tw;
    readonly string[] announceText = new string[2] { "Next ", "OK   " };
    readonly string[] announceTextForSmartPhone = new string[2] { "Press to Next", "Press to OK" };

    void Start()
    {
        if (Managers.instance.GetSmartPhoneFlag())
        {
            transform.GetChild(0).GetComponent<Image>().color = Color.clear;
        }
    }

    void OnEnable()
    {
        ChangeDisplayButtons();
    }

    void Update()
    {
        if (InputManager.isChangedController) { ChangeDisplayButtons(); }
        ChangeText();
    }

    void ChangeDisplayButtons()
    {
        Sprite[] applySprites = InputManager.nowButtonSpriteData.GetAllSprites();
        transform.GetChild(0).GetComponent<Image>().sprite = applySprites[2];
    }
    void ChangeText()
    {
        int applyNum = 0;
        if (tw.TutorialEnd()) { applyNum = 1; }

        string text;
        if (Managers.instance.GetSmartPhoneFlag()) { text = announceTextForSmartPhone[applyNum]; }
        else { text = announceText[applyNum]; }

        transform.GetChild(1).GetComponent<Text>().text = text;
    }
}

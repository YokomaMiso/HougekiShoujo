using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyAnnounceInTutorial : MonoBehaviour
{
    [SerializeField] TutorialWindow tw;
    readonly string[] announceText = new string[2] { "Next", "OK" };

    void Start()
    {
        //if (Managers.instance.GetSmartPhoneFlag()) { Destroy(transform.parent.gameObject); }
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
        string text;
        if (tw.TutorialEnd()) { text = announceText[1]; }
        else { text = announceText[0]; }
        transform.GetChild(1).GetComponent<Text>().text = text;
    }
}

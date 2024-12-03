using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyAnnounceInTutorial : MonoBehaviour
{
     void Start()
    {
        if (Managers.instance.GetSmartPhoneFlag()) { Destroy(transform.parent.gameObject); }
    }

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
        GetComponent<Image>().sprite = applySprites[2];
    }
}

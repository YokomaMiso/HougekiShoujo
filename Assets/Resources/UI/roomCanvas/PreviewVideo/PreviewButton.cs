using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreviewButton : MonoBehaviour
{
    Image image;

    void Start()
    {
        image = GetComponent<Image>();
        image.sprite = InputManager.nowButtonSpriteData.GetTrigger();
    }

    void Update()
    {
        if (InputManager.isChangedController) { image.sprite = InputManager.nowButtonSpriteData.GetTrigger(); }
    }
}

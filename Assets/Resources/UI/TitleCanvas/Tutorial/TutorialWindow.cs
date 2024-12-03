using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialWindow : MonoBehaviour
{
    TitleCanvasBehavior parent;
    public void SetParent(TitleCanvasBehavior _parent) { parent = _parent; }

    [SerializeField] Image tutorialImage;
    [SerializeField] SelecterArrow[] arrows;
    [SerializeField] Sprite[] tutorialSprites;

    int spriteNum = 0;

    void Start()
    {

    }

    void Update()
    {
        Vector2 inputVector = InputManager.GetAxisDelay<Vector2>(Vec2AxisActions.LStickAxis, 0.2f);
        if (Mathf.Abs(inputVector.x) > 0.9f)
        {
            if (inputVector.x > 0) { ChangeSpriteNum(1); }
            else { ChangeSpriteNum(-1); }
        }

        if (InputManager.GetKeyDown(BoolActions.SouthButton)) { Submit(); }
    }

    public void Submit()
    {
        parent.ChangeTitleState(TITLE_STATE.INPUT_NAME);
    }

    public void ChangeSpriteNum(int _num)
    {
        int maxNum = tutorialSprites.Length;

        if (_num > 0) { spriteNum = (spriteNum + 1) % maxNum; }
        else { spriteNum = (spriteNum + maxNum - 1) % maxNum; }

        tutorialImage.sprite = tutorialSprites[spriteNum];
    }
}

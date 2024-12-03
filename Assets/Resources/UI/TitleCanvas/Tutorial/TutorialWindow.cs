using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TutorialWindow : MonoBehaviour
{
    TitleCanvasBehavior parent;
    public void SetParent(TitleCanvasBehavior _parent) { parent = _parent; }

    [SerializeField] Image[] tutorialImage;
    [SerializeField] SelecterArrow[] arrows;

    int spriteNum = 0;
    void Start()
    {
        int maxNum = tutorialImage.Length;

        for (int i = 0; i < maxNum; i++)
        {
            bool active = (i == spriteNum);
            tutorialImage[i].gameObject.SetActive(active);
        }

        ArrowUpdate();
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
        int maxNum = tutorialImage.Length;

        if (_num > 0)
        {
            if (spriteNum == maxNum - 1) { return; }
            spriteNum = (spriteNum + 1) % maxNum;
        }
        else
        {
            if (spriteNum == 0) { return; }
            spriteNum = (spriteNum + maxNum - 1) % maxNum;
        }
        arrows[Mathf.RoundToInt(Mathf.Clamp01(_num))].SetAdd();

        for (int i = 0; i < maxNum; i++)
        {
            bool active = (i == spriteNum);
            tutorialImage[i].gameObject.SetActive(active);
        }

        ArrowUpdate();
    }

    void ArrowUpdate()
    {
        int maxNum = tutorialImage.Length;

        if (spriteNum == 0)
        {
            arrows[0].Init();
            arrows[0].gameObject.SetActive(false);
            arrows[maxNum - 1].gameObject.SetActive(true);
        }
        else if (spriteNum == maxNum - 1)
        {
            arrows[maxNum - 1].Init();
            arrows[maxNum - 1].gameObject.SetActive(false);
            arrows[0].gameObject.SetActive(true);
        }
        else
        {
            for (int i = 0; i < arrows.Length; i++) { arrows[i].gameObject.SetActive(true); }
        }
    }
}

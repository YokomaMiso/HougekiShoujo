﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class OffsetChildObjects : MonoBehaviour
{
    private bool LeftStickcanChange = true;
    private bool RightStickcanChange = true;

    public CharGalleryManager charGalleryManager;
    public MainViewPort mainViewPort;
    public Transform contentParent;
    public GameObject characterButtonPrefab;
    public CharGalleryController charGalleryController;

    public Vector3 firstIconPosition = new Vector3(-25, 130, 0);
    public Vector3 iconScale = new Vector3(0.2f, 0.2f, 0.2f);
    public Vector2 iconSize = new Vector2(200, 200);

    int inputIndex = -1;

    private bool EnterDown = false;
    void Start()
    {
        inputIndex = -1;
        GalleryManager.Instance.SetState(GalleryState.CharacterSelect);

        Vector3 newPosition = transform.localPosition;
        newPosition.y = 0;
        transform.localPosition = newPosition;

        GenerateCharacterUI();

        ArrangeIcons();
    }


    void ArrangeIcons()
    {
        if (contentParent.childCount == 0) return;

        RectTransform firstChild = contentParent.GetChild(0).GetComponent<RectTransform>();
        Vector3 basePosition = firstIconPosition;
        float baseHeight = firstChild.rect.height * firstChild.localScale.y;
        float baseWidth = firstChild.rect.width * firstChild.localScale.x;

        for (int i = 0; i < contentParent.childCount; i++)
        {
            RectTransform currentChild = contentParent.GetChild(i).GetComponent<RectTransform>();
            float newY = basePosition.y - i * (baseHeight / 2);
            float newX = (i % 2 == 0) ? basePosition.x : basePosition.x + baseWidth;
            currentChild.localPosition = new Vector3(newX, newY, basePosition.z);
        }
    }

    void GenerateCharacterUI()
    {
        foreach (CharacterData character in charGalleryManager.characterList)
        {
            GameObject newButton = Instantiate(characterButtonPrefab, contentParent);

            RectTransform buttonRectTransform = newButton.GetComponent<RectTransform>();
            if (buttonRectTransform != null)
            {
                buttonRectTransform.localPosition = firstIconPosition;
                buttonRectTransform.localScale = iconScale;
                buttonRectTransform.sizeDelta = iconSize;
            }

            Image buttonImage = newButton.GetComponentInChildren<Image>();
            if (buttonImage != null)
            {
                buttonImage.sprite = character.charData.characterIcon;
                if (character.iconMaterial != null)
                {
                    buttonImage.material = new Material(character.iconMaterial);
                    buttonImage.material.SetFloat("_OutlinePixelWidth", 0.0f);
                }
            }

            Text buttonText = newButton.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.text = character.charData.characterName;
            }

            Button button = newButton.GetComponent<Button>();
        }
    }


    void Update()
    {
        HandleInput();
        HighlightCurrentIcon();
        HandleEnterKey();

    }

    void HandleInput()
    {
        Vector2 Inout = InputManager.GetAxis<Vector2>(Vec2AxisActions.LStickAxis);
        Vector2 RightInout = InputManager.GetAxis<Vector2>(Vec2AxisActions.RStickAxis);

        if (GalleryManager.Instance.CurrentState == GalleryState.CharacterSelect)
        {
            if (Inout.x > 0.5f && LeftStickcanChange)
            {
                if (inputIndex < contentParent.childCount - 1)
                {
                    inputIndex++;
                }
                LeftStickcanChange = false;
            }
            else if (Inout.x < -0.5f && LeftStickcanChange)
            {
                if (inputIndex > 0)
                {
                    inputIndex--;
                }
                LeftStickcanChange = false;
            }
            else if (Inout.x > -0.5f && Inout.x < 0.5f)
            {
                LeftStickcanChange = true;
            }
        }

    }

    void HighlightCurrentIcon()
    {
        for (int i = 0; i < contentParent.childCount; i++)
        {
            Transform child = contentParent.GetChild(i);
            Image buttonImage = child.GetComponentInChildren<Image>();

            if (buttonImage != null && buttonImage.material != null)
            {
                buttonImage.material.SetFloat("_OutlinePixelWidth", i == inputIndex ? 10.0f : 0.0f);
            }
        }
    }

    void HandleEnterKey()
    {
        if (InputManager.GetKeyUp(BoolActions.SouthButton))
        {
            EnterDown = false;
        }

        if (GalleryManager.Instance.CurrentState == GalleryState.CharacterSelect)
        {
            if (InputManager.GetKeyDown(BoolActions.SouthButton) && inputIndex >= 0 && !EnterDown)
            {
                EnterDown = true;
                mainViewPort.EnterCharacterGallery(charGalleryManager.characterList[inputIndex]);
                charGalleryController.SetCharData(charGalleryManager.characterList[inputIndex]);
                mainViewPort.GenerateIll(charGalleryManager.characterList[inputIndex]);
                GalleryManager.Instance.SetState(GalleryState.CharacterGallery);
            }
        }
    }

}
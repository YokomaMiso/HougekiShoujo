using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class OffsetChildObjects : MonoBehaviour
{
    private bool canChangeIndex = true;

    public CharGalleryManager charGalleryManager;
    public MainViewPort mainViewPort;
    public Transform contentParent;
    public GameObject characterButtonPrefab;
    public Vector3 firstIconPosition = new Vector3(-25, 130, 0);
    public Vector3 iconScale = new Vector3(0.2f, 0.2f, 0.2f);
    public Vector2 iconSize = new Vector2(200, 200);

    int inputIndex = -1;
    private int currentAnimatorIndex = 0;

    void Start()
    {
        inputIndex = -1;
        GalleryManager.Instance.SetState(GalleryState.CharacterSelect);

        Vector3 newPosition = transform.localPosition;
        newPosition.y = 0;
        transform.localPosition = newPosition;

        GenerateCharacterUI();

        ArrangeIcons();

        HandleBackKey();
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
            float newX = (i % 2 == 0) ? basePosition.x : basePosition.x - baseWidth;
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
                buttonImage.sprite = character.characterIcon;
                if (character.iconMaterial != null)
                {
                    buttonImage.material = new Material(character.iconMaterial);
                    buttonImage.material.SetFloat("_OutlinePixelWidth", 0.0f);
                }
            }

            Text buttonText = newButton.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.text = character.characterName;
            }

            Button button = newButton.GetComponent<Button>();
        }
    }


    void Update()
    {
        HandleInput();
        HighlightCurrentIcon();
        HandleEnterKey();
        HandleBackKey();
    }

    void HandleInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        if(GalleryManager.Instance.CurrentState== GalleryState.CharacterSelect)
        {
            if (horizontalInput > 0.5f && canChangeIndex)
            {
                if (inputIndex < contentParent.childCount - 1)
                {
                    inputIndex++;
                }
                canChangeIndex = false;
            }
            else if (horizontalInput < -0.5f && canChangeIndex)
            {
                if (inputIndex > 0)
                {
                    inputIndex--;
                }
                canChangeIndex = false;
            }
            else if (horizontalInput > -0.5f && horizontalInput < 0.5f)
            {
                canChangeIndex = true;
            }
        }

        if(GalleryManager.Instance.CurrentState== GalleryState.CharacterGallery)
        {
            if (horizontalInput > 0.5f && canChangeIndex)
            {
                CharacterData currentCharacter = charGalleryManager.characterList[inputIndex];

                currentAnimatorIndex = (currentAnimatorIndex + 1) % currentCharacter.animatorControllers.Count;

                mainViewPort.UpdateAnimatorController(currentCharacter,currentAnimatorIndex);

                canChangeIndex = false;
            }
            else if (horizontalInput < -0.5f && canChangeIndex)
            {
                CharacterData currentCharacter = charGalleryManager.characterList[inputIndex];

                currentAnimatorIndex = (currentAnimatorIndex - 1 + currentCharacter.animatorControllers.Count)
                                       % currentCharacter.animatorControllers.Count;

                mainViewPort.UpdateAnimatorController(currentCharacter,currentAnimatorIndex);

                canChangeIndex = false;
            }
            else if (horizontalInput > -0.5f && horizontalInput < 0.5f)
            {
                canChangeIndex = true;
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
        if(GalleryManager.Instance.CurrentState== GalleryState.CharacterSelect)
        {
            if (Input.GetButtonDown("Submit") && inputIndex >= 0)
            {

                CharacterData selectedCharacter = charGalleryManager.characterList[inputIndex];

                GalleryManager.Instance.SetState(GalleryState.CharacterGallery);

                mainViewPort.GenerateCharacterILL(selectedCharacter);
                mainViewPort.GenerateCharacterAnime(selectedCharacter);
            }
        }
        
    }

    void HandleBackKey()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (GalleryManager.Instance.CurrentState == GalleryState.CharacterGallery)
            {
                GalleryManager.Instance.SetState(GalleryState.CharacterSelect);


                mainViewPort.ClearCharacterILL();
            }
        }
    }

}
using System.Collections;
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

    private int inputIndex = 0;

    public void SetInputIndex(int index)
    {
        inputIndex = index;
    }

    public int GetInputIndex() {  return inputIndex; }

    private bool isMove = false;
    private bool EnterDown = false;
    void Start()
    {
        inputIndex = 0;
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
            float newY = basePosition.y - (i * baseHeight*1.2f);
            currentChild.localPosition = new Vector3(basePosition.x, newY, basePosition.z);
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
                buttonImage.material = character.iconMaterial;
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
        HandleEnterKey();

    }

    void HandleInput()
    {
        Vector2 Inout = InputManager.GetAxis<Vector2>(Vec2AxisActions.LStickAxis);
        Vector2 RightInout = InputManager.GetAxis<Vector2>(Vec2AxisActions.RStickAxis);

        if (GalleryManager.Instance.CurrentState == GalleryState.CharacterSelect)
        {
            if (Inout.y > 0.5f && LeftStickcanChange)
            {
                MoveScrollUp();
                LeftStickcanChange = false;
            }
            else if (Inout.y < -0.5f && LeftStickcanChange)
            {
                MoveScrollDown();
                LeftStickcanChange = false;
            }
            else if (Inout.y > -0.5f && Inout.y < 0.5f)
            {
                LeftStickcanChange = true;
            }
        }

    }

    public void MoveScrollUp()
    {
        if (inputIndex > 0 && !isMove)
        {
            inputIndex--;
            MoveScrollView(-1);
        }
    }

    public void MoveScrollDown()
    {
        if (inputIndex < contentParent.childCount - 1 && !isMove)
        {
            inputIndex++;
            MoveScrollView(1);
        }
    }


    IEnumerator SmoothScrollTo(Vector2 targetPosition)
    {
        isMove = true;
        RectTransform contentRect = contentParent.GetComponent<RectTransform>();
        Vector2 startPosition = contentRect.anchoredPosition;
        float duration = 0.1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            contentRect.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, elapsed / duration);
            yield return null;
        }

        contentRect.anchoredPosition = targetPosition;
        isMove = false;
    }


    void MoveScrollView(int direction)
    {
        RectTransform contentRect = contentParent.GetComponent<RectTransform>();

        float moveDistance = 188;

        float newY = contentRect.anchoredPosition.y + (direction * moveDistance);
        float minY = 0;
        float maxY = Mathf.Max(0, (contentParent.childCount - 1) * moveDistance);
        newY = Mathf.Clamp(newY, minY, maxY);

        StartCoroutine(SmoothScrollTo(new Vector2(contentRect.anchoredPosition.x, newY)));
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
                charGalleryController.ReSetSpriteState();
                GalleryManager.Instance.SetState(GalleryState.CharacterGallery);
            }
        }
    }

}
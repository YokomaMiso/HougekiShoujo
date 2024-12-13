using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MainViewPortWorld : MonoBehaviour
{

    public AllWorldGalleryData allWorldGalleryData;
    public WorldIntroData worldIntroData;

    public Vector3 BulidingPosition = new Vector3(-450, 0, 0);
    public Vector3 BulidingScale = new Vector3(1, 1, 1);

    public Vector3 WorldIntroPosition = new Vector3(0, 0, 0);


    public Vector3 TextPosition = new Vector3(800, 0, 0);

    public Transform contentParent;
    public GameObject BulidingObjectPrefab;
    public GameObject WolrdIntroObjectPrefab;

    private GameObject CurrentBulidingObject;
    private GameObject CurrentIntroObject;

    private bool LeftStickcanChange = true;
    private int currentBulidingSprite = 0;
    private int currentWorldIntro = 0;

    private int MaxBuliding = 0;

    void Start()
    {
        foreach (WorldGalleryData worldData in allWorldGalleryData.worldGalleryDatas)
        {
            if (worldData != null && worldData.bulidTextPair != null)
            {
                MaxBuliding += worldData.bulidTextPair.Count;
            }
        }
    }

    void Update()
    {
        WorldGalleryInput();
    }

    void GenerateBulidingObject()
    {
        GameObject newBulidingObject = Instantiate(BulidingObjectPrefab, contentParent);
        newBulidingObject.tag = "GalleryObject";

        RectTransform bulidTransform = newBulidingObject.GetComponent<RectTransform>();
        if (bulidTransform != null)
        {
            bulidTransform.localPosition = BulidingPosition;
            CurrentBulidingObject = newBulidingObject;
        }
    }

    void GenerateIntroObject()
    {
        GameObject newWorldIntroObject = Instantiate(WolrdIntroObjectPrefab, contentParent);
        newWorldIntroObject.tag = "GalleryObject";

        RectTransform bulidTransform = newWorldIntroObject.GetComponent<RectTransform>();
        if (bulidTransform != null)
        {
            bulidTransform.localPosition = WorldIntroPosition;
            CurrentIntroObject = newWorldIntroObject;
        }
    }

    void WorldGalleryInput()
    {
        Vector2 Inout = InputManager.GetAxis<Vector2>(Vec2AxisActions.LStickAxis);

        if (InputManager.GetKeyDown(BoolActions.SouthButton))
        {
            if (GalleryManager.Instance.GetInputIndex() == 0 && GalleryManager.Instance.CurrentState == GalleryState.WorldSelect)
            {
                GalleryManager.Instance.SetState(GalleryState.WorldIllGallery);
                GenerateBulidingObject();
                SetBulidingSprite(currentBulidingSprite);
            }
            if (GalleryManager.Instance.GetInputIndex() == 1 && GalleryManager.Instance.CurrentState == GalleryState.WorldSelect)
            {
                GalleryManager.Instance.SetState(GalleryState.WorldTextGallery);
                GenerateIntroObject();
                SetWorldIntro(currentWorldIntro);
            }
        }

        if (InputManager.GetKeyDown(BoolActions.EastButton))
        {
            if (GalleryManager.Instance.CurrentState == GalleryState.WorldIllGallery ||
               GalleryManager.Instance.CurrentState == GalleryState.WorldTextGallery)
            {
                currentBulidingSprite = 0;
                GalleryManager.Instance.SetState(GalleryState.WorldSelect);
                ClearWorldGallery();
            }
        }

        if (GalleryManager.Instance.CurrentState == GalleryState.WorldIllGallery)
        {
            if (Inout.x > 0.5f && LeftStickcanChange)
            {
                currentBulidingSprite++;
                if (currentBulidingSprite >= MaxBuliding) { currentBulidingSprite = 0; }
                SetBulidingSprite(currentBulidingSprite);
                LeftStickcanChange = false;
            }
            else if (Inout.x < -0.5f && LeftStickcanChange)
            {
                currentBulidingSprite--;
                if (currentBulidingSprite < 0) { currentBulidingSprite = MaxBuliding - 1; }
                SetBulidingSprite(currentBulidingSprite);
                LeftStickcanChange = false;
            }
            else if (Inout.x > -0.5f && Inout.x < 0.5f)
            {
                LeftStickcanChange = true;
            }
        }

        if (GalleryManager.Instance.CurrentState == GalleryState.WorldTextGallery)
        {
            if (Inout.x > 0.5f && LeftStickcanChange)
            {
                currentWorldIntro++;
                if (currentWorldIntro >= worldIntroData.worldIntro.Count) { currentWorldIntro = 0; }
                SetWorldIntro(currentWorldIntro);
                Scrollbar scrollbar = CurrentIntroObject.GetComponentInChildren<Scrollbar>();
                if (scrollbar != null)
                {
                    scrollbar.value = 1.0f;
                }
                LeftStickcanChange = false;
            }
            else if (Inout.x < -0.5f && LeftStickcanChange)
            {
                currentWorldIntro--;
                if (currentWorldIntro < 0) { currentWorldIntro = worldIntroData.worldIntro.Count; }
                SetWorldIntro(currentWorldIntro);
                Scrollbar scrollbar = CurrentIntroObject.GetComponentInChildren<Scrollbar>();
                if (scrollbar != null)
                {
                    scrollbar.value = 1.0f;
                }
                LeftStickcanChange = false;
            }
            else if (Inout.x > -0.5f && Inout.x < 0.5f)
            {
                LeftStickcanChange = true;
            }
            if (Inout.y > 0.5f)
            {
                Scrollbar scrollbar = CurrentIntroObject.GetComponentInChildren<Scrollbar>();
                if (scrollbar != null)
                {
                    scrollbar.value = Mathf.Clamp(scrollbar.value - 0.01f, 0f, 1f);
                }
            }
            if (Inout.y < -0.5f)
            {
                Scrollbar scrollbar = CurrentIntroObject.GetComponentInChildren<Scrollbar>();
                if (scrollbar != null)
                {
                    scrollbar.value = Mathf.Clamp(scrollbar.value + 0.01f, 0f, 1f);
                }
            }
        }
    }

    Sprite GetSpriteByIndex(int index)
    {
        int currentIndex = 0;
        foreach (WorldGalleryData worldData in allWorldGalleryData.worldGalleryDatas)
        {
            if (index < currentIndex + worldData.bulidTextPair.Count)
            {
                return worldData.bulidTextPair[index - currentIndex].sprite;
            }
            currentIndex += worldData.bulidTextPair.Count;
        }
        return null;
    }

    string GetTextByIndex(int index)
    {
        int currentIndex = 0;
        foreach (WorldGalleryData worldData in allWorldGalleryData.worldGalleryDatas)
        {
            if (index < currentIndex + worldData.bulidTextPair.Count)
            {
                return worldData.bulidTextPair[index - currentIndex].bulidingText;
            }
            currentIndex += worldData.bulidTextPair.Count;
        }
        return null;
    }

    void SetBulidingSprite(int index)
    {
        if (CurrentBulidingObject != null)
        {
            Image bulidingImage = CurrentBulidingObject.GetComponent<Image>();
            if (bulidingImage != null)
            {
                Sprite newSprite = GetSpriteByIndex(index);
                bulidingImage.sprite = newSprite;
                if (newSprite != null)
                {
                    RectTransform rectTransform = CurrentBulidingObject.GetComponent<RectTransform>();
                    if (rectTransform != null)
                    {
                        rectTransform.sizeDelta = new Vector2(newSprite.rect.width, newSprite.rect.height);
                    }
                }
            }

            Transform textTransform = CurrentBulidingObject.transform.Find("Text");
            if (textTransform != null)
            {
                Text bulidingText = textTransform.GetComponent<Text>();
                if (bulidingText != null)
                {
                    string newText = GetTextByIndex(index);
                    bulidingText.text = newText;
                    if (!string.IsNullOrEmpty(newText))
                    {
                        textTransform.localPosition = TextPosition;
                    }
                }
            }
        }
    }

    void SetWorldIntro(int index)
    {
        if (CurrentIntroObject != null)
        {
            Transform textTransform = CurrentIntroObject.transform.Find("Viewport/Content/Text");
            if (textTransform != null)
            {
                Text worldIntroText = textTransform.GetComponent<Text>();
                if (worldIntroText != null)
                {
                    string newText = worldIntroData.worldIntro[index].WorldIntroText;
                    worldIntroText.text = newText;
                }
            }

            Transform nameTransform = CurrentIntroObject.transform.Find("NameText");
            if (nameTransform != null)
            {
                Text nameText = nameTransform.GetComponent<Text>();
                if (nameText != null)
                {
                    string newText = worldIntroData.worldIntro[index].Name;
                    nameText.text = newText;
                }
            }
        }
    }

    void ClearWorldGallery()
    {
        foreach (Transform child in contentParent)
        {
            if (child.CompareTag("GalleryObject"))
            {
                Destroy(child.gameObject);
            }
        }
    }
}

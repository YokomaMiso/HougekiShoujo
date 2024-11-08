using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MainViewPortWorld : MonoBehaviour
{

    public AllWorldGalleryData allWorldGalleryData;

    public Vector3 BulidingPosition = new Vector3(0,0,0);
    public Vector3 BulidingScale = new Vector3(1, 1, 1);

    public Transform contentParent;
    public GameObject BulidingObjectPrefab;

    private GameObject CurrentBulidingObject;

    private bool LeftStickcanChange = true;
    private int currentBulidingSprite = 0;

    private int MaxBuliding=0;

    void Start()
    {
        foreach(WorldGalleryData worldData in allWorldGalleryData.worldGalleryDatas)
        {
            if (worldData != null && worldData.sprites != null)
            {
                MaxBuliding += worldData.sprites.Count;
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
            bulidTransform.localScale = BulidingScale;
            CurrentBulidingObject = newBulidingObject;
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
            }
        }

        if (InputManager.GetKeyDown(BoolActions.EastButton))
        {
            if(GalleryManager.Instance.CurrentState== GalleryState.WorldIllGallery ||
               GalleryManager.Instance.CurrentState == GalleryState.WorldTextGallery)
            {
                currentBulidingSprite = 0;
                GalleryManager.Instance.SetState(GalleryState.WorldSelect);
                ClearWorldGallery();
            }
        }

        if(GalleryManager.Instance.CurrentState == GalleryState.WorldIllGallery)
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
    }

    Sprite GetSpriteByIndex(int index)
    {
        int currentIndex = 0;
        foreach(WorldGalleryData worldData in allWorldGalleryData.worldGalleryDatas)
        {
            if (index < currentIndex + worldData.sprites.Count)
            {
                return worldData.sprites[index - currentIndex];
            }
            currentIndex += worldData.sprites.Count;
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

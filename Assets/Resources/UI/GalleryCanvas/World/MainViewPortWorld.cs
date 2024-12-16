using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MainViewPortWorld : MonoBehaviour
{

    public AllWorldGalleryData allWorldGalleryData;
    public WorldIntroData worldIntroData;
    private WorldScrollGenerate buildSpawner;
    public WorldScrollGenerate GetViewPortWorld()=>buildSpawner;
    private ScrollViewController scrollViewController;

    public Vector3 BulidingEndPosition = new Vector3(-450, 0, 0);
    public Vector3 BulidingStartPosition;

    public Vector3 IntroEndPosition = new Vector3(1064, -840, 0);
    public Vector3 IntroStartPosition;
    public Vector3 BulidingScale = new Vector3(1, 1, 1);

    public Vector3 WorldIntroPosition = new Vector3(0, 0, 0);
    public Transform contentParent;

    public GameObject BulidingObjectPrefab;
    public GameObject WolrdIntroObjectPrefab;

    private GameObject CurrentBulidingObject;
    private GameObject CurrentIntroObject;

    

    private int MaxBuliding = 0;
    private bool isMove = false;
    public bool GetMoveState() => isMove;

    private Vector3 currentPageLastPosition;

    void Start()
    {
    }

    void Update()
    {
    }

    public void GenerateBulidingObject()
    {
        GameObject newBulidingObject = Instantiate(BulidingObjectPrefab, contentParent);

        RectTransform bulidTransform = newBulidingObject.GetComponent<RectTransform>();
        if (CurrentBulidingObject == null)
        {
            BulidingStartPosition= bulidTransform.localPosition; 
            CurrentBulidingObject = newBulidingObject;
        }
        buildSpawner = newBulidingObject.transform.Find("charScroll")?.GetComponent<WorldScrollGenerate>();
    }

    public void GenerateIntroObject()
    {
        GameObject newWorldIntroObject = Instantiate(WolrdIntroObjectPrefab, contentParent);

        RectTransform bulidTransform = newWorldIntroObject.GetComponent<RectTransform>();
        if (CurrentIntroObject == null)
        {
            IntroStartPosition=bulidTransform.localPosition; 
            CurrentIntroObject = newWorldIntroObject;
        }
    }


    public IEnumerator MoveObject(Transform targetObject, Vector3 targetPosition, float duration,bool isExit,bool hasScroll)
    {
        if (hasScroll) { CurrentBulidingObject.transform.Find("charScroll").gameObject.SetActive(false); }
        
        float elapsedTime = 0f;
        Vector3 startingPosition = targetObject.localPosition;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            t = EaseInOutQuad(t);
            targetObject.localPosition = Vector3.Lerp(startingPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        targetObject.localPosition = targetPosition;
        if (isExit)
        {
            Destroy(CurrentBulidingObject);
            CurrentBulidingObject = null;
            Destroy(CurrentIntroObject);
            CurrentIntroObject = null;
        }
        else
        {
            if (hasScroll) { CurrentBulidingObject.transform.Find("charScroll").gameObject.SetActive(true); }
        }
    }

    public IEnumerator HandleBuildingExitSequentially()
    {
        isMove = true;
        Transform pageTransform = transform.Find("page1-2");
        if(pageTransform != null)
        {
            Vector3 newPagePosition = new Vector3(pageTransform.localPosition.x-1500.0f,pageTransform.localPosition.y,0.0f);
            currentPageLastPosition = pageTransform.localPosition;
            yield return StartCoroutine(MoveObject(pageTransform, newPagePosition, 0.5f, false, false));

            
            yield return StartCoroutine(MoveObject(CurrentBulidingObject.transform, BulidingEndPosition, 0.5f, false, true));
        }
        isMove = false;
    }

    public IEnumerator HandleBuildingResetSequentially()
    {
        isMove = true;
        Transform pageTransform = transform.Find("page1-2");
        if (pageTransform != null)
        {
            yield return StartCoroutine(MoveObject(CurrentBulidingObject.transform, BulidingStartPosition, 0.5f, true, true));


            yield return StartCoroutine(MoveObject(pageTransform, currentPageLastPosition, 0.5f, false, false));
        }
        GalleryManager.Instance.SetState(GalleryState.WorldSelect);
        isMove = false;
    }

    public IEnumerator HandleIntroExitSequentially()
    {
        isMove = true;
        Transform pageTransform = transform.Find("page1-2");
        if (pageTransform != null)
        {
            Vector3 newPagePosition = new Vector3(pageTransform.localPosition.x - 1500.0f, pageTransform.localPosition.y, 0.0f);
            currentPageLastPosition = pageTransform.localPosition;
            yield return StartCoroutine(MoveObject(pageTransform, newPagePosition, 0.5f, false, false));


            yield return StartCoroutine(MoveObject(CurrentIntroObject.transform, IntroEndPosition, 0.5f, false, false));
        }
        isMove = false;
    }

    public IEnumerator HandleIntroResetSequentially()
    {
        isMove = true;
        Transform pageTransform = transform.Find("page1-2");
        if (pageTransform != null)
        {
            yield return StartCoroutine(MoveObject(CurrentIntroObject.transform, IntroStartPosition, 0.5f, true, false));


            yield return StartCoroutine(MoveObject(pageTransform, currentPageLastPosition, 0.5f, false, false));
        }
        GalleryManager.Instance.SetState(GalleryState.WorldSelect);
        isMove = false;
    }

    private float EaseInOutQuad(float t)
    {
        if (t < 0.5f)
            return 2 * t * t;
        return 1 - Mathf.Pow(-2 * t + 2, 2) / 2;
    }

    private float EaseInQuad(float t) => t * t;

    private float EaseOutQuad(float t) => 1 - (1 - t) * (1 - t);

    public void LoadSpriteAndText(int stageIndex,int buildIndex)
    {
        Text nameText = CurrentBulidingObject.transform.Find("Name")?.GetComponent<Text>();
        if(nameText != null)
        {
            nameText.text = allWorldGalleryData.worldGalleryDatas[stageIndex].bulidTextPair[buildIndex].name;
        }

        Text introText = CurrentBulidingObject.transform.Find("Intro")?.GetComponent<Text>();
        if (introText != null)
        {
            introText.text = allWorldGalleryData.worldGalleryDatas[stageIndex].bulidTextPair[buildIndex].bulidingText;
        }

        Image buildSprite = CurrentBulidingObject.transform.Find("Photo/Mask/Sprite")?.GetComponent<Image>();
        if (buildSprite != null)
        {
            Sprite newSprite = allWorldGalleryData.worldGalleryDatas[stageIndex].bulidTextPair[buildIndex].sprite;
            buildSprite.sprite = newSprite;
            RectTransform maskTransform = CurrentBulidingObject.transform.Find("Photo/Mask")?.GetComponent<RectTransform>();
            if (maskTransform != null && newSprite != null)
            {
                Vector2 maskSize = maskTransform.rect.size;
                float spriteWidth = newSprite.rect.width;
                float spriteHeight = newSprite.rect.height;

                float maskAspect = maskSize.x / maskSize.y;
                float spriteAspect = spriteWidth / spriteHeight;

                Vector2 newSize;
                if (spriteAspect > maskAspect)
                {
                    newSize = new Vector2(maskSize.x, maskSize.x / spriteAspect);
                }
                else
                {
                    newSize = new Vector2(maskSize.y * spriteAspect, maskSize.y);
                }

                RectTransform spriteTransform = buildSprite.GetComponent<RectTransform>();
                if (spriteTransform != null)
                {
                    spriteTransform.sizeDelta = newSize;
                    spriteTransform.sizeDelta *= 0.75f;
                }
            }
        }
    }
    public void LoadIntroText(int index)
    {
        Text nameText = CurrentIntroObject.transform.Find("ScrollView/NameText")?.GetComponent<Text>();
        if (nameText != null)
        {
            nameText.text = worldIntroData.worldIntro[index].Name;
        }

        Text introText = CurrentIntroObject.transform.Find("ScrollView/Viewport/Content/Text")?.GetComponent<Text>();
        if (introText != null)
        {
            introText.text = worldIntroData.worldIntro[index].WorldIntroText;
            float preferredHeight = introText.preferredHeight;

            RectTransform rectTransform = introText.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, preferredHeight);

            scrollViewController= CurrentIntroObject.transform.Find("ScrollView/Viewport/Content/Text")?.GetComponent<ScrollViewController>();
            scrollViewController.SetRect(rectTransform);
        }
    }
}

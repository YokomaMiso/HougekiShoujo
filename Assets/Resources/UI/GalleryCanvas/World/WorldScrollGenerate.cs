using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WorldScrollGenerate : MonoBehaviour
{
    public AllWorldGalleryData allWorldDatas;
    private int currentMaxBuildingNum=0;
    public int GetMaxBuild() => currentMaxBuildingNum;
    public GameObject containerParent;
    public GameObject objectPrefab;
    public Vector3 startPosition = new Vector3(-1000.0f, -45.0f, 0.0f);
    public float ySpacing = 45.0f;
    private bool isMove = false;
    private List<BulidingTextPair> buildingDataList = new List<BulidingTextPair>();
    private bool LeftStickcanChange = true;

    void Start()
    {
        LoadStageData(0);
        GenerateObjects();
    }

    void Update()
    {
    }
    
    public void StageLoad(int index)
    {
        LoadStageData(index);
        GenerateObjectsNew();
    }

    private void LoadStageData(int stageIndex)
    {
        buildingDataList.Clear();

        if (stageIndex < 0 || stageIndex >= allWorldDatas.worldGalleryDatas.Count) { return; }

        var stageData = allWorldDatas.worldGalleryDatas[stageIndex];
        currentMaxBuildingNum = stageData.bulidTextPair.Count;

        buildingDataList.AddRange(stageData.bulidTextPair);
    }

    private void GenerateObjects()
    {
        if (containerParent == null)
        {
            return;
        }

        if (objectPrefab == null)
        {
            return;
        }

        RectTransform parentTransform = containerParent.GetComponent<RectTransform>();
        if (parentTransform == null)
        {
            return;
        }

        foreach (Transform child in containerParent.transform)
        {
            Destroy(child.gameObject);
        }

        float currentY = 0f;

        foreach (var data in buildingDataList)
        {
            GameObject newObject = Instantiate(objectPrefab, containerParent.transform);

            Image imageComponent = newObject.GetComponent<Image>();
            if (imageComponent == null)
            {
                continue;
            }

            imageComponent.sprite = data.sprite;
            imageComponent.material = allWorldDatas.buildMat;

            RectTransform rectTransform = newObject.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                Vector2 targetSize = rectTransform.sizeDelta;

                Vector2 spriteSize = new Vector2(
                    data.sprite.bounds.size.x,
                    data.sprite.bounds.size.y
                );

                float widthRatio = targetSize.x / spriteSize.x;
                float heightRatio = targetSize.y / spriteSize.y;

                float scaleFactor = Mathf.Min(widthRatio, heightRatio);

                rectTransform.sizeDelta = spriteSize * scaleFactor;
            }

            if (rectTransform != null)
            {
                rectTransform.localPosition = new Vector3(startPosition.x, startPosition.y - currentY, startPosition.z);
            }
            else
            {
                newObject.transform.localPosition = new Vector3(startPosition.x, startPosition.y - currentY, startPosition.z);
            }

            currentY += ySpacing;
        }

    }

    private void GenerateObjectsNew()
    {
        if (containerParent == null)
        {
            return;
        }

        if (objectPrefab == null)
        {
            return;
        }

        RectTransform parentTransform = containerParent.GetComponent<RectTransform>();
        if (parentTransform == null)
        {
            return;
        }

        foreach (Transform child in containerParent.transform)
        {
            Destroy(child.gameObject);
        }

        float currentY = 0f;

        foreach (var data in buildingDataList)
        {
            GameObject newObject = Instantiate(objectPrefab, containerParent.transform);

            Image imageComponent = newObject.GetComponent<Image>();
            if (imageComponent == null)
            {
                continue;
            }

            imageComponent.sprite = data.sprite;
            imageComponent.material = allWorldDatas.buildMat;

            RectTransform rectTransform = newObject.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                Vector2 targetSize = rectTransform.sizeDelta;

                Vector2 spriteSize = new Vector2(
                    data.sprite.bounds.size.x,
                    data.sprite.bounds.size.y
                );

                float widthRatio = targetSize.x / spriteSize.x;
                float heightRatio = targetSize.y / spriteSize.y;

                float scaleFactor = Mathf.Min(widthRatio, heightRatio);

                rectTransform.sizeDelta = spriteSize * scaleFactor;
            }

            if (rectTransform != null)
            {
                rectTransform.localPosition = new Vector3(startPosition.x+86.0f, startPosition.y - currentY, startPosition.z);
            }
            else
            {
                newObject.transform.localPosition = new Vector3(startPosition.x+86.0f, startPosition.y - currentY, startPosition.z);
            }

            currentY += ySpacing;
        }

    }


    public void MoveScrollView(int direction)
    {
        RectTransform contentRect = containerParent.GetComponent<RectTransform>();

        float moveDistance = 225.0f;

        if (direction == -2)
        {
            contentRect.anchoredPosition = new Vector2(contentRect.anchoredPosition.x, 0);
            return;
        }
        else if (direction == 2)
        {
            float maxY2 = Mathf.Max(0, (containerParent.transform.childCount - 1) * moveDistance);
            contentRect.anchoredPosition = new Vector2(contentRect.anchoredPosition.x, maxY2);
            return;
        }

        float newY = contentRect.anchoredPosition.y + (direction * moveDistance);
        float minY = 0;
        float maxY = Mathf.Max(0, (containerParent.transform.childCount - 1) * moveDistance);
        newY = Mathf.Clamp(newY, minY, maxY);

        StartCoroutine(SmoothScrollTo(new Vector2(contentRect.anchoredPosition.x, newY)));
    }

    IEnumerator SmoothScrollTo(Vector2 targetPosition)
    {
        isMove = true;
        RectTransform contentRect = containerParent.GetComponent<RectTransform>();
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
}

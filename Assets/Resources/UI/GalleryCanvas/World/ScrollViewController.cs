using UnityEngine;
using UnityEngine.UI;

public class ScrollViewController : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform contentRect;
    public GameObject cursorObject;
    public void SetRect(RectTransform otherObject)
    {
        contentRect = otherObject;
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);
        scrollRect.StopMovement();
        ResetScrollPosition();
        CalculateScrollableDistance();
    }
    public RectTransform ContentRect => contentRect;

    public float scrollSpeed = 500.0f;
    private float scrollableDistance;
    private bool IsAtBottom = false;

    void Start()
    {
        SetRect(contentRect);
        scrollableDistance = 0;
    }

    void Update()
    {
        Debug.Log("awdawdawf" + scrollableDistance);
        Vector2 input = InputManager.GetAxis<Vector2>(Vec2AxisActions.LStickAxis);

        if (GalleryManager.Instance.CurrentState != GalleryState.WorldTextGallery) { return; }

        if (input.y > 0.5f)
        {
            ScrollContent(-scrollSpeed * Time.deltaTime);
        }
        else if (input.y < -0.5f)
        {
            ScrollContent(scrollSpeed * Time.deltaTime);
        }
        CheckIfAtBottom();
        if (IsAtBottom)
        {
            cursorObject.SetActive(false);
        }
        else
        {
            cursorObject.SetActive(true);
        }
    }



    void CalculateScrollableDistance()
    {
        float contentHeight = contentRect.rect.height;
        float viewportHeight = scrollRect.viewport.rect.height;

        scrollableDistance = Mathf.Max(0, contentHeight - viewportHeight);
    }

    void ResetScrollPosition()
    {
        contentRect.anchoredPosition = new Vector2(contentRect.anchoredPosition.x, 0);
    }

    void ScrollContent(float delta)
    {
        Vector2 anchoredPos = contentRect.anchoredPosition;

        anchoredPos.y += delta;

        anchoredPos.y = Mathf.Clamp(anchoredPos.y, 0, scrollableDistance);

        contentRect.anchoredPosition = anchoredPos;
    }

    void CheckIfAtBottom()
    {
        IsAtBottom = Mathf.Approximately(contentRect.anchoredPosition.y, scrollableDistance);
    }
}

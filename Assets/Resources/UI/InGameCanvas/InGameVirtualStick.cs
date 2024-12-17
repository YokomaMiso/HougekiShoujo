using UnityEngine;
using UnityEngine.EventSystems;

public class InGameVirtualStick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public RectTransform background;
    public RectTransform handle;
    private Vector2 inputVector;

    Player ownPlayer;

    Vector2 defaultPos;

    private void Start()
    {
        if (!Managers.instance.GetSmartPhoneFlag()) { Destroy(gameObject); return; }

        background.gameObject.SetActive(false);
        handle.gameObject.SetActive(false);

        ownPlayer = Managers.instance.gameManager.GetPlayer(Managers.instance.playerID);
    }

    private void FixedUpdate()
    {
        ownPlayer.SetInputVectorFromUI(inputVector);
        inputVector = Vector3.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        background.gameObject.SetActive(true);
        handle.gameObject.SetActive(true);

        defaultPos = eventData.pressPosition;
        background.transform.position = defaultPos;

        OnDrag(eventData);

        background.transform.position = defaultPos;

        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(background, eventData.position, eventData.pressEventCamera, out pos))
        {
            pos.x = (pos.x / background.sizeDelta.x);
            pos.y = (pos.y / background.sizeDelta.y);

            inputVector = new Vector2(pos.x * 2, pos.y * 2);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            handle.transform.localPosition = new Vector2(inputVector.x * (background.sizeDelta.x / 2), inputVector.y * (background.sizeDelta.y / 2));
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        background.transform.position = defaultPos;

        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(background, eventData.position, eventData.pressEventCamera, out pos))
        {
            pos.x = (pos.x / background.sizeDelta.x);
            pos.y = (pos.y / background.sizeDelta.y);

            inputVector = new Vector2(pos.x * 2, pos.y * 2);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            handle.transform.localPosition = new Vector2(inputVector.x * (background.sizeDelta.x / 2), inputVector.y * (background.sizeDelta.y / 2));
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        background.gameObject.SetActive(false);
        handle.gameObject.SetActive(false);

        handle.transform.localPosition = Vector2.zero;
    }
}
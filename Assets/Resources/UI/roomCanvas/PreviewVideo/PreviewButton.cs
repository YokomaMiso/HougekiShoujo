using UnityEngine;
using UnityEngine.UI;

public class PreviewButton : MonoBehaviour
{
    Image image;
    Image buttonImage;
    Text text;

    bool prevReady;

    void Start()
    {
        image = GetComponent<Image>();
        buttonImage = transform.GetChild(0).GetComponent<Image>();
        text = transform.GetChild(1).GetComponent<Text>();

        buttonImage.sprite = InputManager.nowButtonSpriteData.GetTrigger();
        if (Managers.instance.GetSmartPhoneFlag()) { buttonImage.color = Color.clear; }
    }

    void Update()
    {
        MachingRoomData.RoomData myRoomData = OSCManager.OSCinstance.roomData;
        if (prevReady != myRoomData.ready)
        {
            Color color;

            if (myRoomData.ready) { color = Color.clear; }
            else { color = Color.white; }

            image.color = color;
            text.color = color;
            if (!Managers.instance.GetSmartPhoneFlag()) { buttonImage.color = color; }
            prevReady = myRoomData.ready;
        }

        if (InputManager.isChangedController) { buttonImage.sprite = InputManager.nowButtonSpriteData.GetTrigger(); }
    }
}

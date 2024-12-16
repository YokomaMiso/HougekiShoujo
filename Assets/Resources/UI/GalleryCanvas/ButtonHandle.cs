using UnityEngine;

public class ButtonHandler : MonoBehaviour
{

    public OffsetChildObjects offsetObject;

    public void OnCharacterButtonClick()
    {
        GalleryManager.Instance.TransRotationIn();
        GalleryManager.Instance.SetState(GalleryState.CharacterSelect);
        GalleryManager.Instance.SetNextStage(0);
    }

    public void SliderUp()
    {
        offsetObject.MoveScrollUp();
    }

    public void SliderDown()
    {
        offsetObject.MoveScrollDown();
    }
}

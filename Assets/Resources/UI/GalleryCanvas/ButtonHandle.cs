using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    public void OnCharacterButtonClick()
    {
        GalleryManager.Instance.TransRotationIn();
    }
}

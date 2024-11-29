using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpectateCameraAnnounce : MonoBehaviour
{
    [SerializeField] Image button;

    void Start()
    {
        if (Managers.instance.GetSmartPhoneFlag()) { button.color = Color.clear; }
    }

    public void Display(bool _canChange)
    {
        transform.gameObject.SetActive(_canChange);
        button.sprite = InputManager.nowButtonSpriteData.GetSubmit();
    }

    public void PressThis()
    {
        Camera.main.GetComponent<CameraMove>().TargetChange();
    }
}

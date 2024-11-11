using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpectateCameraAnnounce : MonoBehaviour
{
    [SerializeField] Image button;

    public void Display(bool _canChange)
    {
        transform.gameObject.SetActive(_canChange);

        button.sprite = InputManager.nowButtonSpriteData.GetShoulder();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionButtonUI : MonoBehaviour
{
    Image image;
    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        if (Managers.instance.playerID != 0)
        {
            image.enabled = !OSCManager.OSCinstance.roomData.ready;
        }
    }

    public void PressOptionButton()
    {
        if (Managers.instance.UsingCanvas()) { return; }

        if (Managers.instance.playerID != 0)
        {
            if (OSCManager.OSCinstance.roomData.ready) { return; }
        }

        Managers.instance.CreateOptionCanvas();
    }
}

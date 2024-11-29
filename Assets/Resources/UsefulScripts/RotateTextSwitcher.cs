using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTextSwitcher : MonoBehaviour
{
    RotateText rotateText;

    void Start()
    {
        rotateText = GetComponent<RotateText>();
    }

    void Update()
    {
        Switch();
    }

    void Switch()
    {
        switch (Managers.instance.nowLanguage)
        {
            case LANGUAGE_NUM.JAPANESE:
                rotateText.enabled = true;
                break;
            default:
                rotateText.enabled = false;
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialFourTextDelete : MonoBehaviour
{
    [SerializeField] GameObject emotionText;

    void Start()
    {
        if (Managers.instance.GetSmartPhoneFlag())
        {
            transform.GetChild(0).GetComponent<Text>().color = Color.clear;
            transform.GetChild(1).GetComponent<Text>().color = Color.clear;

            emotionText.transform.localPosition = new Vector3(0, -320, 0);
        }
    }

}

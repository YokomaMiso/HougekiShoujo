using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInGameTimer : MonoBehaviour
{
    Text timerText;
    void Start()
    {
        timerText = GetComponent<Text>();
    }

    void Update()
    {
        float timer = Managers.instance.gameManager.roundTimer;
        if (timer > 99) { timer = 99; }

        string minute = Mathf.FloorToInt(timer / 60).ToString();
        string second = Mathf.FloorToInt(timer % 60).ToString().PadLeft(2, '0');

        timerText.text = minute + ":" + second;
    }
}

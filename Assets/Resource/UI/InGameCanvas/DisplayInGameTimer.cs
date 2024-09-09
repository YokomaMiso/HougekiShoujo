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

        timerText.text = timer.ToString("f0");
    }
}

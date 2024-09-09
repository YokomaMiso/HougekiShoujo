using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameStartAndEndText : MonoBehaviour
{
    Vector3 firstPos = new Vector3(1520, 0, 0);
    Vector3 lastPos = new Vector3(0, 0, 0);

    float timer = 0;

    RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.localPosition = firstPos;
    }

    void Update()
    {
        if (Managers.instance.gameManager.play)
        {
            timer = 0;
            GetComponent<Text>().color = Color.clear;
            return;
        }

        GetComponent<Text>().color = Color.white;

        if (Managers.instance.gameManager.start)
        {
            GetComponent<Text>().text = "Game Set";

            timer = Managers.instance.gameManager.endTimer;
            if (timer > 1) { timer = 1; }
            rectTransform.localPosition = Vector3.Lerp(firstPos, lastPos, timer);
        }
        else
        {
            timer = Managers.instance.gameManager.startTimer;

            if (timer > 2) { GetComponent<Text>().text = "Fight!"; timer = 2; }
            else { GetComponent<Text>().text = "Round " + Managers.instance.gameManager.roundCount.ToString(); }

            if (timer > 1) { timer = 1; }
            rectTransform.localPosition = Vector3.Lerp(firstPos, lastPos, timer);
        }
    }
}

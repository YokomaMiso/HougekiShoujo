using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInGameTimer : MonoBehaviour
{
    Image hourglassImage;
    Text timerText;
    [SerializeField] Sprite[] hourglassSprites;

    void Start()
    {
        hourglassImage = GetComponent<Image>();
        timerText = transform.GetChild(0).GetComponent<Text>();
    }

    void Update()
    {
        float timer = Managers.instance.gameManager.roundTimer;
        if (timer > 60) { timer = 60; }

        int spriteNum = 0;
        if (Mathf.FloorToInt(timer) % 10 == 0)
        {
            float rate = Mathf.CeilToInt(timer) - timer;
            spriteNum = (int)((hourglassSprites.Length - 1) * rate);
        }

        hourglassImage.sprite = hourglassSprites[spriteNum];

        string second = Mathf.FloorToInt(timer % 60).ToString("f0").PadLeft(2, '0');
        timerText.text = second;
    }
}

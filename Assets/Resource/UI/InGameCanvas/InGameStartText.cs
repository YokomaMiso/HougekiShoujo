using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameStartText : MonoBehaviour
{
    RectTransform rectTransform;
    Image image;
    [SerializeField]Image vfx;

    [SerializeField] Sprite fightSprite;
    [SerializeField] Sprite[] roundSprites;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.localPosition = Vector3.zero;
        image = GetComponent<Image>();
        image.color = Color.clear;

        vfx.color = Color.clear;
    }

    void Update()
    {
        IngameData.GameData hostIngameData = OSCManager.OSCinstance.GetIngameData(0).mainPacketData.inGameData;

        if (hostIngameData.play)
        {
            image.color = Color.clear;
            vfx.color = Color.clear;

            return;
        }

        float timer = hostIngameData.startTimer;

        if (4.0f > timer && timer > 3.5f)
        {
            const float maxTime = 4.0f;
            float seed = (maxTime - timer) * 20;
            float[] randoms = new float[2] { Random.Range(-seed, seed), Random.Range(-seed, seed) };

            rectTransform.localPosition = new Vector2(randoms[0] * randoms[0], randoms[1] * randoms[1]);
            image.sprite = fightSprite;
            image.color = Color.white;
            vfx.color = Color.white;

        }
        else if (3.5f > timer && timer > 2.5f)
        {
            image.sprite = roundSprites[hostIngameData.roundCount - 1];
            image.color = Color.white;
            vfx.color = Color.clear;
        }
        else
        {
            image.color = Color.clear;
            vfx.color = Color.clear;
        }
    }
}

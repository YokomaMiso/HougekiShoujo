using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameStartText : MonoBehaviour
{
    Vector2 pos = new Vector3(0, 0, 0);

    RectTransform rectTransform;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.localPosition = pos;
    }

    void Update()
    {
        IngameData.GameData hostIngameData = OSCManager.OSCinstance.GetIngameData(0).mainPacketData.inGameData;

        if (hostIngameData.play)
        {
            GetComponent<Text>().color = Color.clear;
            return;
        }

        float timer = hostIngameData.startTimer;

        if (4.0f > timer && timer > 3.5f)
        {
            const float maxTime = 4.0f;
            float seed = (maxTime - timer) * 20;
            float[] randoms = new float[2] { Random.Range(-seed, seed), Random.Range(-seed, seed) };

            rectTransform.localPosition = new Vector2(randoms[0] * randoms[0], randoms[1] * randoms[1]);
            GetComponent<Text>().text = "Fight!";
            GetComponent<Text>().color = Color.white;
        }
        else if (3.5f > timer && timer > 2.5f)
        {
            GetComponent<Text>().text = "Round " + hostIngameData.roundCount.ToString();
            GetComponent<Text>().color = Color.white;
        }
        else
        {
            GetComponent<Text>().color = Color.clear;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameStartAndEndText : MonoBehaviour
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
        if (Managers.instance.gameManager.play)
        {
            GetComponent<Text>().color = Color.clear;
            return;
        }


        if (Managers.instance.gameManager.start)
        {
            GetComponent<Text>().text = "Game Set";
            float timer = Managers.instance.gameManager.endTimer;
        }
        else
        {
            float timer = Managers.instance.gameManager.startTimer;

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
                GetComponent<Text>().text = "Round " + Managers.instance.gameManager.roundCount.ToString();
                GetComponent<Text>().color = Color.white;
            }
            else
            {
                GetComponent<Text>().color = Color.clear;
            }
        }
    }
}

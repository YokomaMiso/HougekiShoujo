using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditBlack : MonoBehaviour
{
    float timer;
    const float blackAlphaStartTime = 1;
    const float blackAlphaMinusTime = 7;
    const float blackAlphaPlusTime = 142;
    const float blackAlphaEndTime = 148;

    Image black;

    void Start()
    {
        black = transform.GetChild(0).GetComponent<Image>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer <= blackAlphaMinusTime)
        {
            float nowRate = 1.0f - (timer - blackAlphaStartTime) / (blackAlphaMinusTime - blackAlphaStartTime);
            black.color = Color.black * nowRate;
        }

        if (timer > blackAlphaPlusTime)
        {
            float nowRate = (timer - blackAlphaPlusTime) / (blackAlphaEndTime - blackAlphaPlusTime);
            black.color = Color.black * nowRate;
        }
    }
}

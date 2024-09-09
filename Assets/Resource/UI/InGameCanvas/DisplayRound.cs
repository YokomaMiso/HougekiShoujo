using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayRound : MonoBehaviour
{
    Text roundText;
    int roundNumber;

    void Start()
    {
        roundText = GetComponent<Text>();
    }
    void Update()
    {
        if (roundNumber!= Managers.instance.gameManager.roundCount)
        {
            roundNumber = Managers.instance.gameManager.roundCount;
            roundText.text=roundNumber.ToString();
        }
    }
}

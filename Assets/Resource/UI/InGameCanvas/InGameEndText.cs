using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameEndText : MonoBehaviour
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
        if (!Managers.instance.gameManager.end) { GetComponent<Text>().color = Color.clear; return; }

        GetComponent<Text>().color = Color.white;

        float timer = Managers.instance.gameManager.endTimer;

        string[] displayTeam = new string[2] { "Team A ", "Team B " };
        string[] displayResult = new string[2] { "Get Round", "Win" };

        GameManager gm = Managers.instance.gameManager;

        bool gameEnd = false;
        for (int i = 0; i < 2; i++) { if (gm.roundWinCount[i] >= 3) { gameEnd = true; } }

        int teamIndex = 0;
        if (gm.deadPlayerCount[0] > gm.deadPlayerCount[1]) { teamIndex = 1; }

        int resultIndex = 0;
        if (gameEnd) { resultIndex = 1; }

        GetComponent<Text>().text = displayTeam[teamIndex] + displayResult[resultIndex];
        GetComponent<Text>().color = Color.white;
    }
}
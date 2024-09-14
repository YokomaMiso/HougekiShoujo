using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayWinCount : MonoBehaviour
{
    [SerializeField] int teamNum = 0;
    [SerializeField] Sprite[] scoreSprites;

    void Update()
    {
        int score = Managers.instance.gameManager.roundWinCount[teamNum];

        for (int i = 0; i < transform.childCount; i++)
        {
            int spriteNum = 0;
            if (i < score) { spriteNum = 1; }

            transform.GetChild(i).GetComponent<Image>().sprite = scoreSprites[spriteNum];
        }
    }
}

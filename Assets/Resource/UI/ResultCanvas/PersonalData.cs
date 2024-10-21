using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonalData : MonoBehaviour
{
    readonly Color[] textColor = new Color[2] { Color.white, Color.black };

    Text[] texts;

    public void SetData(ResultScoreBoard.KDFData _kdfData, int _colorNum)
    {
        texts = new Text[transform.childCount];
        for (int i = 0; i < transform.childCount; i++) 
        {
            texts[i] = transform.GetChild(i).GetComponent<Text>();
            texts[i].color = textColor[_colorNum]; 
        }

        texts[0].text = _kdfData.playerName;
        texts[1].text = _kdfData.killCount.ToString();
        texts[2].text = _kdfData.deathCount.ToString();
        texts[3].text = _kdfData.friendlyFireCount.ToString();
    }
}

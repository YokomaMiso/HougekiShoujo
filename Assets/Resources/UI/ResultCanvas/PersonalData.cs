using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Unity.VisualScripting.StickyNote;

public class PersonalData : MonoBehaviour
{
    readonly Color[] textColor = new Color[3] { Color.white, Color.black, new Color(1.0f, 1.0f, 0.4f) };

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

    public void YouAreMVP()
    {
        for (int i = 0; i < transform.childCount; i++) { texts[i].color = textColor[2]; }
    }
}

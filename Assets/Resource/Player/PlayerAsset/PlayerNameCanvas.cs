using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameCanvas : MonoBehaviour
{
    readonly Color[] constColor = new Color[2] { new Color(0.1255f, 0.3137f, 0.8941f, 1), new Color(1, 0.125f, 0.125f, 1) };

    public void SetName(string _name, int team)
    {
        transform.GetComponent<Text>().text = _name;
        transform.GetComponent<Outline>().effectColor = constColor[team];
    }
}

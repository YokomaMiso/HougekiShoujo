using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameCanvas : MonoBehaviour
{
    Player ownerPlayer;
    public void SetPlayer(Player _player) { ownerPlayer = _player; }

    readonly Color[] constColor = new Color[2] { new Color(0.1255f, 0.3137f, 0.8941f, 1), new Color(1, 0.125f, 0.125f, 1) };
    Image bulletIcon;

    public void SetName(string _name, int team)
    {
        transform.GetComponent<Text>().text = _name;
        Outline[] outlines = transform.GetComponents<Outline>();
        for (int i = 0; i < outlines.Length; i++) { outlines[i].effectColor = constColor[team]; }

        bulletIcon = transform.GetChild(0).GetComponent<Image>();
        bulletIcon.color = Color.clear;
    }

    public void ChangeShellIconColor(int _num)
    {
        int colorValue = _num;
        Color iconColor = Color.white * colorValue;
        bulletIcon.color = iconColor;
    }
}

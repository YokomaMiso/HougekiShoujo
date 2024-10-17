using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharaFrame : MonoBehaviour
{
    Image charaIllust;
    Text playerName;

    public void SetData(Sprite _illust,string _playerName)
    {
        charaIllust = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        playerName = transform.GetChild(1).GetComponent<Text>();

        charaIllust.sprite = _illust;
        playerName.text = _playerName;
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DisplayShellIcon : MonoBehaviour
{
    [SerializeField] Sprite iconFrame;

    Image[] icons;
    Vector3[] iconPos;

    const int iconFrameNum = PlayerData.maxShellCount;

    void Start()
    {
        icons = new Image[PlayerData.maxShellCount + 1];
        iconPos = new Vector3[PlayerData.maxShellCount];

        for (int i = 0; i < PlayerData.maxShellCount; i++)
        {
            icons[i] = transform.GetChild(i).GetComponent<Image>();
            icons[i].sprite = Player.instance.playerData.GetShell(i).GetSprite();
            iconPos[i] = icons[i].transform.position;
        }

        icons[iconFrameNum] = transform.GetChild(iconFrameNum).GetComponent<Image>();
        icons[iconFrameNum].sprite = null;
        icons[iconFrameNum].color = Color.clear;
    }

    void Update()
    {
        CheckPlayerShell();
    }

    void CheckPlayerShell()
    {
       int canonState = Player.instance.GetCanonState();

        switch (canonState)
        {
            case -1:
                icons[iconFrameNum].sprite = null;
                icons[iconFrameNum].color = Color.clear;
                break;
            default:
                icons[iconFrameNum].sprite = iconFrame;
                icons[iconFrameNum].color = Color.white;
                icons[iconFrameNum].transform.position = iconPos[canonState];
                break;
        }
    }
}

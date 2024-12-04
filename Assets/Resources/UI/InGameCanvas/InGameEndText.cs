using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameEndText : MonoBehaviour
{
    RectTransform rectTransform;
    Image image;
    [SerializeField] Image vfx;

    [SerializeField] Sprite[] sprites;

    bool soundPlayed;
    [SerializeField] AudioClip[] roundSetVoice;
    [SerializeField] AudioClip[] winVoice;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.localPosition = Vector3.zero;
        image = GetComponent<Image>();
        image.color = Color.clear;

        vfx.color = Color.clear;

        //soundPlayed = false;
    }

    void Update()
    {
        IngameData.GameData hostIngameData = OSCManager.OSCinstance.GetIngameData(0).mainPacketData.inGameData;

        if (hostIngameData.winner == -1)
        {
            image.color = Color.clear;
            vfx.color = Color.clear;
            //soundPlayed = false;

            return;
        }

        bool gameEnd = false;
        if (hostIngameData.winCountTeamA >= 3) { gameEnd = true; }
        else if (hostIngameData.winCountTeamB >= 3) { gameEnd = true; }

        int teamIndex = hostIngameData.winner;

        int resultIndex = 0;
        if (gameEnd)
        {
            resultIndex++;
            vfx.color = Color.white;
        }

        if (teamIndex != -1)
        {
            image.sprite = sprites[teamIndex + resultIndex * 2];
            image.color = Color.white;

            /*
            if (!soundPlayed)
            {
                if (gameEnd) { SoundManager.PlaySFXForUI(winVoice[teamIndex]); }
                else { SoundManager.PlaySFXForUI(roundSetVoice[teamIndex]); }
                soundPlayed = true;

            }
            */
        }
    }
}
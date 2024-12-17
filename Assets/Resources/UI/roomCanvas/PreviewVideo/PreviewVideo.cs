using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PreviewVideo : MonoBehaviour
{
    int characterNum;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] VideoClip[] clips;
    [SerializeField] Image button;
    [SerializeField] Text annouce;

    RawImage videoImage;
    Color nowColor;
    float timer;
    const float brightTime = 0.5f;

    readonly string[] announceTexts = new string[2]
    {
        "Close",
        "Press to Close",
    };

    void OnEnable()
    {
        MachingRoomData.RoomData roomData = OSCManager.OSCinstance.roomData;
        characterNum = roomData.selectedCharacterID;

        videoPlayer.clip = clips[characterNum];
        videoPlayer.loopPointReached += LoopPointReached;
        videoPlayer.Play();
        button.sprite = InputManager.nowButtonSpriteData.GetCancel();
        if (Managers.instance.GetSmartPhoneFlag()) 
        {
            button.color = Color.clear;
            annouce.text = announceTexts[1];
        }

        nowColor = Color.black;
        videoImage = videoPlayer.GetComponent<RawImage>();
        timer = 0;

        Managers.instance.PlaySFXForUI(0);
    }

    void Update()
    {
        if (InputManager.GetKeyDown(BoolActions.EastButton)) { CloseBehavior(); }
        if (InputManager.isChangedController) { button.sprite = InputManager.nowButtonSpriteData.GetCancel(); }

        if (timer >= brightTime) { return; }

        timer += Time.deltaTime;
        if (timer >= brightTime) { timer = brightTime; }

        float nowRate = Mathf.Clamp01(timer / brightTime);
        nowColor = new Color(nowRate, nowRate, nowRate, 1);
        videoImage.color = nowColor;
    }

    void CloseBehavior()
    {
        nowColor = Color.black;
        videoImage.color = nowColor;
        gameObject.SetActive(false);
        timer = 0;

        Managers.instance.PlaySFXForUI(1);
    }

    public void LoopPointReached(VideoPlayer vp) { CloseBehavior(); }
    public void CloseWindow() { CloseBehavior(); }
}

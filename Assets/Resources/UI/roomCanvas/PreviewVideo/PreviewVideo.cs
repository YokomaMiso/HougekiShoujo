using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PreviewVideo : MonoBehaviour
{
    int characterNum;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] VideoClip[] clips;
    [SerializeField] Image button;

    void OnEnable()
    {
        MachingRoomData.RoomData roomData = OSCManager.OSCinstance.roomData;
        characterNum = roomData.selectedCharacterID;

        videoPlayer.clip = clips[characterNum];
        videoPlayer.loopPointReached += LoopPointReached;
        videoPlayer.Play();
        button.sprite = InputManager.nowButtonSpriteData.GetCancel();
    }

    void Update()
    {
        if (InputManager.GetKeyDown(BoolActions.EastButton)) { gameObject.SetActive(false); }
        if (InputManager.isChangedController) { button.sprite = InputManager.nowButtonSpriteData.GetCancel(); }
    }

    public void LoopPointReached(VideoPlayer vp) { gameObject.SetActive(false); }
    public void CloseWindow() { gameObject.SetActive(false); }
}

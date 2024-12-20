using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitCheck : MonoBehaviour
{
    RoomCanvasBehavior owner;
    public void SetOwner(RoomCanvasBehavior _owner) { owner = _owner; }

    bool exit;
    int id;
    [SerializeField] GameObject announceTextInstance;
    TextChangerAtLanguage[] announceText;

    [SerializeField] Image[] buttonIcon;

    private void OnEnable()
    {
        if (announceText == null) { return; }
        announceText[id].ChangeText();
    }
    void Start()
    {
        announceText = announceTextInstance.GetComponents<TextChangerAtLanguage>();

        id = (int)Mathf.Clamp01(Managers.instance.playerID);
        announceText[id].enabled = true;
        announceText[(id + 1) % 2].enabled = false;

        if (Managers.instance.GetSmartPhoneFlag())
        {
            for (int i = 0; i < buttonIcon.Length; i++)
            {
                Destroy(buttonIcon[i].gameObject);
            }
        }
        else
        {
            ChangeDisplayButtons();
        }
    }

    void Update()
    {
        if (InputManager.isChangedController) { ChangeDisplayButtons(); }

        if (InputManager.GetKeyDown(BoolActions.SouthButton))
        {
            PressSubmit();
            return;
        }
        else if (InputManager.GetKeyDown(BoolActions.EastButton))
        {
            PressCancel();
            return;
        }
    }

    void ChangeDisplayButtons()
    {
        if (Managers.instance.GetSmartPhoneFlag()) { return; }

        buttonIcon[0].sprite = InputManager.nowButtonSpriteData.GetSubmit();
        buttonIcon[1].sprite = InputManager.nowButtonSpriteData.GetCancel();
    }

    public void PressSubmit()
    {
        if (exit) { return; }

        exit = true;
        owner.rm.BackToTitle();
    }

    public void PressCancel()
    {
        if (exit) { return; }
        gameObject.SetActive(false);
    }
}

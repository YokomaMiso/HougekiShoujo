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
    TextChangerAtLanguage announceText;

    [SerializeField] Image[] buttonIcon;

    private void OnEnable()
    {
        Managers.instance.PlaySFXForUI(1);

        if (announceText == null) { return; }
        announceText.ChangeText();
    }
    void Start()
    {
        announceText = announceTextInstance.GetComponent<TextChangerAtLanguage>();

        id = (int)Mathf.Clamp01(Managers.instance.playerID);

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
        Managers.instance.PlaySFXForUI(3);
        owner.rm.BackToTitle();
    }

    public void PressCancel()
    {
        if (exit) { return; }
        Managers.instance.PlaySFXForUI(1);
        gameObject.SetActive(false);
    }
}

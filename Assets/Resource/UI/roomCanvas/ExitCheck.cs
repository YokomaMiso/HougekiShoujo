using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitCheck : MonoBehaviour
{
    RoomCanvasBehavior owner;
    public void SetOwner(RoomCanvasBehavior _owner) { owner = _owner; }

    bool exit;
    [SerializeField] GameObject announceTextInstance;
    TextChangerAtLanguage[] announceText;

    void Start()
    {
        announceText = announceTextInstance.GetComponents<TextChangerAtLanguage>();

        int id = (int)Mathf.Clamp01(Managers.instance.playerID);
        announceText[id].enabled = true;
        announceText[(id + 1) % 2].enabled = false;
    }

    void Update()
    {
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

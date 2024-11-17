using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class InputName : MonoBehaviour
{
    InputField inputField;
    TitleCanvasBehavior parent;

    public void SetParent(TitleCanvasBehavior _parent) { parent = _parent; }

    string prevText;
    [SerializeField] AudioClip inputTextSFX;

    [SerializeField] GameObject unlockWindowPrefab;
    GameObject unlockWindowInstance;

    readonly string[] unlockText = new string[(int)UNLOCK_ITEM.MAX_NUM]
    {
        "DELETE",
        "wing",
        //"13",
    };

    void Start()
    {
        inputField = transform.GetChild(1).GetComponent<InputField>();
        inputField.text = Managers.instance.optionData.playerName;
        inputField.Select();

        prevText = inputField.text;
    }
    void Update()
    {
        if (parent.GetTitleState() != TITLE_STATE.INPUT_NAME) { return; }
        if (unlockWindowInstance) { return; }

        if (prevText != inputField.text)
        {
            SoundManager.PlaySFXForUI(inputTextSFX);
            prevText = inputField.text;
        }

        //AddText();
        //SubText();
        Submit();
        Cancel();
    }

    void Submit()
    {
        //決定ボタンが押されてないなら
        if (!InputManager.GetKeyDown(BoolActions.SouthButton))
        {
            //エンターキーが押されてないならリターン
            if (!Keyboard.current[Key.Enter].wasPressedThisFrame) { return; }
        }

        if (Keyboard.current.FindKeyOnCurrentKeyboardLayout("z").wasPressedThisFrame) { return; }
        if (Keyboard.current.FindKeyOnCurrentKeyboardLayout("j").wasPressedThisFrame) { return; }

        GoToNextScene();
    }

    bool SeachUnlockKeyWord()
    {
        string text = inputField.text;
        for (int i = 0; i < (int)UNLOCK_ITEM.MAX_NUM; i++)
        {
            if (text == unlockText[i])
            {
                if (!Managers.instance.unlockFlag[i])
                {
                    Managers.instance.unlockFlag[i] = true;
                    unlockWindowInstance = Instantiate(unlockWindowPrefab, parent.transform);
                    unlockWindowInstance.GetComponent<UnlockWindow>().SetTextNum(i);
                    return true;
                }
            }
        }

        return false;
    }

    void GoToNextScene()
    {
        if (SeachUnlockKeyWord()) { return; }

        string playerName = inputField.text;
        if (playerName == "") { playerName = "Player"; }
        Managers.instance.optionData.playerName = playerName;
        Managers.instance.SaveOptionData(Managers.instance.optionData);
        OSCManager.OSCinstance.roomData.playerName = playerName;

        parent.PlaySFXInTitle(0);
        parent.ChangeTitleState(TITLE_STATE.CHANGE_TO_CONNECTION);
    }

    void Cancel()
    {
        //キャンセルボタンが押されてないなら
        if (!InputManager.GetKeyDown(BoolActions.EastButton)) { return; }

        if (Keyboard.current.FindKeyOnCurrentKeyboardLayout("k").wasPressedThisFrame) { return; }
        if (Keyboard.current.FindKeyOnCurrentKeyboardLayout("x").wasPressedThisFrame) { return; }
        if (Keyboard.current[Key.LeftShift].wasPressedThisFrame) { return; }
        if (Keyboard.current[Key.RightShift].wasPressedThisFrame) { return; }

        parent.PlaySFXInTitle(1);
        parent.ChangeTitleState(TITLE_STATE.SELECT);
    }

    public void SubmitFromUIButton()
    {
        GoToNextScene();
    }

    void AddText()
    {
        int charNum;
        if (Keyboard.current[Key.LeftShift].isPressed || Keyboard.current[Key.RightShift].isPressed) { charNum = 'A'; }
        else { charNum = 'a'; }

        for (int i = 0; i < 'z' + 1 - 'a'; i++)
        {
            char c = (char)(charNum + i);
            string t = "";
            t += c;

            if (Keyboard.current.FindKeyOnCurrentKeyboardLayout(t).wasPressedThisFrame)
            {
                AddTextToInputField(t);
            }
        }

        charNum = '0';
        for (int i = 0; i < '9' + 1 - '0'; i++)
        {
            char c = (char)(charNum + i);
            string t = "";
            t += c;

            if (Keyboard.current.FindKeyOnCurrentKeyboardLayout(t).wasPressedThisFrame)
            {
                AddTextToInputField(t);
            }
        }
    }
    void AddTextToInputField(string _c)
    {
        if (inputField.text.Length >= inputField.characterLimit) { return; }
        inputField.text += _c;
    }
    void SubText()
    {
        if (inputField.text.Length == 0) { return; }

        if (Keyboard.current[Key.Backspace].wasPressedThisFrame)
        {
            string t = inputField.text.Remove(inputField.text.Length - 1);
            inputField.text = t;
        }
    }
}

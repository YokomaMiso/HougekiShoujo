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

    void Start()
    {
        inputField = transform.GetChild(1).GetComponent<InputField>();
        inputField.text = Managers.instance.optionData.playerName;
        //inputField.Select();
    }
    void Update()
    {
        if (parent.GetTitleState() != TITLE_STATE.INPUT_NAME) { return; }

        AddText();
        SubText();
        Submit();
        Cancel();
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

    void GoToNextScene()
    {
        string playerName = inputField.text;
        if (playerName == "") { playerName = "Player"; }
        Managers.instance.optionData.playerName = playerName;
        Managers.instance.SaveOptionData(Managers.instance.optionData);
        OSCManager.OSCinstance.roomData.playerName = playerName;
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

        parent.ChangeTitleState(TITLE_STATE.SELECT);
    }

    public void SubmitFromUIButton()
    {
        GoToNextScene();
    }
}

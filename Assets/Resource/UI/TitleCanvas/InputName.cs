using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        inputField.Select();
    }

    void Update()
    {
        if (parent.GetTitleState() != TITLE_STATE.INPUT_NAME) { return; }

        if (InputManager.GetKeyDown(BoolActions.SouthButton))
        {
            string playerName = inputField.text;
            if (playerName == "") { playerName = "Player " + (Managers.instance.playerID + 1).ToString(); }
            Managers.instance.optionData.playerName = playerName;
            Managers.instance.SaveOptionData(Managers.instance.optionData);
            OSCManager.OSCinstance.roomData.playerName = playerName;
            parent.ChangeTitleState(TITLE_STATE.CHANGE_TO_CONNECTION);
        }
        else if (InputManager.GetKeyDown(BoolActions.EastButton))
        {
            parent.ChangeTitleState(TITLE_STATE.SELECT);
        }
    }
}

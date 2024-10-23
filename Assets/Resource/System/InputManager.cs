using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//-------------------------------------------------------------------------------------------------------------//
//--新しく登録する際は Assets/Resource/System にあるPlayerControllerのActionsの名前と並びを統一させてください--//
//-------------------------------------------------------------------------------------------------------------//

public enum Vec2AxisActions
{
    RStickAxis = 0,
    LStickAxis,
}

public enum BoolActions
{
    RStickButton = 0,
    SouthButton,
    EastButton,
    RightShoulder,
    LeftShoulder,
    Option,
    RadioChat1,
    RadioChat2,
    RadioChat3,
    RadioChat4,
    RadioChat5,
    RadioChat6,
    RadioChat7,
    RadioChat8
}

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputActionMap actionMap;

    private static List<InputAction> boolActions = new List<InputAction>();
    private static List<InputAction> vec2Actions = new List<InputAction>();
    
    private static InputAction.CallbackContext inputContext;

    private static List<string> boolKeyStrings = new List<string>();
    private static List<string> vec2KeyStrings = new List<string>();

    private void Awake()
    {
        foreach (BoolActions an in BoolActions.GetValues(typeof(BoolActions)))
        {
            boolKeyStrings.Add(an.ToString());
        }

        foreach (Vec2AxisActions an in BoolActions.GetValues(typeof(Vec2AxisActions)))
        {
            vec2KeyStrings.Add(an.ToString());
        }

        playerInput = GetComponent<PlayerInput>();

        actionMap = playerInput.currentActionMap;
        
        if(actionMap != null)
        {
            foreach(InputAction action in actionMap.actions)
            {
                if(action.type == InputActionType.Button)
                {
                    boolActions.Add(action);

                    Debug.Log(action.name + "がButtonにセットされました");
                }
                else if(action.type == InputActionType.Value)
                {
                    vec2Actions.Add(action);

                    Debug.Log(action.name + "がvec2にセットされました");
                }
            }
        }
        else
        {
            Debug.LogError("actionMapの取得に失敗しました");
        }
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    public static Vector2 GetAxis(Vec2AxisActions an)
    {
        return vec2Actions[(int)an].ReadValue<Vector2>();
    }

    public static bool GetKey(BoolActions an)
    {
        return boolActions[(int)an].IsPressed();
    }

    public static bool GetKeyDown(BoolActions an)
    {
        Debug.Log(boolActions[(int)an].name);

        return boolActions[(int)an].WasPressedThisFrame();
    }

    public static bool GetKeyUp(BoolActions an)
    {
        return boolActions[(int)an].WasReleasedThisFrame();
    }
}
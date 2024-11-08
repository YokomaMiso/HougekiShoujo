using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//-------------------------------------------------------------------------------------------------------------//
//--�V�����o�^����ۂ� Assets/Resource/System �ɂ���PlayerController��Actions�̖��O�ƕ��т𓝈ꂳ���Ă�������--//
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
    LeftTrigger,
    Option,
    RadioChat1,
    RadioChat2,
    RadioChat3,
    RadioChat4,
    RadioChat5,
    RadioChat6,
    RadioChat7,
    RadioChat8,
    TouchTap,
    LeftClick
}

public enum ControllerType
{
    XInput = 0,
    DirectInput,
    SwitchInput,
    Keyboard,
    TouchScreen,
    Other,
    None
}

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    private static PlayerInput playerInput;
    private static InputActionMap actionMap;

    // �A�N�V�����ۑ����X�g
    private static List<InputAction> boolActions = new List<InputAction>();
    private static List<InputAction> vec2Actions = new List<InputAction>();

    // delay�p�̎��Ԍv�Z���X�g
    private static List<float> axisTimeList = new List<float>();

    public static ControllerType currentController { get; private set; } = ControllerType.None;

    public static bool isChangedController { get; private set; } = false;

    // �e�f�o�C�X�̂��ׂẴL�[���P�Ƀo�C���h����InputAction�i�L�[��ʌ��m�p�j
    private InputAction xInputAnyKey              = new InputAction(type: InputActionType.PassThrough, binding: "<XInputController>/*", interactions: "Press");
    private InputAction dualShock4AnyKey          = new InputAction(type: InputActionType.PassThrough, binding: "<DualShockGamepad>/*", interactions: "Press");
    private InputAction detectDualSenseAnyKey     = new InputAction(type: InputActionType.PassThrough, binding: "<DualSenseGamepadHID>/*", interactions: "Press");
    private InputAction switchProControllerAnyKey = new InputAction(type: InputActionType.PassThrough, binding: "<SwitchProControllerHID>/*", interactions: "Press");

    private void Awake()
    {
        // vector2��action���镪�̃^�C�}�[�����
        foreach (Vec2AxisActions an in BoolActions.GetValues(typeof(Vec2AxisActions)))
        {
            axisTimeList.Add(0.0f);
        }

        playerInput = GetComponent<PlayerInput>();

        actionMap = playerInput.currentActionMap;
        
        if(actionMap != null)
        {
            // �A�N�V�����^�C�v�ɍ��킹�ăf�[�^��ۑ�
            foreach(InputAction action in actionMap.actions)
            {
                if(action.type == InputActionType.Button)
                {
                    boolActions.Add(action);

                    //Debug.Log(action.name + "��Button�ɃZ�b�g����܂���");
                }
                else if(action.type == InputActionType.Value)
                {
                    vec2Actions.Add(action);

                    //Debug.Log(action.name + "��vec2�ɃZ�b�g����܂���");
                }
            }
        }
        else
        {
            Debug.LogError("actionMap�̎擾�Ɏ��s���܂���");
        }

        EnableInput();

        // �L�[���m�p�A�N�V�����̗L����
        xInputAnyKey.Enable();
        dualShock4AnyKey.Enable();
        detectDualSenseAnyKey.Enable();
        switchProControllerAnyKey.Enable();
    }

    private void Update()
    {
        isChangedController = false;

        UpdateCurrentController();

        Debug.Log(currentController);
        Debug.Log(isChangedController);
    }

    /// <summary>
    /// �A�i���O�X�e�B�b�N�̌��ݒl�擾
    /// </summary>
    /// <typeparam name="T">�擾�������^�Abool�܂���vector2</typeparam>
    /// <param name="an">Action�ɓo�^����Ă���L�[</param>
    /// <returns>�X�e�B�b�N���|����Ă��� = true or ���ݒl: �|����Ă��Ȃ� = false or (0, 0)</returns>
    public static T GetAxis<T>(Vec2AxisActions an) where T : struct
    {
        object val = new object();

        if (typeof(T) == typeof(bool))
        {
            val = true;

            return (T)val;
        }
        else if (typeof(T) == typeof(Vector2))
        {
            val = vec2Actions[(int)an].ReadValue<Vector2>();

            return (T)val;
        }
        else
        {
            Debug.LogError("bool��������vector2�ȊO���w�肳��Ă��܂�");
        }

        return default(T);
    }

    /// <summary>
    /// �A�i���O�X�e�B�b�N�������ꂽ�u�Ԃ̎擾
    /// </summary>
    /// <typeparam name="T">�擾�������^�Abool�܂���vector2</typeparam>
    /// <param name="an">Action�ɓo�^����Ă���L�[</param>
    /// <returns>�X�e�B�b�N���|���ꂽ�ŏ��̃t���[�� = true or ���ݒl: ����ȊO = false or (0, 0)</returns>
    public static T GetAxisDown<T>(Vec2AxisActions an) where T : struct
    {
        object val = new object();

        if (vec2Actions[(int)an].WasPressedThisFrame())
        {

            if (typeof(T) == typeof(bool))
            {
                val = true;

                return (T)val;
            }
            else if (typeof(T) == typeof(Vector2))
            {
                val = vec2Actions[(int)an].ReadValue<Vector2>().normalized;

                return (T)val;
            }
            else
            {
                Debug.LogError("bool��������vector2�ȊO���w�肳��Ă��܂�");
            }
        }

        return default(T);
    }

    /// <summary>
    /// �A�i���O�X�e�B�b�N�����ꂽ�u�Ԃ̎擾
    /// </summary>
    /// <typeparam name="T">�擾�������^�Abool�܂���vector2</typeparam>
    /// <param name="an">Action�ɓo�^����Ă���L�[</param>
    /// <returns>�X�e�B�b�N�������ꂽ�ŏ��̃t���[�� = true or ���ݒl: ����ȊO = false or (0, 0)</returns>
    public static T GetAxisUp<T>(Vec2AxisActions an)where T : struct
    {
        object val = new object();

        if (vec2Actions[(int)an].WasReleasedThisFrame())
        {
            if (typeof(T) == typeof(bool))
            {
                val = true;

                return (T)val;
            }
            else if(typeof(T) == typeof(Vector2))
            {
                val = vec2Actions[(int)an].ReadValue<Vector2>().normalized;

                return (T)val;
            }
            else
            {
                Debug.LogError("bool��������vector2�ȊO���w�肳��Ă��܂�");
            }
        }

        return default(T);
    }

    /// <summary>
    /// �A�i���O�X�e�B�b�N�̓��Ԋu�擾
    /// </summary>
    /// <typeparam name="T">�擾�������^�Abool�܂���vector2</typeparam>
    /// <param name="an">Action�ɓo�^����Ă���L�[</param>
    /// <param name="_time">���l�𓾂�Ԋu�i�b�j</param>
    /// <returns>�X�e�B�b�N���|���ꂽ�ŏ��̃t���[���Ƃ���ȍ~�w�肵�����Ԍo�ߌ� = true or ���ݒl: ����ȊO = false or (0, 0)</returns>
    public static T GetAxisDelay<T>(Vec2AxisActions an, float _time) where T : struct
    {
        object val = new object();
        int actionNum = (int)an;

        if (vec2Actions[actionNum].WasPressedThisFrame())
        {
            if (typeof(T) == typeof(bool))
            {
                val = true;

                return (T)val;
            }
            else if (typeof(T) == typeof(Vector2))
            {
                val = vec2Actions[actionNum].ReadValue<Vector2>().normalized;

                return (T)val;
            }
            else
            {
                Debug.LogError("bool��������vector2�ȊO���w�肳��Ă��܂�");
            }
        }

        if (vec2Actions[actionNum].IsPressed())
        {
            axisTimeList[actionNum] += Time.deltaTime;

            if (typeof(T) == typeof(bool))
            {
                if (axisTimeList[actionNum] > _time)
                {
                    val = true;

                    axisTimeList[actionNum] = 0;
                    return (T)val;
                }
            }
            else if (typeof(T) == typeof(Vector2))
            {
                if (axisTimeList[actionNum] > _time)
                {
                    val = vec2Actions[actionNum].ReadValue<Vector2>().normalized;

                    axisTimeList[actionNum] = 0;
                    return (T)val;
                }
            }
            else
            {
                Debug.LogError("bool��������vector2�ȊO���w�肳��Ă��܂�");
            }
        }

        if(vec2Actions[actionNum].WasReleasedThisFrame())
        {
            axisTimeList[actionNum] = 0;
        }

        return default(T);
    }

    /// <summary>
    /// bool�^�̃L�[���͎擾
    /// </summary>
    /// <param name="an">Action�ɓo�^����Ă���L�[</param>
    /// <returns>������Ă��� = true: ������Ă��Ȃ� = false</returns>
    public static bool GetKey(BoolActions an)
    {
        return boolActions[(int)an].IsPressed();
    }
    
    /// <summary>
    /// bool�^�̃L�[���͎擾
    /// </summary>
    /// <param name="an">Action�ɓo�^����Ă���L�[</param>
    /// <returns>�����ꂽ�ŏ��̂P�t���[�� = true: ����ȊO = false</returns>
    public static bool GetKeyDown(BoolActions an)
    {
        return boolActions[(int)an].WasPressedThisFrame();
    }

    /// <summary>
    /// bool�^�̃L�[���͎擾
    /// </summary>
    /// <param name="an">Action�ɓo�^����Ă���L�[</param>
    /// <returns>�������ŏ��̂P�t���[�� = true: ����ȊO = false</returns>
    public static bool GetKeyUp(BoolActions an)
    {
        return boolActions[(int)an].WasReleasedThisFrame();
    }

    private void UpdateCurrentController()
    {
        var beforeDeviceType = currentController;
        
        switch (playerInput.currentControlScheme)
        {
            case "Gamepad":
                
                if (xInputAnyKey.triggered)
                {
                    currentController = ControllerType.XInput;
                }
                else if (dualShock4AnyKey.triggered || detectDualSenseAnyKey.triggered)
                {
                    currentController = ControllerType.DirectInput;
                }
                else if (switchProControllerAnyKey.triggered)
                {
                    currentController = ControllerType.SwitchInput;
                }

                if (beforeDeviceType != currentController)
                {
                    isChangedController = true;
                }

                return;
            case "Key&Mouse":

                currentController = ControllerType.Keyboard;

                if (beforeDeviceType != currentController)
                {
                    isChangedController = true;
                }

                return;
            case "Touchscreen":

                currentController = ControllerType.TouchScreen;

                if (beforeDeviceType != currentController)
                {
                    isChangedController = true;
                }

                return;
            case "OtherControll":

                currentController = ControllerType.Other;

                if (beforeDeviceType != currentController)
                {
                    isChangedController = true;
                }

                return;
        }

        currentController = ControllerType.None;

        if (beforeDeviceType != currentController)
        {
            isChangedController = true;
        }
    }

    /// <summary>
    /// InputSystem�̗L����
    /// </summary>
    public static void EnableInput()
    {
        actionMap.Enable();

        return;
    }

    /// <summary>
    /// InputSystem�̖�����
    /// </summary>
    public static void DisableInput()
    {
        actionMap.Disable();

        return;
    }
}
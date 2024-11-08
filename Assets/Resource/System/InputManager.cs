using System;
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
    LeftTrigger,
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
    private static InputActionMap actionMap;

    // アクション保存リスト
    private static List<InputAction> boolActions = new List<InputAction>();
    private static List<InputAction> vec2Actions = new List<InputAction>();

    // delay用の時間計算リスト
    private static List<float> axisTimeList = new List<float>();

    private void Awake()
    {
        // vector2のactionある分のタイマーを作る
        foreach (Vec2AxisActions an in BoolActions.GetValues(typeof(Vec2AxisActions)))
        {
            axisTimeList.Add(0.0f);
        }

        playerInput = GetComponent<PlayerInput>();

        actionMap = playerInput.currentActionMap;

        if (actionMap != null)
        {
            // アクションタイプに合わせてデータを保存
            foreach (InputAction action in actionMap.actions)
            {
                if (action.type == InputActionType.Button)
                {
                    boolActions.Add(action);

                    //Debug.Log(action.name + "がButtonにセットされました");
                }
                else if (action.type == InputActionType.Value)
                {
                    vec2Actions.Add(action);

                    //Debug.Log(action.name + "がvec2にセットされました");
                }
            }
        }
        else
        {
            Debug.LogError("actionMapの取得に失敗しました");
        }
    }

    /// <summary>
    /// アナログスティックの現在値取得
    /// </summary>
    /// <typeparam name="T">取得したい型、boolまたはvector2</typeparam>
    /// <param name="an">Actionに登録されているキー</param>
    /// <returns>スティックが倒されている = true or 現在値: 倒されていない = false or (0, 0)</returns>
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
            Debug.LogError("boolもしくはvector2以外が指定されています");
        }

        return default(T);
    }

    /// <summary>
    /// アナログスティックが押された瞬間の取得
    /// </summary>
    /// <typeparam name="T">取得したい型、boolまたはvector2</typeparam>
    /// <param name="an">Actionに登録されているキー</param>
    /// <returns>スティックが倒された最初のフレーム = true or 現在値: それ以外 = false or (0, 0)</returns>
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
                Debug.LogError("boolもしくはvector2以外が指定されています");
            }
        }

        return default(T);
    }

    /// <summary>
    /// アナログスティックが離れた瞬間の取得
    /// </summary>
    /// <typeparam name="T">取得したい型、boolまたはvector2</typeparam>
    /// <param name="an">Actionに登録されているキー</param>
    /// <returns>スティックが離された最初のフレーム = true or 現在値: それ以外 = false or (0, 0)</returns>
    public static T GetAxisUp<T>(Vec2AxisActions an) where T : struct
    {
        object val = new object();

        if (vec2Actions[(int)an].WasReleasedThisFrame())
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
                Debug.LogError("boolもしくはvector2以外が指定されています");
            }
        }

        return default(T);
    }

    /// <summary>
    /// アナログスティックの等間隔取得
    /// </summary>
    /// <typeparam name="T">取得したい型、boolまたはvector2</typeparam>
    /// <param name="an">Actionに登録されているキー</param>
    /// <param name="_time">数値を得る間隔（秒）</param>
    /// <returns>スティックが倒された最初のフレームとそれ以降指定した時間経過後 = true or 現在値: それ以外 = false or (0, 0)</returns>
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
                Debug.LogError("boolもしくはvector2以外が指定されています");
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
                Debug.LogError("boolもしくはvector2以外が指定されています");
            }
        }

        if (vec2Actions[actionNum].WasReleasedThisFrame())
        {
            axisTimeList[actionNum] = 0;
        }

        return default(T);
    }

    /// <summary>
    /// bool型のキー入力取得
    /// </summary>
    /// <param name="an">Actionに登録されているキー</param>
    /// <returns>押されている = true: 押されていない = false</returns>
    public static bool GetKey(BoolActions an)
    {
        return boolActions[(int)an].IsPressed();
    }

    /// <summary>
    /// bool型のキー入力取得
    /// </summary>
    /// <param name="an">Actionに登録されているキー</param>
    /// <returns>押された最初の１フレーム = true: それ以外 = false</returns>
    public static bool GetKeyDown(BoolActions an)
    {
        return boolActions[(int)an].WasPressedThisFrame();
    }

    /// <summary>
    /// bool型のキー入力取得
    /// </summary>
    /// <param name="an">Actionに登録されているキー</param>
    /// <returns>離した最初の１フレーム = true: それ以外 = false</returns>
    public static bool GetKeyUp(BoolActions an)
    {
        return boolActions[(int)an].WasReleasedThisFrame();
    }

    /// <summary>
    /// InputSystemの有効化
    /// </summary>
    public static void EnableInput()
    {
        actionMap.Enable();

        return;
    }

    /// <summary>
    /// InputSystemの無効化
    /// </summary>
    public static void DisableInput()
    {
        actionMap.Disable();

        return;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class IngameStickGraphic : MonoBehaviour
{
    [SerializeField] Transform bg;
    [SerializeField] Transform handle;

    void Start()
    {
        bg.gameObject.SetActive(false);
        handle.gameObject.SetActive(false);
    }

    public void Press()
    {
        bg.gameObject.SetActive(true);
        handle.gameObject.SetActive(true);

        bg.position = Pointer.current.position.ReadValue();
    }

    public void Release()
    {
        bg.gameObject.SetActive(false);
        handle.gameObject.SetActive(false);
    }

    public void Move()
    {
        handle.localPosition = InputManager.GetAxis<Vector2>(Vec2AxisActions.LStickAxis) * 100;
    }
}

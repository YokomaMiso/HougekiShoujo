using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FastChat : MonoBehaviour
{
    public Material fastChat;
    public void SetFastChatMat(Material _mat) { fastChat = _mat; }
    public Material GetFastChatMat() { return fastChat; }

    RawImage rawImage;
    GameObject selecter;

    const float scaleDuration = 0.2f;
    //public float rotationTurns = 15;

    private bool isChatActive = false;
    private float currentScale = 10.0f;
    private float currentRotation = 0.0f;
    private float scaleStartValue = 2.0f;
    private float scaleEndValue = 1.0f;

    private float elapsedTime = 0f;

    void Start()
    {
        rawImage = transform.GetChild(0).GetComponent<RawImage>();
        selecter = transform.GetChild(1).gameObject;
        selecter.SetActive(false);
        //SetVisibility(false);
    }

    void Update()
    {
        //ButtonCheck();

        if (isChatActive)
        {
            UpdateShader();
        }
    }

    void ButtonCheck()
    {
        if (Input.GetButtonDown("LB"))
        {
            isChatActive = true;
            SetVisibility(true);
            elapsedTime = 0f;
        }

        if (Input.GetButtonUp("LB"))
        {
            isChatActive = false;
            SetVisibility(false);
            ResetShaderValues();
        }
    }

    public void ReceiverFromRadioChat(bool _start)
    {
        if (_start)
        {
            isChatActive = true;
            SetVisibility(true);
            elapsedTime = 0f;
        }
        else
        {
            isChatActive = false;
            SetVisibility(false);
            ResetShaderValues();
        }
    }

    void UpdateShader()
    {
        if (fastChat != null)
        {
            elapsedTime += Time.deltaTime;

            float progress = Mathf.Clamp01(elapsedTime / scaleDuration);

            currentScale = Mathf.Lerp(scaleStartValue, scaleEndValue, progress);
            fastChat.SetFloat("_Scale", currentScale);

            currentRotation = 360.0f * Mathf.Deg2Rad * progress;
            fastChat.SetFloat("_Rotation", currentRotation);

            selecter.SetActive(progress >= 1);

            int region = GetJoystickRegion();
            fastChat.SetFloat("_SelectedRegion", region);

        }
    }

    void ResetShaderValues()
    {
        if (fastChat != null)
        {
            currentScale = scaleStartValue;
            fastChat.SetFloat("_Scale", currentScale);

            currentRotation = 0f;
            fastChat.SetFloat("_Rotation", currentRotation);

            elapsedTime = 0f;
        }
    }

    public int GetJoystickRegion()
    {
        Vector3 joyStick = Vector3.zero;
        joyStick.x = Input.GetAxis("RightStickHorizontal");
        joyStick.y = Input.GetAxis("RightStickVertical");

        if (joyStick.magnitude < 0.2f) { return -1; }

        float angle = Mathf.Atan2(-joyStick.x, -joyStick.y) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;

        int region = Mathf.Clamp(Mathf.FloorToInt(angle / 45.0f), 0, 7);
        Debug.Log("Atan" + region);

        return region;
    }

    void SetVisibility(bool isVisible)
    {
        if (rawImage != null)
        {
            rawImage.enabled = isVisible;
        }
    }

}

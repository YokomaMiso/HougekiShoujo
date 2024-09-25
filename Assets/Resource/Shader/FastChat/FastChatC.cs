using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FastChat : MonoBehaviour
{
    public Material fastChat;
    public void SetFastChatMat(Material _mat) { fastChat = _mat; }
    public Material GetFastChatMat() { return fastChat; }

    public float scaleDuration = 1.0f;         
    public float rotationTurns = 3;              

    private bool isChatActive = false;
    private float currentScale = 10.0f;              
    private float currentRotation = 0.0f;           
    private float scaleStartValue = 3.0f;           
    private float scaleEndValue = 1.0f;            

    private float elapsedTime = 0f;                

    void Start()
    {
        SetVisibility(false);
    }

    void Update()
    {
        ButtonCheck();

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

    void UpdateShader()
    {
        if (fastChat != null)
        {
            elapsedTime += Time.deltaTime;

            float progress = Mathf.Clamp01(elapsedTime / scaleDuration);



            currentScale = Mathf.Lerp(scaleStartValue, scaleEndValue, progress);
            fastChat.SetFloat("_Scale", currentScale);

            currentRotation = 360.0f * rotationTurns * progress;
            fastChat.SetFloat("_Rotation", currentRotation);


            int region = GetJoystickRegion();
            fastChat.SetFloat("_SelectedRegion", region);

        }
    }

    // リセット
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
        float joystickX = Input.GetAxis("RightStickHorizontal"); //右スティックの水平入力
        float joystickY = Input.GetAxis("RightStickVertical");   //右スティックの垂直入力


        //中心ゾーン判定
        if (joystickX * joystickX + joystickY * joystickY < 0.1f * 0.1f)
        {
            return -1; //中央（-1）を返す
        }

        //右スティックの角度を計算
        float angle = Mathf.Atan2(joystickY, joystickX) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360; //正規化

        //360度を8つの領域に分割し、それに基づいて領域番号を返す
        int region = Mathf.FloorToInt(angle / 45.0f); //45度ごとに領域を分割

        Debug.Log("Atan" + region);
        Debug.Log("Atan" + region);
        Debug.Log("Atan" + region);
        Debug.Log("Atan" + region);

        return region; //0～7の領域番号を返す
    }

    void SetVisibility(bool isVisible)
    {
        RawImage rawImage = gameObject.GetComponent<RawImage>();
        if (rawImage != null)
        {
            rawImage.enabled = isVisible;
        }
    }

}

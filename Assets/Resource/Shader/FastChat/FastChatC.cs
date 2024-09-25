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

    // �ꥻ�å�
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
        float joystickX = Input.GetAxis("RightStickHorizontal"); //�ҥ��ƥ��å���ˮƽ����
        float joystickY = Input.GetAxis("RightStickVertical");   //�ҥ��ƥ��å��δ�ֱ����


        //���ĥ��`���ж�
        if (joystickX * joystickX + joystickY * joystickY < 0.1f * 0.1f)
        {
            return -1; //���루-1���򷵤�
        }

        //�ҥ��ƥ��å��νǶȤ�Ӌ��
        float angle = Mathf.Atan2(joystickY, joystickX) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360; //��Ҏ��

        //360�Ȥ�8�Ĥ��I��˷ָ������˻��Ť����I�򷬺Ť򷵤�
        int region = Mathf.FloorToInt(angle / 45.0f); //45�Ȥ��Ȥ��I���ָ�

        Debug.Log("Atan" + region);
        Debug.Log("Atan" + region);
        Debug.Log("Atan" + region);
        Debug.Log("Atan" + region);

        return region; //0��7���I�򷬺Ť򷵤�
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

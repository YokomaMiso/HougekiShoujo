using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class OptionCanvasBehavior : MonoBehaviour
{
    GameManager gameManager;
    public void SetGameManager(GameManager _gameManager) { gameManager = _gameManager; }

    OptionData optionData;
    int selectNum = 0;
    bool isCanSelect = true;
    float timer = 0;
    const float cursorTimer = 0.15f;

    void Start()
    {
        optionData = Managers.instance.GetOptionData();
        transform.GetChild(1).GetComponent<VolumeIndexSetting>().SetValue(optionData.masterVolume);
        transform.GetChild(2).GetComponent<VolumeIndexSetting>().SetValue(optionData.bgmVolume);
        transform.GetChild(3).GetComponent<VolumeIndexSetting>().SetValue(optionData.sfxVolume);
        transform.GetChild(4).GetComponent<RadioBoxIndexSetting>().SetValue(optionData.cameraShakeOn);
        transform.GetChild(5).GetComponent<VolumeIndexSetting>().SetValue(optionData.mortarSensitive);

        transform.GetChild(1).GetChild(0).GetComponent<Image>().color = SelectColor(true);
    }
    Color SelectColor(bool _select)
    {
        Color color = Color.white;
        if (_select) { color = Color.yellow; }

        return color;
    }

    void Update()
    {
        CursorMove();
        ChangeSelectedValue();
    }


    void CursorMove()
    {
        float value = Input.GetAxis("Vertical");

        //�J�[�\���ړ�
        if (Mathf.Abs(value) > 0.9f)
        {
            if (isCanSelect)
            {
                transform.GetChild(selectNum + 1).GetChild(0).GetComponent<Image>().color = SelectColor(false);

                if (value > 0) { selectNum -= 1; }
                else { selectNum += 1; }

                if (selectNum < 0) { selectNum = 0; }
                if (selectNum > 5) { selectNum = 5; }

                isCanSelect = false;

                transform.GetChild(selectNum + 1).GetChild(0).GetComponent<Image>().color = SelectColor(true);
            }
        }
        //�O�t���[���̏��ۑ�
        else if (Mathf.Abs(value) < 0.2f)
        {
            isCanSelect = true;
        }
    }

    void ChangeSelectedValue()
    {
        float value = Input.GetAxis("Horizontal");

        if (Mathf.Abs(value) > 0.6f)
        {
            if (timer == 0)
            {
                switch (selectNum)
                {
                    case 3:
                        transform.GetChild(selectNum + 1).GetComponent<RadioBoxIndexSetting>().SetValue(value < 0);
                        break;
                    case 5:
                        break;
                    default:
                        float applyValue = 1;
                        if (value < 0) { applyValue *= -1; }
                        transform.GetChild(selectNum + 1).GetComponent<VolumeIndexSetting>().AddValue(applyValue);
                        break;
                }
            }

            timer += Time.deltaTime;
            if (timer > cursorTimer) { timer = 0; }
        }
        else
        {
            timer = 0;
        }

        optionData.masterVolume = transform.GetChild(1).GetComponent<VolumeIndexSetting>().GetValue();
        optionData.bgmVolume = transform.GetChild(2).GetComponent<VolumeIndexSetting>().GetValue();
        optionData.sfxVolume = transform.GetChild(3).GetComponent<VolumeIndexSetting>().GetValue();
        optionData.cameraShakeOn = transform.GetChild(4).GetComponent<RadioBoxIndexSetting>().on;
        optionData.mortarSensitive = transform.GetChild(5).GetComponent<VolumeIndexSetting>().GetValue();

        if (Input.GetButtonDown("Submit"))
        {
            if (selectNum == 5)
            {
                Managers.instance.SaveOptionData(optionData);
                Destroy(gameObject);
            }
        }
    }
}

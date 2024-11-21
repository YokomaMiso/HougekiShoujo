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
        transform.GetChild(4).GetComponent<VolumeIndexSetting>().SetValue(optionData.voiceVolume);
        transform.GetChild(5).GetComponent<RadioBoxIndexSetting>().SetValue(optionData.cameraShakeOn);
        transform.GetChild(6).GetComponent<VolumeIndexSetting>().SetValue(optionData.mortarSensitive);
        transform.GetChild(7).GetComponent<LanguageRadioBoxSetting>().SetValue(optionData.languageNum);

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
        Vector2 value = InputManager.GetAxis<Vector2>(Vec2AxisActions.LStickAxis);

        //カーソル移動
        if (Mathf.Abs(value.y) > 0.8f)
        {
            if (isCanSelect)
            {
                transform.GetChild(selectNum + 1).GetChild(0).GetComponent<Image>().color = SelectColor(false);

                if (value.y > 0) { selectNum -= 1; }
                else { selectNum += 1; }

                if (selectNum < 0) { selectNum = 0; }
                if (selectNum > 7) { selectNum = 7; }

                isCanSelect = false;

                transform.GetChild(selectNum + 1).GetChild(0).GetComponent<Image>().color = SelectColor(true);
            }
        }
        //前フレームの情報保存
        else if (Mathf.Abs(value.y) < 0.2f)
        {
            isCanSelect = true;
        }
    }

    void ChangeSelectedValue()
    {
        Vector2 value = InputManager.GetAxis<Vector2>(Vec2AxisActions.LStickAxis);

        if (Mathf.Abs(value.x) > 0.8f)
        {
            if (timer == 0)
            {
                float applyValue = 1;

                switch (selectNum)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                        if (value.x < 0) { applyValue *= -1; }
                        transform.GetChild(selectNum + 1).GetComponent<VolumeIndexSetting>().AddValue(applyValue);
                        SoundManager.masterVolume = transform.GetChild(1).GetComponent<VolumeIndexSetting>().GetValue();
                        SoundManager.bgmVolume = transform.GetChild(2).GetComponent<VolumeIndexSetting>().GetValue();
                        SoundManager.sfxVolume = transform.GetChild(3).GetComponent<VolumeIndexSetting>().GetValue();
                        SoundManager.voiceVolume = transform.GetChild(4).GetComponent<VolumeIndexSetting>().GetValue();

                        float nowVolume = SoundManager.masterVolume * SoundManager.bgmVolume;
                        SoundManager.BGMVolumeChange(nowVolume);

                        break;

                    case 4:
                        transform.GetChild(selectNum + 1).GetComponent<RadioBoxIndexSetting>().SetValue(value.x < 0);
                        break;

                    case 5:
                        if (value.x < 0) { applyValue *= -1; }
                        transform.GetChild(selectNum + 1).GetComponent<VolumeIndexSetting>().AddValue(applyValue);
                        break;

                    case 6:
                        int valueToInt;
                        if (value.x > 0) { valueToInt = 1; }
                        else { valueToInt = -1; }
                        int returnValue= transform.GetChild(selectNum + 1).GetComponent<LanguageRadioBoxSetting>().AddValue(valueToInt);
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
        optionData.voiceVolume = transform.GetChild(4).GetComponent<VolumeIndexSetting>().GetValue();
        optionData.cameraShakeOn = transform.GetChild(5).GetComponent<RadioBoxIndexSetting>().on;
        optionData.mortarSensitive = transform.GetChild(6).GetComponent<VolumeIndexSetting>().GetValue();
        optionData.languageNum = transform.GetChild(7).GetComponent<LanguageRadioBoxSetting>().num;

        if (InputManager.GetKeyDown(BoolActions.SouthButton))
        {
            if (selectNum == 7)
            {
                Submit();
            }
        }
    }

    public void Submit()
    {
        Managers.instance.SaveOptionData(optionData);
        Destroy(gameObject);
    }
}

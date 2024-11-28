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
    int selectCategory = 0;
    const int categoryMaxNum = 4;
    const float cursorTimer = 0.15f;

    int settingNum = 0;
    readonly int[] settingMaxNum = new int[categoryMaxNum] { 5, 3, 2, 0 };

    bool inCategory;

    [SerializeField] public Sprite[] levelOneWindow;
    [SerializeField] public Sprite[] levelTwoWindow;
    [SerializeField] public Sprite[] levelThreePen;
    [SerializeField] public Sprite[] levelFourWindow;

    [SerializeField] Image soundCategory;
    [SerializeField] Image gamePlayCategory;
    [SerializeField] Image languageCategory;
    [SerializeField] Image submit;
    Image[] categorys;

    [SerializeField] GameObject soundSetting;
    [SerializeField] GameObject gamePlaySetting;
    [SerializeField] GameObject languageSetting;
    [SerializeField] GameObject emptySetting;
    GameObject[] settings;

    void Start()
    {
        optionData = Managers.instance.GetOptionData();

        categorys = new Image[categoryMaxNum];
        categorys[0] = soundCategory;
        categorys[1] = gamePlayCategory;
        categorys[2] = languageCategory;
        categorys[3] = submit;

        settings = new GameObject[categoryMaxNum];
        settings[0] = soundSetting;
        settings[1] = gamePlaySetting;
        settings[2] = languageSetting;
        settings[3] = emptySetting;

        soundSetting.transform.GetChild(0).GetComponent<VolumeIndexSetting>().SetValue(optionData.masterVolume);
        soundSetting.transform.GetChild(1).GetComponent<VolumeIndexSetting>().SetValue(optionData.bgmVolume);
        soundSetting.transform.GetChild(2).GetComponent<VolumeIndexSetting>().SetValue(optionData.sfxVolume);
        soundSetting.transform.GetChild(3).GetComponent<VolumeIndexSetting>().SetValue(optionData.voiceVolume);
        gamePlaySetting.transform.GetChild(0).GetComponent<RadioBoxIndexSetting>().SetValue(optionData.cameraShakeOn);
        gamePlaySetting.transform.GetChild(1).GetComponent<VolumeIndexSetting>().SetValue(optionData.mortarSensitive);
        languageSetting.transform.GetChild(0).GetComponent<LanguageRadioBoxSetting>().SetValue(optionData.languageNum);

        for (int i = 0; i < categoryMaxNum; i++)
        {
            if (i == selectCategory)
            {
                categorys[i].sprite = levelOneWindow[1];
                settings[i].SetActive(true);
            }
            else
            {
                categorys[i].sprite = levelOneWindow[0];
                settings[i].SetActive(false);
            }
        }
    }
    void Update()
    {
        Vector2 value = InputManager.GetAxisDelay<Vector2>(Vec2AxisActions.LStickAxis, cursorTimer);

        if (inCategory)
        {
            ChangeSelectedValue(value.x);
            ChangeSettingNum(value.y);
        }
        else
        {
            CategoryMove(value.y);
            if (InputManager.GetKeyDown(BoolActions.SouthButton)) { SubmitInCategory(); }
        }
    }


    void CategoryMove(float _y)
    {
        //ƒJ[ƒ\ƒ‹ˆÚ“®
        if (Mathf.Abs(_y) > 0.8f)
        {
            categorys[selectCategory].sprite = levelOneWindow[0];
            settings[selectCategory].SetActive(false);

            if (_y > 0) { selectCategory = (selectCategory + categoryMaxNum - 1) % categoryMaxNum; }
            else { selectCategory = (selectCategory + 1) % categoryMaxNum; }

            categorys[selectCategory].sprite = levelOneWindow[1];
            settings[selectCategory].SetActive(true);
        }
    }
    void SubmitInCategory()
    {
        switch (selectCategory)
        {
            default:
                inCategory = true;
                settingNum = 0;
                break;
            case 3:
                Submit();
                break;
        }
    }

    void ChangeSettingNum(float _y)
    {
        if (Mathf.Abs(_y) < 0.8f) { return; }

        if (_y < 0) { settingNum = (settingNum + 1) % settingMaxNum[selectCategory]; }
        else { settingNum = (settingNum + settingMaxNum[selectCategory] - 1) % settingMaxNum[selectCategory]; }
    }

    void ChangeSelectedValue(float _x)
    {
        if (Mathf.Abs(_x) < 0.8f) { return; }

        float applyValue = 1;

        switch (selectCategory)
        {
            //sound
            case 0:
                if (settingNum < settingMaxNum[selectCategory] - 1)
                {
                    if (_x < 0) { applyValue *= -1; }
                    soundSetting.transform.GetChild(settingNum).GetComponent<VolumeIndexSetting>().AddValue(applyValue);
                    SoundManager.masterVolume = soundSetting.transform.GetChild(0).GetComponent<VolumeIndexSetting>().GetValue();
                    SoundManager.bgmVolume = soundSetting.transform.GetChild(1).GetComponent<VolumeIndexSetting>().GetValue();
                    SoundManager.sfxVolume = soundSetting.transform.GetChild(2).GetComponent<VolumeIndexSetting>().GetValue();
                    SoundManager.voiceVolume = soundSetting.transform.GetChild(3).GetComponent<VolumeIndexSetting>().GetValue();

                    float nowVolume = SoundManager.masterVolume * SoundManager.bgmVolume;
                    SoundManager.BGMVolumeChange(nowVolume);
                }
                break;
            //gameplay
            case 1:
                break;
            //language
            case 2:
                break;
            //submit
            case 3:
                break;
        }
        /*
        if (Mathf.Abs(value.x) > 0.8f)
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
                    int returnValue = transform.GetChild(selectNum + 1).GetComponent<LanguageRadioBoxSetting>().AddValue(valueToInt);
                    break;
            }
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
            }
        }
        */
    }

    public void Submit()
    {
        Managers.instance.SaveOptionData(optionData);
        Destroy(gameObject);
    }
}

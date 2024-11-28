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

    [SerializeField] Sprite[] levelOneWindow;
    [SerializeField] Sprite[] levelTwoWindow;
    [SerializeField] Sprite[] levelThreePen;
    [SerializeField] Sprite[] levelFourWindow;

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

        RadioBoxIndexSetting.levelFourWindow = levelFourWindow;
        LanguageRadioBoxSetting.levelFourWindow = levelFourWindow;

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
            if (InputManager.GetKeyDown(BoolActions.SouthButton))
            {
                if (settingNum == settingMaxNum[selectCategory] - 1) { SubmitInSetting(); }
            }
        }
        else
        {
            CategoryMove(value.y);
            if (InputManager.GetKeyDown(BoolActions.SouthButton)) { SubmitInCategory(); }
        }

        optionData.masterVolume = soundSetting.transform.GetChild(0).GetComponent<VolumeIndexSetting>().GetValue();
        optionData.bgmVolume = soundSetting.transform.GetChild(1).GetComponent<VolumeIndexSetting>().GetValue();
        optionData.sfxVolume = soundSetting.transform.GetChild(2).GetComponent<VolumeIndexSetting>().GetValue();
        optionData.voiceVolume = soundSetting.transform.GetChild(3).GetComponent<VolumeIndexSetting>().GetValue();
        optionData.cameraShakeOn = gamePlaySetting.transform.GetChild(0).GetComponent<RadioBoxIndexSetting>().on;
        optionData.mortarSensitive = gamePlaySetting.transform.GetChild(1).GetComponent<VolumeIndexSetting>().GetValue();
        optionData.languageNum = languageSetting.transform.GetChild(0).GetComponent<LanguageRadioBoxSetting>().num;
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
            case 0:
                inCategory = true;
                settingNum = 0;
                soundSetting.transform.GetChild(settingNum).GetChild(0).GetComponent<Image>().sprite = levelTwoWindow[1];
                soundSetting.transform.GetChild(settingNum).GetChild(1).GetComponent<Slider>().handleRect.GetComponent<Image>().sprite = levelThreePen[1];
                soundSetting.transform.GetChild(settingNum).GetChild(2).GetComponent<Image>().sprite = levelFourWindow[1];
                break;
            case 1:
                inCategory = true;
                settingNum = 0;
                gamePlaySetting.transform.GetChild(settingNum).GetChild(0).GetComponent<Image>().sprite = levelTwoWindow[1];
                break;
            case 2:
                inCategory = true;
                settingNum = 0;
                languageSetting.transform.GetChild(settingNum).GetChild(0).GetComponent<Image>().sprite = levelTwoWindow[1];
                break;

            case 3:
                Submit();
                break;
        }
    }

    public void CategoryMoveFromUI(int _num)
    {
        categorys[selectCategory].sprite = levelOneWindow[0];
        settings[selectCategory].SetActive(false);

        selectCategory = _num;

        categorys[selectCategory].sprite = levelOneWindow[1];
        settings[selectCategory].SetActive(true);

        SubmitInCategory();
    }

    void ChangeSettingNum(float _y)
    {
        if (Mathf.Abs(_y) < 0.8f) { return; }

        if (_y < 0) { settingNum = (settingNum + 1) % settingMaxNum[selectCategory]; }
        else { settingNum = (settingNum + settingMaxNum[selectCategory] - 1) % settingMaxNum[selectCategory]; }

        UpdateDisplaySetting();
    }
    public void ChangeSettingNumFromUI(int _num)
    {
        settingNum = _num;
        UpdateDisplaySetting();
    }

    void UpdateDisplaySetting()
    {
        switch (selectCategory)
        {
            //sound
            case 0:
                for (int i = 0; i < settingMaxNum[selectCategory]; i++)
                {
                    if (i == settingNum) { soundSetting.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = levelTwoWindow[1]; }
                    else { soundSetting.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = levelTwoWindow[0]; }

                    if (i < settingMaxNum[selectCategory] - 1)
                    {
                        if (i == settingNum)
                        {
                            soundSetting.transform.GetChild(i).GetChild(1).GetComponent<Slider>().handleRect.GetComponent<Image>().sprite = levelThreePen[1];
                            soundSetting.transform.GetChild(i).GetChild(2).GetComponent<Image>().sprite = levelFourWindow[1];
                        }
                        else
                        {
                            soundSetting.transform.GetChild(i).GetChild(1).GetComponent<Slider>().handleRect.GetComponent<Image>().sprite = levelThreePen[0];
                            soundSetting.transform.GetChild(i).GetChild(2).GetComponent<Image>().sprite = levelFourWindow[0];
                        }
                    }

                }
                break;
            //gameplay
            case 1:
                switch (settingNum)
                {
                    case 0:
                        gamePlaySetting.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = levelTwoWindow[1];
                        gamePlaySetting.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = levelTwoWindow[0];
                        gamePlaySetting.transform.GetChild(1).GetChild(1).GetComponent<Slider>().handleRect.GetComponent<Image>().sprite = levelThreePen[0];
                        gamePlaySetting.transform.GetChild(1).GetChild(2).GetComponent<Image>().sprite = levelFourWindow[0];
                        gamePlaySetting.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = levelTwoWindow[0];
                        break;
                    case 1:
                        gamePlaySetting.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = levelTwoWindow[0];
                        gamePlaySetting.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = levelTwoWindow[1];
                        gamePlaySetting.transform.GetChild(1).GetChild(1).GetComponent<Slider>().handleRect.GetComponent<Image>().sprite = levelThreePen[1];
                        gamePlaySetting.transform.GetChild(1).GetChild(2).GetComponent<Image>().sprite = levelFourWindow[1];
                        gamePlaySetting.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = levelTwoWindow[0];
                        break;
                    default:
                        gamePlaySetting.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = levelTwoWindow[0];
                        gamePlaySetting.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = levelTwoWindow[0];
                        gamePlaySetting.transform.GetChild(1).GetChild(1).GetComponent<Slider>().handleRect.GetComponent<Image>().sprite = levelThreePen[0];
                        gamePlaySetting.transform.GetChild(1).GetChild(2).GetComponent<Image>().sprite = levelFourWindow[0];
                        gamePlaySetting.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = levelTwoWindow[1];
                        break;
                }
                break;
            //language
            case 2:
                if (0 == settingNum)
                {
                    languageSetting.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = levelTwoWindow[1];
                    languageSetting.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = levelTwoWindow[0];
                }
                else
                {
                    languageSetting.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = levelTwoWindow[0];
                    languageSetting.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = levelTwoWindow[1];
                }
                break;
            //submit
            case 3:
                break;
        }
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
                switch (settingNum)
                {
                    case 0:
                        gamePlaySetting.transform.GetChild(settingNum).GetComponent<RadioBoxIndexSetting>().SetValue(_x < 0);
                        break;
                    case 1:
                        if (_x < 0) { applyValue *= -1; }
                        gamePlaySetting.transform.GetChild(settingNum).GetComponent<VolumeIndexSetting>().AddValue(applyValue);
                        break;
                }
                break;
            //language
            case 2:
                if (settingNum < settingMaxNum[selectCategory] - 1)
                {
                    int valueToInt;
                    if (_x > 0) { valueToInt = 1; }
                    else { valueToInt = -1; }
                    languageSetting.transform.GetChild(settingNum).GetComponent<LanguageRadioBoxSetting>().AddValue(valueToInt);
                }
                break;
            //submit
            case 3:
                break;
        }
    }

    void SubmitInSetting()
    {
        switch (selectCategory)
        {
            //sound
            case 0:
                for (int i = 0; i < settingMaxNum[selectCategory]; i++)
                {
                    soundSetting.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = levelTwoWindow[0];

                    if (i < settingMaxNum[selectCategory] - 1)
                    {
                        soundSetting.transform.GetChild(i).GetChild(1).GetComponent<Slider>().handleRect.GetComponent<Image>().sprite = levelThreePen[0];
                        soundSetting.transform.GetChild(i).GetChild(2).GetComponent<Image>().sprite = levelFourWindow[0];
                    }
                }
                break;

            //gameplay
            case 1:
                gamePlaySetting.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = levelTwoWindow[0];
                gamePlaySetting.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = levelTwoWindow[0];
                gamePlaySetting.transform.GetChild(1).GetChild(1).GetComponent<Slider>().handleRect.GetComponent<Image>().sprite = levelThreePen[0];
                gamePlaySetting.transform.GetChild(1).GetChild(2).GetComponent<Image>().sprite = levelFourWindow[0];
                gamePlaySetting.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = levelTwoWindow[0];
                break;

            //language
            case 2:
                languageSetting.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = levelTwoWindow[0];
                languageSetting.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = levelTwoWindow[0];
                break;
        }

        inCategory = false;
        settingNum = 0;
    }
    public void SubmitInSettingFromUI()
    {
        SubmitInSetting();
    }

    public void Submit()
    {
        Managers.instance.SaveOptionData(optionData);
        Destroy(gameObject);
    }
}

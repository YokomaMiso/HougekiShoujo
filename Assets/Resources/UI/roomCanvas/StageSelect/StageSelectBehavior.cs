using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectBehavior : MonoBehaviour
{
    float timer;
    bool start;
    bool end;

    const float binderMoveTime = 0.5f;

    float stumpTimer;
    const float stumpMoveTime = 0.5f;

    readonly Vector3 startPos = new Vector3(-416, -880);
    readonly Vector3 endPos = new Vector3(-416, 36);

    [SerializeField] Transform binder;
    Image stump;

    [SerializeField] SelecterArrow[] selectArrows;
    [SerializeField] TextChangerAtLanguage stageAnnounce;

    [SerializeField] AudioClip binderIn;
    [SerializeField] AudioClip binderOut;
    [SerializeField] AudioClip stageChange;
    [SerializeField] AudioClip stageDecide;

    bool inEffect;
    bool outEffect;

    void OnEnable()
    {
        start = true;
        end = false;
        binder.localPosition = startPos;
        binder.GetChild(1).localScale = Vector3.one * 0.8f;
        stump = binder.GetChild(1).GetComponent<Image>();
        stump.color = Color.clear;

        DisplayStageUpdate(OSCManager.OSCinstance.roomData.stageNum);
        stageAnnounce.ChangeText();

        inEffect = false;
        outEffect = false;
    }

    void Update()
    {
        if (start)
        {
            timer += Time.deltaTime;
            if (timer >= binderMoveTime)
            {
                timer = binderMoveTime;
                start = false;
            }

            float nowRate = Mathf.Sqrt(timer / binderMoveTime);
            binder.localPosition = Vector3.Lerp(startPos, endPos, nowRate);

            if (!inEffect)
            {
                SoundManager.PlaySFXForUI(binderIn);
                inEffect = true;
            }
        }
        else if (!end)
        {
            StageNumChange();
            PressSubmit();
            float colorAlpha = 0.5f + (Mathf.Sin(Time.time * 6) / 3);
            stump.color = new Color(1, 1, 1, colorAlpha);
        }
        else
        {
            stump.color = Color.white;

            if (stumpTimer >= stumpMoveTime)
            {
                binder.GetChild(1).localScale = Vector3.one * 0.8f;
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    timer = 0;
                    gameObject.SetActive(false);
                    stump.color = Color.clear;
                    stumpTimer = 0;
                }

                float nowRate = Mathf.Sqrt(timer / binderMoveTime);
                binder.localPosition = Vector3.Lerp(startPos, endPos, nowRate);
                if (!outEffect)
                {
                    SoundManager.PlaySFXForUI(binderOut);
                    outEffect = true;
                }
            }
            else
            {
                stumpTimer += Time.deltaTime;
                float nowRate = Mathf.Clamp01(1.0f - stumpTimer * 2 / stumpMoveTime);
                float scaleValue = Mathf.Clamp(4.0f * nowRate, 1.0f, 4.0f);
                binder.GetChild(1).localScale = Vector3.one * scaleValue;
            }
        }
    }

    void StageNumChange()
    {
        float input = InputManager.GetAxisDelay<Vector2>(Vec2AxisActions.LStickAxis, 0.5f).x;
        if (Mathf.Abs(input) >= 0.9f)
        {
            MachingRoomData.RoomData myRoomData = OSCManager.OSCinstance.roomData;
            int stageNum = myRoomData.stageNum;

            int stageMaxCount = Managers.instance.gameManager.allStageData.GetStageMaxCount();

            if (input > 0)
            {
                stageNum = (stageNum + 1) % stageMaxCount;
                selectArrows[1].SetAdd();
            }
            else
            {
                stageNum = (stageNum + (stageMaxCount - 1)) % stageMaxCount;
                selectArrows[0].SetAdd();
            }

            OSCManager.OSCinstance.roomData.stageNum = stageNum;
            SoundManager.PlaySFXForUI(stageChange);

            DisplayStageUpdate(stageNum);
        }
    }

    public void StageNumChangeFromUI(int _num)
    {
        MachingRoomData.RoomData myRoomData = OSCManager.OSCinstance.roomData;
        int stageNum = myRoomData.stageNum;

        int stageMaxCount = Managers.instance.gameManager.allStageData.GetStageMaxCount();

        if (_num > 0)
        {
            stageNum = (stageNum + 1) % stageMaxCount;
            selectArrows[1].SetAdd();
        }
        else
        {
            stageNum = (stageNum + (stageMaxCount - 1)) % stageMaxCount;
            selectArrows[0].SetAdd();
        }

        OSCManager.OSCinstance.roomData.stageNum = stageNum;
        SoundManager.PlaySFXForUI(stageChange);

        DisplayStageUpdate(stageNum);
    }

    void DisplayStageUpdate(int _num)
    {
        StageData sd = Managers.instance.gameManager.allStageData.GetStageData(_num);
        binder.GetChild(2).GetComponent<Image>().sprite = sd.GetScreenShot();
        binder.GetChild(3).GetComponent<Image>().sprite = sd.GetMinimap();
        binder.GetChild(7).GetComponent<Text>().text = sd.GetStageName();

        if (_num == 0) { binder.GetChild(8).GetComponent<Text>().text = "??"; }
        else { binder.GetChild(8).GetComponent<Text>().text = _num.ToString("00"); }
    }

    void PressSubmit()
    {
        if (end) { return; }

        if (InputManager.GetKeyDown(BoolActions.SouthButton))
        {
            end = true;
            SoundManager.PlaySFXForUI(stageDecide);
        }
    }
    public void PressSubmitFromUI()
    {
        if (end) { return; }

        end = true;
        SoundManager.PlaySFXForUI(stageDecide);
    }
}

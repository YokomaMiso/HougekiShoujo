using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectBehavior : MonoBehaviour
{
    float timer;
    bool start;
    bool end;
    const float moveTime = 0.5f;

    readonly Vector3 startPos = new Vector3(-416, -1180);
    readonly Vector3 endPos = new Vector3(-416, -80);

    void OnEnable()
    {
        start = true;
        end = false;
        transform.GetChild(0).localPosition = startPos;
    }

    void Update()
    {
        if (start)
        {
            timer += Time.deltaTime;
            if (timer >= moveTime)
            {
                timer = moveTime;
                start = false;
            }

            float nowRate = Mathf.Sqrt(timer / moveTime);
            transform.GetChild(0).localPosition = Vector3.Lerp(startPos, endPos, nowRate);
        }
        else if (!end)
        {
            StageNumChange();
            PressSubmit();
        }
        else
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
                gameObject.SetActive(false);
            }

            float nowRate = Mathf.Sqrt(timer / moveTime);
            transform.GetChild(0).localPosition = Vector3.Lerp(startPos, endPos, nowRate);
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

            if (input > 0) { stageNum = (stageNum + 1) % stageMaxCount; }
            else { stageNum = (stageNum + (stageMaxCount - 1)) % stageMaxCount; }

            OSCManager.OSCinstance.roomData.stageNum = stageNum;

            DisplayStageUpdate(stageNum);
        }
    }

    void DisplayStageUpdate(int _num)
    {
        StageData sd= Managers.instance.gameManager.allStageData.GetStageData(_num);
        transform.GetChild(0).GetChild(2).GetComponent<Image>().sprite = sd.GetScreenShot();
        transform.GetChild(0).GetChild(3).GetComponent<Image>().sprite = sd.GetMinimap();
    }

    void PressSubmit()
    {
        if (InputManager.GetKeyDown(BoolActions.SouthButton))
        {
            end = true;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomCanvasBehavior : MonoBehaviour
{
    int charaSelectNum = 0;
    const int maxCharaCount = 3;

    [SerializeField] Text shellText;

    float timer = 0;
    const float charaChangeTimer = 0.5f;

    void Update()
    {
        float input = Input.GetAxis("Horizontal");

        if (Mathf.Abs(input) > 0.9f)
        {
            if (timer == 0)
            {
                if (input > 0) { charaSelectNum = (charaSelectNum + 1) % maxCharaCount; }
                else { charaSelectNum = (charaSelectNum + (maxCharaCount - 1)) % maxCharaCount; }
            }

            timer += Time.deltaTime;
            if (timer > charaChangeTimer) { timer = 0; }
        }
        else if (Mathf.Abs(input) < 0.2f)
        {
            timer = 0;
        }

        Managers.instance.gameManager.SetCharacterNum(charaSelectNum);
        shellText.text = Managers.instance.gameManager.playerDatas[charaSelectNum].GetShell().GetShellExplain();

        if (Input.GetButtonDown("Submit"))
        {
            GAME_STATE sendState = GAME_STATE.IN_GAME;

            Managers.instance.ChangeScene(sendState);
            Managers.instance.ChangeState(sendState);
            Destroy(gameObject);
        }
    }
}

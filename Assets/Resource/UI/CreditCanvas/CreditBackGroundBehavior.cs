using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditBackGroundBehavior : MonoBehaviour
{
    float timer;
    const float endTime = 139;
    const float lifeTime = 150;

    readonly Vector3 backStartPos = Vector3.right * 320;
    readonly Vector3 backEndPos = Vector3.left * 320;

    readonly Vector3 charaMoveValue = Vector3.right * 960;

    RawImage back;
    RawImage front;
    Animator chara;

    void Start()
    {
        back = transform.GetChild(0).GetComponent<RawImage>();
        front = transform.GetChild(1).GetComponent<RawImage>();
        chara = transform.GetChild(2).GetComponent<Animator>();

        int randomValue = Random.Range(0, Managers.instance.gameManager.playerDatas.Length);
        chara.runtimeAnimatorController = Managers.instance.gameManager.playerDatas[randomValue].GetCharacterAnimData().GetRunAnimForUI();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer <= endTime)
        {
            var frontUVRect = front.uvRect;
            frontUVRect.x = timer / 2;
            front.uvRect = frontUVRect;

            back.transform.localPosition = Vector3.Lerp(backStartPos, backEndPos, timer / lifeTime);
        }
        else
        {
            chara.transform.localPosition = charaMoveValue * (timer - endTime);
        }

        if (timer >= lifeTime)
        {
            Managers.instance.ChangeScene(GAME_STATE.TITLE);
            Managers.instance.ChangeState(GAME_STATE.TITLE);
        }
    }
}

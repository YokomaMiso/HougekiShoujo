using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelCharaForMVP : MonoBehaviour
{
    enum ANIM_ID { RUN = 0, IDLE, MAX_NUM };

    readonly Vector3 startPos = new Vector3(-1200, 860);
    readonly Vector3 endPos = new Vector3(-160, 860);

    float timer;
    const float startTime = 9.0f;
    const float arriveTime = 11.0f;

    Animator animator;
    RuntimeAnimatorController[] controllers = new RuntimeAnimatorController[(int)ANIM_ID.MAX_NUM];

    public void SetAnim(ResultScoreBoard.KDFData _kdf)
    {
        animator = transform.GetComponent<Animator>();

        PlayerData pd = Managers.instance.gameManager.playerDatas[_kdf.characterID];
        controllers[(int)ANIM_ID.RUN] = pd.GetCharacterAnimData().GetRunAnimForUI();
        controllers[(int)ANIM_ID.IDLE] = pd.GetCharacterAnimData().GetIdleAnimForUI();

        animator.runtimeAnimatorController = controllers[(int)ANIM_ID.RUN];
    }

    void Update()
    {
        if (timer >= arriveTime) { return; }

        timer += Time.deltaTime;
        if (timer >= arriveTime)
        {
            timer = arriveTime;
            animator.runtimeAnimatorController = controllers[(int)ANIM_ID.IDLE];
        }

        float nowRate = (timer - startTime) / (arriveTime - startTime);
        transform.localPosition = Vector3.Lerp(startPos, endPos, nowRate);
    }
}

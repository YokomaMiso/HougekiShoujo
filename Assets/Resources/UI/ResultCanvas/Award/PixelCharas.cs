using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PixelCharas : MonoBehaviour
{
    enum ANIM_ID { RUN = 0, IDLE, MAX_NUM };

    readonly Vector3 startPos = Vector3.left * 1800;
    readonly Vector3 endPos = Vector3.zero;

    float timer;
    const float arriveTime = 2.0f;

    Animator[] animators = new Animator[(int)AWARD_ID.MAX_NUM];
    RuntimeAnimatorController[][] controllers = new RuntimeAnimatorController[(int)AWARD_ID.MAX_NUM][]
    {
        new RuntimeAnimatorController[(int)ANIM_ID.MAX_NUM],
        new RuntimeAnimatorController[(int)ANIM_ID.MAX_NUM],
        new RuntimeAnimatorController[(int)ANIM_ID.MAX_NUM]
    };

    public void SetAnim(AWARD_ID _id, ResultScoreBoard.KDFData _kdf)
    {
        animators[(int)_id] = transform.GetChild((int)_id).GetComponent<Animator>();

        PlayerData pd = Managers.instance.gameManager.playerDatas[_kdf.characterID];
        controllers[(int)_id][(int)ANIM_ID.RUN] = pd.GetCharacterAnimData().GetRunAnimForUI();
        controllers[(int)_id][(int)ANIM_ID.IDLE] = pd.GetCharacterAnimData().GetIdleAnimForUI();

        animators[(int)_id].runtimeAnimatorController = controllers[(int)_id][(int)ANIM_ID.RUN];
    }

    void Update()
    {
        if (timer >= arriveTime) { return; }

        timer += Time.deltaTime;
        if (timer >= arriveTime)
        {
            timer = arriveTime;
            for (int i = 0; i < (int)AWARD_ID.MAX_NUM; i++) { animators[i].runtimeAnimatorController = controllers[i][(int)ANIM_ID.IDLE]; }
        }

        transform.localPosition = Vector3.Lerp(startPos, endPos, timer / arriveTime);
    }
}

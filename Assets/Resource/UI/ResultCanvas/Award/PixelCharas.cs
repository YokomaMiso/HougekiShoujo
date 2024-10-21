using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelCharas : MonoBehaviour
{
    readonly Vector3 startPos = Vector3.left * 1800;
    readonly Vector3 endPos = Vector3.zero;

    float timer;
    const float arriveTime = 3.0f;

    Animator[] animators = new Animator[(int)AWARD_ID.MAX_NUM];

    public void SetAnim(AWARD_ID _id, ResultScoreBoard.KDFData _kdf)
    {
        animators[(int)_id] = transform.GetChild((int)_id).GetComponent<Animator>();

        PlayerData pd = Managers.instance.gameManager.playerDatas[_kdf.characterID];
        animators[(int)_id].runtimeAnimatorController = pd.GetCharacterAnimData().GetIdleAnimForUI();
    }

    void Update()
    {
        if (timer >= arriveTime) { return; }

        timer += Time.deltaTime;
        if (timer >= arriveTime) { timer = arriveTime; }

        transform.localPosition = Vector3.Lerp(startPos, endPos, timer / arriveTime);
    }
}

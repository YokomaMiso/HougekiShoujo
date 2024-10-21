using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelCharaForMVP : MonoBehaviour
{
    readonly Vector3 startPos = new Vector3(-1200, 860);
    readonly Vector3 endPos = new Vector3(-160,860);

    float timer;
    const float startTime = 9.0f;
    const float arriveTime = 11.0f;

    Animator animator;

    public void SetAnim(ResultScoreBoard.KDFData _kdf)
    {
        animator = transform.GetComponent<Animator>();

        PlayerData pd = Managers.instance.gameManager.playerDatas[_kdf.characterID];
        animator.runtimeAnimatorController = pd.GetCharacterAnimData().GetIdleAnimForUI();
    }

    void Update()
    {
        if (timer >= arriveTime) { return; }

        timer += Time.deltaTime;
        if (timer >= arriveTime) { timer = arriveTime; }

        float nowRate = (timer - startTime) / (arriveTime - startTime);
        transform.localPosition = Vector3.Lerp(startPos, endPos, nowRate);
    }
}

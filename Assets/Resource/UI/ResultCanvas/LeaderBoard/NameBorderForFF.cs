using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameBorderForFF : NameBorder
{
    public override void SetData(ResultScoreBoard.KDFData _kdf, PlayerData _pd)
    {
        //�v���C���[��
        transform.GetChild(0).GetComponent<Text>().text = _kdf.playerName;
        //�L����
        killCountText = transform.GetChild(3).GetComponent<Text>();
        killCountText.text = _kdf.friendlyFireCount.ToString();
        killCountText.color = Color.clear;
        killCountText.transform.localScale = Vector3.one * 5;
        //�L���J�E���g�̃A�E�g���C���𖳌���
        outlines = killCountText.GetComponents<Outline>();
        for (int i = 0; i < outlines.Length; i++) { outlines[i].enabled = false; }

        if (transform.childCount > 4)
        {
            charaIdleAnim = transform.GetChild(4).GetComponent<Animator>();
            charaIdleAnim.runtimeAnimatorController = _pd.GetCharacterAnimData().GetIdleAnimForUI();
        }
        transform.localPosition = startPos;
    }
}

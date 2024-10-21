using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AWARD_ID { JUNKY, VICTIM, DANGER, MAX_NUM };
public class AwardCanvas : MonoBehaviour
{
    ResultCanvasBehavior resultCanvasBehavior;
    public void SetResultCanvas(ResultCanvasBehavior _owner) 
    {
        resultCanvasBehavior = _owner;

        charaIllusts.SetSprite(AWARD_ID.JUNKY, resultCanvasBehavior.GetJunky());
        charaIllusts.SetSprite(AWARD_ID.VICTIM, resultCanvasBehavior.GetVictim());
        charaIllusts.SetSprite(AWARD_ID.DANGER, resultCanvasBehavior.GetDanger());

        charaTexts.SetText(AWARD_ID.JUNKY, resultCanvasBehavior.GetJunky());
        charaTexts.SetText(AWARD_ID.VICTIM, resultCanvasBehavior.GetVictim());
        charaTexts.SetText(AWARD_ID.DANGER, resultCanvasBehavior.GetDanger());

        pixelCharas.SetAnim(AWARD_ID.JUNKY, resultCanvasBehavior.GetJunky());
        pixelCharas.SetAnim(AWARD_ID.VICTIM, resultCanvasBehavior.GetVictim());
        pixelCharas.SetAnim(AWARD_ID.DANGER, resultCanvasBehavior.GetDanger());

        playerName.SetText(AWARD_ID.JUNKY, resultCanvasBehavior.GetJunky());
        playerName.SetText(AWARD_ID.VICTIM, resultCanvasBehavior.GetVictim());
        playerName.SetText(AWARD_ID.DANGER, resultCanvasBehavior.GetDanger());

        awardCount.SetText(AWARD_ID.JUNKY, resultCanvasBehavior.GetJunky());
        awardCount.SetText(AWARD_ID.VICTIM, resultCanvasBehavior.GetVictim());
        awardCount.SetText(AWARD_ID.DANGER, resultCanvasBehavior.GetDanger());
    }

    [SerializeField] CharaIllusts charaIllusts;
    [SerializeField] CharaTexts charaTexts;
    [SerializeField] PixelCharas pixelCharas;
    [SerializeField] DisplayPlayerNameInAward playerName;
    [SerializeField] AwardCount awardCount;

    void Update()
    {

    }
}

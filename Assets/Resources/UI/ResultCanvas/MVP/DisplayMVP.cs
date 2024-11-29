using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayMVP : MonoBehaviour
{
    [SerializeField] CharaIllustForMVP charaIllust;
    [SerializeField] CharaTextForMVP charaText;
    [SerializeField] DisplayPlayerNameForMVP displayPlayerName;
    [SerializeField] KillCountForMVP killCountText;
    [SerializeField] PixelCharaForMVP pixelChara;
    [SerializeField] MVPTextSpawn mvpText;
    [SerializeField] Text mvpExplain;

    ResultCanvasBehavior resultCanvasBehavior;
    public void SetResultCanvas(ResultCanvasBehavior _owner)
    {
        resultCanvasBehavior = _owner;

        ResultScoreBoard.KDFData mvpData = resultCanvasBehavior.GetMVP();

        charaIllust.SetSprite(mvpData);
        charaText.SetText(mvpData);
        displayPlayerName.SetText(mvpData);
        killCountText.SetText(mvpData);
        pixelChara.SetAnim(mvpData);
        mvpExplain.text = Managers.instance.gameManager.playerDatas[mvpData.characterID].GetMVPExplain();
    }
}

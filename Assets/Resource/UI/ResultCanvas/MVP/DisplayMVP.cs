using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayMVP : MonoBehaviour
{
    [SerializeField] CharaIllustForMVP charaIllust;
    [SerializeField] CharaTextForMVP charaText;
    [SerializeField] DisplayPlayerNameForMVP displayPlayerName;
    [SerializeField] KillCountForMVP killCountText;
    [SerializeField] PixelCharaForMVP pixelChara;
    [SerializeField] MVPTextSpawn mvpText;

    ResultCanvasBehavior resultCanvasBehavior;
    public void SetResultCanvas(ResultCanvasBehavior _owner)
    {
        resultCanvasBehavior = _owner;

        charaIllust.SetSprite(resultCanvasBehavior.GetMVP());
        charaText.SetText(resultCanvasBehavior.GetMVP());
        displayPlayerName.SetText(resultCanvasBehavior.GetMVP());
        killCountText.SetText(resultCanvasBehavior.GetMVP());
        pixelChara.SetAnim(resultCanvasBehavior.GetMVP());
    }
}

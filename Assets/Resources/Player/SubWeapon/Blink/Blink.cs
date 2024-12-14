using System.Collections;
using UnityEngine;

public class Blink : MonoBehaviour
{
    const int shadowNum = 8;
    AudioClip blinkSFX;
    public void SetSFX(AudioClip _clip) { blinkSFX = _clip; }

    void Start()
    {
        BlinkShadowBehavior.timeSub = 1.0f / shadowNum;

        Player player = GetComponent<Player>();
        if (!player) { Destroy(this); return; }

        //長さ
        float length = 5;
        //プレイヤーの座標値
        Vector3 playerPos = transform.position + Vector3.up * 0.5f;
        //プレイヤーの入力
        Vector3 targetVec = player.GetInputVector();
        //ワープする座標
        Vector3 warpPos = playerPos + targetVec * length;

        bool isHit = false;

        //ワープ座標に壁があるかどうか球体のレイを作る
        RaycastHit[] sphereHits;
        sphereHits = Physics.SphereCastAll(warpPos + Vector3.up * 0.5f, 0.5f, targetVec, 0.01f);
        for (int i = 0; i < sphereHits.Length; i++)
        {
            if (sphereHits[i].collider.tag == "Ground") { isHit = true; break; }
        }

        if (isHit)
        {
            //プレイヤーから飛ばすレイ
            RaycastHit[] hits;
            hits = Physics.RaycastAll(playerPos, targetVec, length);

            //Groundタグと当たっていたら
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.tag == "Ground")
                {
                    //ワープ座標を変更
                    warpPos = hits[i].point - targetVec;
                    break;
                }
            }
        }

        //残像を生成
        float posRate = 1.0f / shadowNum;
        float posRateSub = posRate;
        GameObject imageRef = player.GetPlayerImageObject();
        for (int i = 0; i < shadowNum; i++)
        {
            Vector3 pos = Vector3.Lerp(warpPos, transform.position + Vector3.up * 0.5f, posRate + posRateSub * i);
            GameObject obj = Instantiate(imageRef, pos, Quaternion.identity);
            Destroy(obj.GetComponent<Animator>());
            BlinkShadowBehavior bsb = obj.AddComponent<BlinkShadowBehavior>();
            bsb.SetTime(i);
            bsb.SetTeamNum(OSCManager.OSCinstance.GetRoomData(player.GetPlayerID()).myTeamNum);

            obj.transform.localScale = new Vector3(player.NowDirection() * 2, 2, 2);
        }
        /*
        {
            Vector3 pos = transform.position + Vector3.up * 0.5f;
            GameObject obj = Instantiate(imageRef, pos, Quaternion.identity);
            Destroy(obj.GetComponent<Animator>());
            obj.AddComponent<BlinkShadowBehavior>();

            obj.transform.localScale = new Vector3(player.NowDirection() * 2, 2, 2);
        }
        */

        transform.position = warpPos - Vector3.up * 0.5f;

        //自分の時は移動前の座標
        GameObject so; 
        if (player.IsMine())
        {
            so = SoundManager.PlaySFX(blinkSFX,player.transform);
        }
        //他人の時は移動後の座標
        else
        {
            so = SoundManager.PlaySFX(blinkSFX);
            so.transform.position = transform.position;
        }

        Destroy(this);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Blink : MonoBehaviour
{
    void Start()
    {
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
        sphereHits = Physics.SphereCastAll(warpPos, 0.5f, targetVec, 0.1f);
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

        transform.position = warpPos - Vector3.up * 0.5f;
        Destroy(this);
    }
}

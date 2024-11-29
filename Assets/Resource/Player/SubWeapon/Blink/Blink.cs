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

        //����
        float length = 5;
        //�v���C���[�̍��W�l
        Vector3 playerPos = transform.position + Vector3.up * 0.5f;
        //�v���C���[�̓���
        Vector3 targetVec = player.GetInputVector();
        //���[�v������W
        Vector3 warpPos = playerPos + targetVec * length;

        bool isHit = false;

        //���[�v���W�ɕǂ����邩�ǂ������̂̃��C�����
        RaycastHit[] sphereHits;
        sphereHits = Physics.SphereCastAll(warpPos, 0.5f, targetVec, 0.1f);
        for (int i = 0; i < sphereHits.Length; i++)
        {
            if (sphereHits[i].collider.tag == "Ground") { isHit = true; break; }
        }

        if (isHit)
        {
            //�v���C���[�����΂����C
            RaycastHit[] hits;
            hits = Physics.RaycastAll(playerPos, targetVec, length);

            //Ground�^�O�Ɠ������Ă�����
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.tag == "Ground")
                {
                    //���[�v���W��ύX
                    warpPos = hits[i].point - targetVec;
                    break;
                }
            }
        }

        transform.position = warpPos - Vector3.up * 0.5f;
        Destroy(this);
    }
}

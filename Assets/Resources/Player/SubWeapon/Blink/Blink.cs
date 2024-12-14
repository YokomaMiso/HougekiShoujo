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
        sphereHits = Physics.SphereCastAll(warpPos + Vector3.up * 0.5f, 0.5f, targetVec, 0.01f);
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

        //�c���𐶐�
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

        //�����̎��͈ړ��O�̍��W
        GameObject so; 
        if (player.IsMine())
        {
            so = SoundManager.PlaySFX(blinkSFX,player.transform);
        }
        //���l�̎��͈ړ���̍��W
        else
        {
            so = SoundManager.PlaySFX(blinkSFX);
            so.transform.position = transform.position;
        }

        Destroy(this);
    }
}

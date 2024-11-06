using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailGunProjectileBehavior : ProjectileBehavior
{
    [SerializeField] Transform damageArea;
    [SerializeField] RailGunBulletBehavior trailObject;

    public override void SetData(Shell _data)
    {
        lifeTime = _data.GetLifeTime();
        speed = _data.GetSpeed();

        if (_data.GetShellType() != SHELL_TYPE.BLAST) { SoundManager.PlaySFX(ownerPlayer.GetPlayerData().GetPlayerSFXData().GetFlySFX(), transform); }

        damageArea.GetComponent<RailGunDamageArea>().SetPlayer(ownerPlayer);
    }

    protected override void Start() { }

    public override void SetAngle(float _angle)
    {
        angle = _angle;
        damageArea.rotation = Quaternion.Euler(0, -angle+90, 0);

        //����
        float length = 100;
        //�v���C���[�̍��W�l
        Vector3 playerPos = transform.position + Vector3.up * 0.5f;
        //�v���C���[�̓���
        Vector3 targetVec = damageArea.forward;

        //�I�[�̍��W
        Vector3 endPos = playerPos + targetVec * length;

        //�v���C���[�����΂����C
        RaycastHit[] hits;
        hits = Physics.RaycastAll(playerPos, targetVec, length);

        //Ground�^�O�Ɠ������Ă�����
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.tag == "Ground")
            {
                //�I�[���W��ύX
                endPos = hits[i].point;
                break;
            }
        }

        damageArea.position = (playerPos + endPos) / 2;
        Vector3 scale = damageArea.localScale;
        float distance = (endPos - playerPos).magnitude;
        damageArea.localScale = new Vector3(scale.x, scale.y, scale.z * distance);

        trailObject.transform.position = playerPos;
        trailObject.SetForward(damageArea.forward, 400);
    }

    protected override void Update()
    {
        float deltaTime = Managers.instance.timeManager.GetDeltaTime();
        timer += deltaTime;
        if (timer > lifeTime) { Destroy(gameObject); }
    }

    protected override void SpawnExplosion() { }

    protected override void OnTriggerEnter(Collider other) { }
}

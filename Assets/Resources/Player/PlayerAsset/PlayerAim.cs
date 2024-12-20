using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    Player ownerPlayer;

    GameObject attackArea;
    GameObject aoeArea;

    [SerializeField] Material[] attackAreaMat;

    Vector3 aimVector = Vector3.zero;
    Shell shellData;

    public void Init()
    {
        aimVector = Vector3.zero;
        if (ownerPlayer.IsMine())
        {
            attackArea.SetActive(false);
            aoeArea.SetActive(false);
        }
    }

    public void SetPlayer(Player _player, GameObject _aoeArea, GameObject _attackArea)
    {
        ownerPlayer = _player;
        shellData = ownerPlayer.GetPlayerData().GetShell();

        if (ownerPlayer.IsMine())
        {
            attackArea = _attackArea;
            float aimRange = shellData.GetAimRange();
            attackArea.transform.localScale = new Vector3(aimRange, aimRange, 1);
            attackArea.GetComponent<MeshRenderer>().sortingOrder = 1;
            attackArea.SetActive(false);

            aoeArea = _aoeArea;
            float explosionRadius = shellData.GetExplosion().GetBody().GetComponent<SphereCollider>().radius;
            float explosionScale = shellData.GetExplosion().GetScale();
            float explosionRange = explosionRadius * 2 * explosionScale;
            aoeArea.transform.localScale = new Vector3(explosionRange, explosionRange, 1);
            aoeArea.GetComponent<MeshRenderer>().sortingOrder = 2;
            aoeArea.SetActive(false);
        }
    }
    public void AimStart()
    {
        if (!ownerPlayer.IsMine()) { return; }

        aimVector = ownerPlayer.GetInputVector();
        if (aimVector == Vector3.zero) { aimVector = Vector3.right * ownerPlayer.NowDirection(); }

        switch (shellData.GetShellType())
        {
            case SHELL_TYPE.BLAST:
                attackArea.SetActive(true);
                attackAreaMat[0].SetFloat("_Degree", shellData.GetAOEDegree());
                attackArea.GetComponent<MeshRenderer>().material = attackAreaMat[0];
                break;
            case SHELL_TYPE.CANON:
                attackArea.SetActive(true);
                attackArea.GetComponent<MeshRenderer>().material = attackAreaMat[1];
                aoeArea.SetActive(true);
                aoeArea.GetComponent<MeshRenderer>().material.SetFloat("_Degree", 360);
                break;
            case SHELL_TYPE.MORTAR:
                attackArea.SetActive(true);
                attackAreaMat[2].SetFloat("_Degree", 360);
                attackAreaMat[2].SetColor("_baseColor", Color.cyan * 0.5f);
                attackArea.GetComponent<MeshRenderer>().material = attackAreaMat[2];
                aoeArea.SetActive(true);
                aoeArea.GetComponent<MeshRenderer>().material.SetFloat("_Degree", 360);
                break;
        }

        GameObject aimSFX = SoundManager.PlaySFX(ownerPlayer.GetPlayerData().GetPlayerSFXData().GetAimSFX(), transform);
        Camera.main.GetComponent<CameraMove>().SetCameraFar(shellData.GetAimRange() / 2);

        if (!ownerPlayer.IsMine())
        {
            aimSFX.GetComponent<AudioSource>().volume *= 0.5f;
            aimSFX.AddComponent<AudioLowPassFilter>();
        }
    }

    public Vector3 AimMove()
    {
        if (ownerPlayer.IsMine())
        {
            Vector3 movement = Vector3.zero;
            movement += Vector3.right * InputManager.GetAxis<Vector2>(Vec2AxisActions.LStickAxis).x;
            movement += Vector3.forward * InputManager.GetAxis<Vector2>(Vec2AxisActions.LStickAxis).y;
            if (movement.magnitude >= 1) { movement = movement.normalized; }

            if (movement != Vector3.zero)
            {
                switch (shellData.GetShellType())
                {
                    case SHELL_TYPE.BLAST:
                        aimVector = movement;
                        break;

                    case SHELL_TYPE.CANON:
                        aimVector = movement;
                        break;

                    case SHELL_TYPE.MORTAR:
                        float limit = attackArea.transform.localScale.x / 2;
                        aimVector += movement * Managers.instance.GetOptionData().mortarSensitive * Managers.instance.timeManager.GetDeltaTime();
                        if (aimVector.magnitude >= limit) { aimVector = aimVector.normalized * limit; }
                        break;
                }
            }

            switch (shellData.GetShellType())
            {
                case SHELL_TYPE.BLAST:
                    attackAreaMat[0].SetFloat("_Direction", Mathf.Atan2(aimVector.x, aimVector.z) * Mathf.Rad2Deg);
                    break;

                case SHELL_TYPE.CANON:
                    attackAreaMat[1].SetFloat("_Direction", Mathf.Atan2(aimVector.x, aimVector.z) * Mathf.Rad2Deg);
                    float aimRange = shellData.GetAimRange();
                    aoeArea.transform.position = attackArea.transform.position + aimVector.normalized * aimRange / 2;
                    break;

                case SHELL_TYPE.MORTAR:
                    aoeArea.transform.position = attackArea.transform.position + aimVector;
                    break;
            }

            OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData.playerStickValue = aimVector;
        }
        else
        {
            aimVector = OSCManager.OSCinstance.GetIngameData(ownerPlayer.GetPlayerID()).mainPacketData.inGameData.playerStickValue;
        }

        return aimVector;
    }

    public void Fire(Vector3 _scale)
    {
        if (!ownerPlayer.IsMine()) { aimVector = OSCManager.OSCinstance.GetIngameData(ownerPlayer.GetPlayerID()).mainPacketData.inGameData.playerStickValue; }

        GameObject projectile = shellData.GetProjectile();
        GameObject obj;
        float angle;
        Vector3 spawnPos;

        Vector3 applyPos = aimVector.normalized;
        if (applyPos == Vector3.zero) { applyPos = Vector3.forward; }

        switch (shellData.GetShellType())
        {
            default: //SHELL_TYPE.BLAST
                angle = Mathf.Atan2(aimVector.x, aimVector.z) * Mathf.Rad2Deg;
                const float blastDistance = 1.5f;
                spawnPos = transform.position + applyPos * blastDistance;
                obj = Instantiate(projectile, spawnPos, Quaternion.Euler(0, angle, 0));
                break;

            case SHELL_TYPE.CANON:
                angle = Mathf.Atan2(applyPos.x, applyPos.z) * Mathf.Rad2Deg;
                spawnPos = transform.position + aimVector.normalized;
                obj = Instantiate(projectile, spawnPos, Quaternion.Euler(0, angle, 0));
                angle = Mathf.Atan2(applyPos.z, applyPos.x) * Mathf.Rad2Deg;
                obj.GetComponent<ProjectileBehavior>().SetAngle(angle);
                break;

            case SHELL_TYPE.MORTAR:
                spawnPos = transform.position + (aimVector.normalized * 0.5f);
                obj = Instantiate(projectile, spawnPos, Quaternion.identity);
                obj.transform.GetChild(0).localScale = _scale;
                obj.GetComponent<ProjectileBehavior>().ProjectileStart(transform.position + aimVector);
                break;
        }

        GameObject fireSFX = SoundManager.PlaySFX(ownerPlayer.GetPlayerData().GetPlayerSFXData().GetFireSFX(), transform);
        ownerPlayer.PlayVoice(ownerPlayer.GetPlayerData().GetPlayerVoiceData().GetUseMain());
        obj.GetComponent<ProjectileBehavior>().SetPlayer(ownerPlayer);
        obj.GetComponent<ProjectileBehavior>().SetData(ownerPlayer.GetPlayerData().GetShell());

        if (ownerPlayer.IsMine())
        {
            OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData.fire = !OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData.fire;
            OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData.fireCount++;
            OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData.playerStickValue = aimVector;

            ResetAim();
        }
        else
        {
            fireSFX.GetComponent<AudioSource>().volume *= 0.5f;
            fireSFX.AddComponent<AudioLowPassFilter>();
        }

        ownerPlayer.ChangeShellIconColor(0);
    }

    public void ResetAim()
    {
        aimVector = Vector3.zero;
        attackAreaMat[0].SetFloat("_Direction", 0);
        attackAreaMat[1].SetFloat("_Direction", 0);
        aoeArea.transform.localPosition = Vector3.zero;

        attackArea.SetActive(false);
        aoeArea.SetActive(false);
        Camera.main.GetComponent<CameraMove>().ResetCameraFar();
    }
}

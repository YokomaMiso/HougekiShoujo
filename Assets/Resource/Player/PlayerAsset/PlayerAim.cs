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

    public void SetPlayer(Player _player, GameObject _aoeArea, GameObject _attackArea)
    {
        ownerPlayer = _player;
        shellData = ownerPlayer.GetPlayerData().GetShell();

        attackArea = _attackArea;
        float aimRange = shellData.GetAimRange();
        attackArea.transform.localScale = new Vector3(aimRange, aimRange, 1);
        attackArea.GetComponent<MeshRenderer>().sortingOrder = 1;
        attackArea.SetActive(false);

        aoeArea = _aoeArea;
        float explosionRadius = shellData.GetExplosion().GetComponent<SphereCollider>().radius;
        float explosionScale = shellData.GetExplosion().transform.localScale.x;
        float explosionRange = explosionRadius * 2 * explosionScale;
        aoeArea.transform.localScale = new Vector3(explosionRange, explosionRange, 1);
        aoeArea.GetComponent<MeshRenderer>().sortingOrder = 2;
        aoeArea.SetActive(false);
    }
    public void AimStart()
    {
        switch (shellData.GetShellType())
        {
            case SHELL_TYPE.BLAST:
                attackArea.SetActive(true);
                attackArea.GetComponent<MeshRenderer>().material = attackAreaMat[0];
                break;
            case SHELL_TYPE.CANON:
                attackArea.SetActive(true);
                attackArea.GetComponent<MeshRenderer>().material = attackAreaMat[1];
                break;
            case SHELL_TYPE.MORTAR:
                attackArea.SetActive(true);
                attackArea.GetComponent<MeshRenderer>().material = attackAreaMat[2];
                aoeArea.SetActive(true);
                break;
        }

        Camera.main.GetComponent<CameraMove>().SetCameraFar(shellData.GetAimRange() / 2);
    }

    public Vector3 AimMove()
    {
        Vector3 movement = Vector3.zero;
        movement += Vector3.right * Input.GetAxis("Horizontal");
        movement += Vector3.forward * Input.GetAxis("Vertical");

        if (movement != Vector3.zero)
        {
            switch (shellData.GetShellType())
            {
                case SHELL_TYPE.BLAST:
                    attackAreaMat[0].SetFloat("_Direction", Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg);
                    aimVector = movement;
                    break;

                case SHELL_TYPE.CANON:
                    attackAreaMat[1].SetFloat("_Direction", Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg);
                    aimVector = movement;
                    break;

                case SHELL_TYPE.MORTAR:
                    float limit = attackArea.transform.localScale.x / 2;
                    aimVector += movement * Managers.instance.GetOptionData().mortarSensitive * Managers.instance.timeManager.GetDeltaTime();
                    if (aimVector.magnitude >= limit) { aimVector = aimVector.normalized * limit; }
                    aoeArea.transform.position = attackArea.transform.position + aimVector;
                    break;
            }
        }
        return aimVector;
    }

    public void Fire(Vector3 _scale)
    {
        GameObject projectile = shellData.GetProjectile();
        GameObject obj;
        float angle;

        switch (shellData.GetShellType())
        {
            default: //SHELL_TYPE.BLAST
                angle = Mathf.Atan2(aimVector.x, aimVector.z) * Mathf.Rad2Deg;
                const float blastDistance = 1.5f;
                Vector3 applyPos = aimVector.normalized;
                if (applyPos == Vector3.zero) { applyPos = Vector3.forward; }
                obj = Instantiate(projectile, transform.position + applyPos * blastDistance + Vector3.up, Quaternion.Euler(0, angle, 0));
                obj.GetComponent<ExplosionBehavior>().SetPlayer(ownerPlayer);
                break;

            case SHELL_TYPE.CANON:
                angle = Mathf.Atan2(aimVector.x, aimVector.z) * Mathf.Rad2Deg;
                obj = Instantiate(projectile, transform.position + Vector3.up, Quaternion.Euler(0, angle, 0));
                angle = Mathf.Atan2(aimVector.z, aimVector.x) * Mathf.Rad2Deg;
                obj.GetComponent<CanonProjectileBehavior>().SetAngle(angle);
                obj.GetComponent<CanonProjectileBehavior>().SetPlayer(ownerPlayer);
                break;

            case SHELL_TYPE.MORTAR:
                Vector3 spawnPos = transform.position + Vector3.up + (aimVector.normalized * 0.5f);
                obj = Instantiate(projectile, spawnPos, Quaternion.identity);
                obj.transform.GetChild(0).localScale = _scale;
                obj.GetComponent<MortarProjectileBehavior>().ProjectileStart(transform.position + aimVector);
                obj.GetComponent<MortarProjectileBehavior>().SetPlayer(ownerPlayer);
                break;
        }

        Camera.main.GetComponent<CameraMove>().ResetCameraFar();
    }

    void Update()
    {
        if (ownerPlayer.playerState != PLAYER_STATE.AIMING)
        {
            aimVector = Vector3.zero;
            attackAreaMat[0].SetFloat("_Direction", 0);
            attackAreaMat[1].SetFloat("_Direction", 0);
            aoeArea.transform.localPosition = Vector3.zero;

            attackArea.SetActive(false);
            aoeArea.SetActive(false);
            return;
        }
    }
}

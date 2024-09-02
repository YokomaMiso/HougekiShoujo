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
    Shell nowShell;
    SHELL_TYPE nowShellType;
    public void SetPlayer(Player _player, GameObject _aoeArea, GameObject _attackArea)
    {
        ownerPlayer = _player;

        aoeArea = _aoeArea;
        attackArea = _attackArea;

        aoeArea.GetComponent<MeshRenderer>().sortingOrder = 1;
        attackArea.SetActive(false);
        aoeArea.SetActive(false);
    }
    public void AimStart(Shell _shell)
    {
        nowShell = _shell;
        nowShellType = nowShell.GetShellType();

        switch (nowShellType)
        {
            case SHELL_TYPE.BLAST:
                attackArea.SetActive(true);
                attackArea.GetComponent<MeshRenderer>().material = attackAreaMat[0];
                break;
            case SHELL_TYPE.CANON:
                attackArea.SetActive(true);
                attackArea.GetComponent<MeshRenderer>().material = attackAreaMat[1];
                break;
            case SHELL_TYPE.MORTOR:
                attackArea.SetActive(true);
                attackArea.GetComponent<MeshRenderer>().material = attackAreaMat[2];
                aoeArea.SetActive(true);
                break;
        }
    }

    public Vector3 AimMove()
    {
        Vector3 movement = Vector3.zero;
        movement += Vector3.right * Input.GetAxis("Horizontal");
        movement += Vector3.forward * Input.GetAxis("Vertical");

        if (movement != Vector3.zero)
        {
            switch (nowShellType)
            {
                case SHELL_TYPE.BLAST:
                    attackAreaMat[0].SetFloat("_Direction", Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg);
                    aimVector = movement;
                    break;

                case SHELL_TYPE.CANON:
                    attackAreaMat[1].SetFloat("_Direction", Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg);
                    aimVector = movement;
                    break;

                case SHELL_TYPE.MORTOR:
                    float limit = attackArea.transform.localScale.x / 2;
                    aimVector += movement * 5 * TimeManager.deltaTime;
                    if (aimVector.magnitude >= limit) { aimVector = aimVector.normalized * limit; }
                    aoeArea.transform.position = attackArea.transform.position + aimVector;
                    break;
            }
        }
        return aimVector;
    }

    public void Fire(Vector3 _scale)
    {
        GameObject projectile = nowShell.GetProjectile();
        GameObject obj;
        float angle;
        OwnerID id;

        switch (nowShellType)
        {
            case SHELL_TYPE.BLAST:

                break;

            case SHELL_TYPE.CANON:
                angle = Mathf.Atan2(aimVector.x, aimVector.z) * Mathf.Rad2Deg;
                obj = Instantiate(projectile, transform.position + Vector3.up, Quaternion.Euler(0, angle, 0));
                angle = Mathf.Atan2(aimVector.z, aimVector.x) * Mathf.Rad2Deg;
                obj.GetComponent<CanonProjectileBehavior>().SetAngle(angle);

                id = obj.AddComponent<OwnerID>();
                id.SetID(ownerPlayer.GetPlayerID());
                break;

            case SHELL_TYPE.MORTOR:
                Vector3 spawnPos = transform.position + Vector3.up + (aimVector.normalized * 0.5f);
                obj = Instantiate(projectile, spawnPos, Quaternion.identity);
                obj.transform.GetChild(0).localScale = _scale;
                obj.GetComponent<MortarProjectileBehavior>().ProjectileStart(transform.position + aimVector);

                id = obj.AddComponent<OwnerID>();
                id.SetID(ownerPlayer.GetPlayerID());
                break;
        }


        

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailGunDamageArea : MonoBehaviour
{
    protected Player ownerPlayer;
    public void SetPlayer(Player _player) { ownerPlayer = _player; }

    private void OnTriggerStay(Collider other)
    {
        //���������I�u�W�F�N�g����Player�^���擾
        Player player = other.GetComponent<Player>();

        //Player�^�łȂ���Α������^�[��
        if (!player) { return; }

        //�����̃L�����N�^�[����Ȃ���Α������^�[��
        if (player && player.GetPlayerID() != Managers.instance.playerID) { return; }

        //�����̃L�����N�^�[�̎��S������s��
        other.GetComponent<Player>().SetDead(ownerPlayer.GetPlayerID());
    }
}

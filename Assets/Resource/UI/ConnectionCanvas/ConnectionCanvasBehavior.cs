using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionCanvasBehavior : MonoBehaviour
{
    [SerializeField]
    Text stateText;

    void Update()
    {
        //�n���h�V�F�C�N���I�����Ă���΃��[���V�[���ֈڍs����
        if(OSCManager.OSCinstance.GetIsFinishedHandshake())
        {
            stateText.text = "���[���V�[���ֈڍs���܂�";

            Managers.instance.ChangeScene(GAME_STATE.ROOM);
            Managers.instance.ChangeState(GAME_STATE.ROOM);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionCanvasBehavior : MonoBehaviour
{
    [SerializeField]
    Text stateText;

    private void Start()
    {
        stateText = GetComponentInChildren<Text>();

        OSCManager.OSCinstance.CreateTempNet();
    }

    void Update()
    {
        //�n���h�V�F�C�N���I�����Ă���΃��[���V�[���ֈڍs����
        if(OSCManager.OSCinstance.GetIsFinishedHandshake())
        {
            if (Managers.instance.playerID != 0)
            {
                stateText.text = "�����𔭌����܂����A�Q�����܂�";
            }
            else
            {
                stateText.text = "�����������������ߐV�����쐬���܂�";
            }

            Invoke("MoveToRoomScene", 2.0f);
        }
    }

    private void MoveToRoomScene()
    {
        Managers.instance.ChangeScene(GAME_STATE.ROOM);
        Managers.instance.ChangeState(GAME_STATE.ROOM);

        return;
    }
}

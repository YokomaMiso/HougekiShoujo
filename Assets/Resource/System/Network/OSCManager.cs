using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OscCore;

public class OSCManager : MonoBehaviour
{
    //////////////////////////////
    //////// �{�Ԏg�p�ϐ� ////////
    //////////////////////////////

    List<SendDataCreator.PlayerNetData> netData = new List<SendDataCreator.PlayerNetData>();

    //1�t���[���O�̑S�v���C���[�f�[�^�i�[�p
    List<SendDataCreator.PlayerNetData> beforeNetData = new List<SendDataCreator.PlayerNetData>();


    ///////// OSCcore���� ////////

    OscClient client;
    OscServer server;


    ////////////////////////////////
    //////// �f�o�b�N�p�ϐ� ////////
    ////////////////////////////////

    //���t���[���̑S�v���C���[�f�[�^�i�[�p
    //List<Player> nowFramePlayerData = new List<Player>();

    //////////////////////
    //////// �֐� ////////
    //////////////////////

    // Start is called before the first frame update
    void Start()
    {
        //netData[0].mainPacketData
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

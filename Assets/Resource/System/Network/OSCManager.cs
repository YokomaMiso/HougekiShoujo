using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OscCore;

public class OSCManager : MonoBehaviour
{
    //////////////////////////////
    //////// 本番使用変数 ////////
    //////////////////////////////

    List<SendDataCreator.PlayerNetData> netData = new List<SendDataCreator.PlayerNetData>();

    //1フレーム前の全プレイヤーデータ格納用
    List<SendDataCreator.PlayerNetData> beforeNetData = new List<SendDataCreator.PlayerNetData>();


    ///////// OSCcore周り ////////

    OscClient client;
    OscServer server;


    ////////////////////////////////
    //////// デバック用変数 ////////
    ////////////////////////////////

    //現フレームの全プレイヤーデータ格納用
    //List<Player> nowFramePlayerData = new List<Player>();

    //////////////////////
    //////// 関数 ////////
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

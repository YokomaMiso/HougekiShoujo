using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    [SerializeField] GameObject postProcess;

    void Start()
    {
        Managers.instance.gameManager.CreatePlayer();

        postProcess.SetActive(OSCManager.OSCinstance.GetRoomData(0).stageNum == 3);

        Destroy(gameObject);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class Managers : MonoBehaviour
{
    int playerID;
    int characterID = 0;

    [SerializeField] GameObject[] playerPrefab;
    [SerializeField] GameObject otherPlayerPrefab;
    GameObject[] playerInstance;
    const int playerMaxNum = 2;

    //仮座標
    Vector3[] pos = new Vector3[2] { Vector3.forward * 3, Vector3.left * 3 };

    public OptionData optionData;

    public GameManager gameManager;
    public SaveManager saveManager;
    public TimeManager timeManager;

    void Start()
    {
        gameManager = GetComponent<GameManager>();
        gameManager.SetManagerMaster(this);
        saveManager = GetComponent<SaveManager>();
        saveManager.SetManagerMaster(this);
        timeManager = GetComponent<TimeManager>();
        timeManager.SetManagerMaster(this);

        saveManager.LoadOptionData();

        playerInstance = new GameObject[playerMaxNum];
        //仮のプレイヤー生成処理
        for (int i = 0; i < playerMaxNum; i++)
        {
            if (i == 0) { playerInstance[i] = Instantiate(playerPrefab[characterID], pos[i], Quaternion.identity); }
            else { playerInstance[i] = Instantiate(otherPlayerPrefab, pos[i], Quaternion.identity); }

            playerInstance[i].GetComponent<Player>().SetManagerMaster(this);
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject otherPlayerPrefab;
    GameObject[] playerInstance;
    const int playerMaxNum = 2;

    [SerializeField] PlayerData[] playerDatas;

    //仮座標
    Vector3[] pos = new Vector3[2] { Vector3.forward * 3, Vector3.left * 3 };
    public void CreatePlayer()
    {
        playerInstance = new GameObject[playerMaxNum];
        //仮のプレイヤー生成処理
        for (int i = 0; i < playerMaxNum; i++)
        {
            if (i == Managers.instance.playerID)
            {
                playerInstance[i] = Instantiate(playerPrefab, pos[i], Quaternion.identity);
                Camera.main.GetComponent<CameraMove>().SetPlayer(playerInstance[i].GetComponent<Player>());
            }
            else { playerInstance[i] = Instantiate(otherPlayerPrefab, pos[i], Quaternion.identity); }

            Player nowPlayer = playerInstance[i].GetComponent<Player>();
            nowPlayer.SetPlayerID(i);
            nowPlayer.SetPlayerData(playerDatas[0]);
        }
    }

    public GameObject GetPlayer(int _num)
    {
        if (playerInstance == null) { return null; }
        if (_num >= playerInstance.Length) { return null; }

        return playerInstance[_num];
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == (int)GAME_STATE.IN_GAME)
        {
            for (int i = 0; i < playerMaxNum; i++)
            {
                if (!playerInstance[i].GetComponent<Player>().GetAlive())
                {
                    Managers.instance.ChangeScene(GAME_STATE.ROOM);
                    Managers.instance.ChangeState(GAME_STATE.ROOM);
                }
        }
        }
    }
}

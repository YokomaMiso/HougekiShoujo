using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;


public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject[] playerPrefab;
    [SerializeField] GameObject otherPlayerPrefab;
    GameObject[] playerInstance;
    const int playerMaxNum = 2;

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
                playerInstance[i] = Instantiate(playerPrefab[0], pos[i], Quaternion.identity);
                Camera.main.GetComponent<CameraMove>().SetPlayer(playerInstance[i].GetComponent<Player>());
            }
            else { playerInstance[i] = Instantiate(otherPlayerPrefab, pos[i], Quaternion.identity); }
        }
    }

    public GameObject GetPlayer(int _num)
    {
        if (playerInstance == null) { return null; }
        if(_num >= playerInstance.Length) { return null; }

        return playerInstance[_num];
    }

}

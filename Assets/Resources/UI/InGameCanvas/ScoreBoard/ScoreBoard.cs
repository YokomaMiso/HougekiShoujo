using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField] GameObject personalScorePrefab;
    [SerializeField] GameObject itemAnnouncePrefab;

    [SerializeField] Sprite[] scoreBG;

    GameObject[] scoreInstance = new GameObject[6];

    readonly int baseHeight = 240;
    readonly int heightSub = 140;

    int[] scoreNums = new int[6] { -1, -1, -1, -1, -1, -1 };

    void Start()
    {
        int[] teamCount = new int[2] { 0, 0 };
        for (int i = 0; i < 6; i++)
        {
            //ルームデータの取得
            MachingRoomData.RoomData oscRoomData = OSCManager.OSCinstance.GetRoomData(i);
            scoreInstance[i] = Instantiate(personalScorePrefab, transform);

            if (oscRoomData.myTeamNum == MachingRoomData.bannerEmpty)
            {
                transform.GetChild(i).gameObject.SetActive(false);
                continue;
            }

            //インゲームデータの取得
            IngameData.GameData gameData = OSCManager.OSCinstance.GetIngameData(i).mainPacketData.inGameData;

            //表示順の登録
            scoreNums[i] = oscRoomData.myTeamNum * 3 + teamCount[oscRoomData.myTeamNum];
            teamCount[oscRoomData.myTeamNum]++;

            //背景の色変更
            scoreInstance[i].transform.GetChild(0).GetComponent<Image>().sprite = scoreBG[oscRoomData.myTeamNum];

            //アイコンの変更
            Sprite icon = Managers.instance.gameManager.playerDatas[oscRoomData.selectedCharacterID].GetCharacterAnimData().GetCharaIcon();
            scoreInstance[i].transform.GetChild(1).GetComponent<Image>().sprite = icon;

            //名前の変更
            scoreInstance[i].transform.GetChild(2).GetComponent<Text>().text = oscRoomData.playerName;

            //キル数
            scoreInstance[i].transform.GetChild(3).GetComponent<Text>().text = gameData.killCount.ToString();

            //デス数
            scoreInstance[i].transform.GetChild(4).GetComponent<Text>().text = gameData.deathCount.ToString();

            //FF数
            scoreInstance[i].transform.GetChild(5).GetComponent<Text>().text = gameData.friendlyFireCount.ToString();

            //座標の変更
            scoreInstance[i].transform.localPosition = Vector3.up * (baseHeight - heightSub * scoreNums[i]);
        }

        GameObject announce = Instantiate(itemAnnouncePrefab, transform);
        announce.transform.localPosition = Vector3.up * (baseHeight + heightSub);
    }
}

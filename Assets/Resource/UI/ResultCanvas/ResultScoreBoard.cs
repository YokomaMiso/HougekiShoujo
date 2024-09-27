using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultScoreBoard : MonoBehaviour
{
    [SerializeField] GameObject personalScorePrefab;
    [SerializeField] GameObject itemAnnouncePrefab;

    GameObject[] scoreInstance = new GameObject[6];

    readonly Color[] BGColor = new Color[2];

    readonly int[] teamPosX = new int[2] { -480, 480 };
    readonly int baseHeight = 140;
    readonly int heightSub = 140;

    int[] scoreNums = new int[6] { -1, -1, -1, -1, -1, -1 };

    void Start()
    {
        BGColor[0] = ColorCordToRGB("#2050E4");
        BGColor[1] = ColorCordToRGB("#ff1f1f");

        int[] teamCount = new int[2] { 0, 0 };
        for (int i = 0; i < 6; i++)
        {
            //���[���f�[�^�̎擾
            MachingRoomData.RoomData oscRoomData = OSCManager.OSCinstance.GetRoomData(i);
            scoreInstance[i] = Instantiate(personalScorePrefab, transform);

            if (oscRoomData.myTeamNum == MachingRoomData.bannerEmpty)
            {
                transform.GetChild(i).gameObject.SetActive(false);
                continue;
            }

            //�C���Q�[���f�[�^�̎擾
            IngameData.GameData gameData = OSCManager.OSCinstance.GetIngameData(i).mainPacketData.inGameData;

            //�w�i�̐F�ύX
            scoreInstance[i].transform.GetChild(0).GetComponent<Image>().color = BGColor[oscRoomData.myTeamNum];

            //�A�C�R���̕ύX
            Sprite icon = Managers.instance.gameManager.playerDatas[oscRoomData.selectedCharacterID].GetCharacterAnimData().GetCharaIcon();
            scoreInstance[i].transform.GetChild(1).GetComponent<Image>().sprite = icon;

            //���O�̕ύX
            scoreInstance[i].transform.GetChild(2).GetComponent<Text>().text = "Player " + (oscRoomData.myID + 1).ToString();

            //�L����
            scoreInstance[i].transform.GetChild(3).GetComponent<Text>().text = gameData.killCount.ToString();

            //�f�X��
            scoreInstance[i].transform.GetChild(4).GetComponent<Text>().text = gameData.deathCount.ToString();

            //FF��
            scoreInstance[i].transform.GetChild(5).GetComponent<Text>().text = gameData.friendlyFireCount.ToString();

            //���W�̕ύX
            scoreInstance[i].transform.localPosition = new Vector3(teamPosX[i], baseHeight - heightSub * teamCount[oscRoomData.myTeamNum]);

            //�\�����̓o�^
            scoreNums[i] = oscRoomData.myTeamNum * 3 + teamCount[oscRoomData.myTeamNum];
            teamCount[oscRoomData.myTeamNum]++;
        }

        for (int i = 0; i < 2; i++)
        {
            GameObject announce = Instantiate(itemAnnouncePrefab, transform);
            announce.transform.localPosition = new Vector3(teamPosX[i], baseHeight + heightSub);
        }
    }

    Color ColorCordToRGB(string hex)
    {
        if (ColorUtility.TryParseHtmlString(hex, out Color color)) return color;
        else return Color.black;
    }
}

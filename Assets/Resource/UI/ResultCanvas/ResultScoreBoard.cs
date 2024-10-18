using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultScoreBoard : MonoBehaviour
{
    [SerializeField] GameObject personalScorePrefab;
    [SerializeField] GameObject itemAnnouncePrefab;

    GameObject[] scoreInstance = new GameObject[6];

    readonly int[] teamPosX = new int[2] { -480, 480 };
    readonly int baseHeight = 140;
    readonly int heightSub = 140;

    int[] scoreNums = new int[6] { -1, -1, -1, -1, -1, -1 };

    bool moveToCenter;
    public bool arriveToCenter;
    float moveTimer;
    const float moveTime = 0.75f;
    readonly Vector3 startPos = Vector3.right * 1920;
    readonly Vector3 endPos = Vector3.zero;
    public void MoveToCenter() { moveToCenter = true; }

    int winner;

    public struct KDFData
    {
        public string playerName;
        public int playerID;
        public int characterID;
        public int killCount;
        public int deathCount;
        public int friendlyFireCount;

        public KDFData(int _id)
        {
            playerName = "";
            playerID = _id;
            characterID = 0;
            killCount = 0;
            deathCount = 0;
            friendlyFireCount = 0;
        }
    }

    KDFData[][] kdfDatas;

    public void Init()
    {
        IngameData.GameData hostIngameData;
        if (Managers.instance.playerID == 0) { hostIngameData = OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData; }
        else { hostIngameData = OSCManager.OSCinstance.GetIngameData(0).mainPacketData.inGameData; }
        winner = hostIngameData.winner;

        kdfDatas = new KDFData[2][];
        kdfDatas[(int)TEAM_NUM.A] = new KDFData[3];
        kdfDatas[(int)TEAM_NUM.B] = new KDFData[3];
        for (int i = 0; i < kdfDatas.Length; i++)
        {
            for (int j = 0; j < kdfDatas[i].Length; j++) { kdfDatas[i][j].playerID = -1; }
        }

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
            scoreInstance[i].transform.GetChild(0).GetComponent<Image>().color = Managers.instance.ColorCordToRGB(oscRoomData.myTeamNum);

            //�A�C�R���̕ύX
            Sprite icon = Managers.instance.gameManager.playerDatas[oscRoomData.selectedCharacterID].GetCharacterAnimData().GetCharaIcon();
            scoreInstance[i].transform.GetChild(1).GetComponent<Image>().sprite = icon;

            //���O�̕ύX
            scoreInstance[i].transform.GetChild(2).GetComponent<Text>().text = oscRoomData.playerName;

            //�L����
            scoreInstance[i].transform.GetChild(3).GetComponent<Text>().text = gameData.killCount.ToString();

            //�f�X��
            scoreInstance[i].transform.GetChild(4).GetComponent<Text>().text = gameData.deathCount.ToString();

            //FF��
            scoreInstance[i].transform.GetChild(5).GetComponent<Text>().text = gameData.friendlyFireCount.ToString();

            //���W�̕ύX
            scoreInstance[i].transform.localPosition = new Vector3(teamPosX[oscRoomData.myTeamNum], baseHeight - heightSub * teamCount[oscRoomData.myTeamNum]);

            //KDF�f�[�^�̕ۑ�
            kdfDatas[oscRoomData.myTeamNum][teamCount[oscRoomData.myTeamNum]].playerName = oscRoomData.playerName;
            kdfDatas[oscRoomData.myTeamNum][teamCount[oscRoomData.myTeamNum]].playerID = oscRoomData.myID;
            kdfDatas[oscRoomData.myTeamNum][teamCount[oscRoomData.myTeamNum]].characterID = oscRoomData.selectedCharacterID;
            kdfDatas[oscRoomData.myTeamNum][teamCount[oscRoomData.myTeamNum]].killCount = gameData.killCount;
            kdfDatas[oscRoomData.myTeamNum][teamCount[oscRoomData.myTeamNum]].deathCount = gameData.deathCount;
            kdfDatas[oscRoomData.myTeamNum][teamCount[oscRoomData.myTeamNum]].friendlyFireCount = gameData.friendlyFireCount;

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

    void Update()
    {
        if (arriveToCenter) { return; }
        if (!moveToCenter) { return; }

        moveTimer += Time.deltaTime;

        if (moveTimer > moveTime)
        {
            moveTimer = moveTime;
            arriveToCenter = true;
        }

        float nowRate = Mathf.Sqrt(moveTimer / moveTime);
        transform.localPosition = Vector3.Lerp(startPos, endPos, nowRate);
    }

    public KDFData GetMVPKDF()
    {
        KDFData returnKDFData = kdfDatas[winner][0];
        for (int i = 1; i < kdfDatas[winner].Length; i++)
        {
            if (kdfDatas[winner][i].playerID == -1) { continue; }

            //�L�����������Ă���ꍇ
            if (returnKDFData.killCount < kdfDatas[winner][i].killCount) { returnKDFData = kdfDatas[winner][i]; continue; }
            //�L�����������ꍇ
            else if (returnKDFData.killCount == kdfDatas[winner][i].killCount)
            {
                //�f�X����������Ă���ꍇ
                if (returnKDFData.deathCount > kdfDatas[winner][i].deathCount) { returnKDFData = kdfDatas[winner][i]; continue; }
                //�f�X���������ꍇ
                else if (returnKDFData.deathCount == kdfDatas[winner][i].deathCount)
                {
                    //FF����������Ă���ꍇ
                    if (returnKDFData.friendlyFireCount > kdfDatas[winner][i].friendlyFireCount) { returnKDFData = kdfDatas[winner][i]; continue; };
                }
            }
        }

        return returnKDFData;
    }

    public KDFData GetDeadRankerKDF()
    {
        KDFData returnKDFData = kdfDatas[0][0];
        for (int i = 0; i < kdfDatas.Length; i++)
        {
            for (int j = 0; j < kdfDatas[i].Length; j++)
            {
                if (kdfDatas[i][j].playerID == -1) { continue; }

                //�f�X���������Ă���ꍇ
                if (returnKDFData.deathCount < kdfDatas[i][j].deathCount) { returnKDFData = kdfDatas[i][j]; continue; }
                //�f�X���������ꍇ
                else if (returnKDFData.deathCount == kdfDatas[i][j].deathCount)
                {
                    //�L������������Ă���ꍇ
                    if (returnKDFData.killCount > kdfDatas[i][j].killCount) { returnKDFData = kdfDatas[i][j]; continue; }
                    //�L�����������ꍇ
                    else if (returnKDFData.killCount == kdfDatas[i][j].killCount)
                    {
                        //FF����������Ă���ꍇ
                        if (returnKDFData.friendlyFireCount > kdfDatas[i][j].friendlyFireCount) { returnKDFData = kdfDatas[i][j]; continue; };
                    }
                }
            }
        }

        return returnKDFData;
    }

    public KDFData GetCriminalKDF()
    {
        KDFData returnKDFData = kdfDatas[0][0];
        for (int i = 0; i < kdfDatas.Length; i++)
        {
            for (int j = 0; j < kdfDatas[i].Length; j++)
            {
                if (kdfDatas[i][j].playerID == -1) { continue; }

                //FF���������Ă���ꍇ
                if (returnKDFData.friendlyFireCount < kdfDatas[i][j].friendlyFireCount) { returnKDFData = kdfDatas[i][j]; continue; }
                //FF���������ꍇ
                else if (returnKDFData.friendlyFireCount == kdfDatas[i][j].friendlyFireCount)
                {
                    //�L������������Ă���ꍇ
                    if (returnKDFData.deathCount > kdfDatas[i][j].deathCount) { returnKDFData = kdfDatas[i][j]; continue; }
                    //�L�����������ꍇ
                    else if (returnKDFData.deathCount == kdfDatas[i][j].deathCount)
                    {
                        //�L������������Ă���ꍇ
                        if (returnKDFData.killCount > kdfDatas[i][j].killCount) { returnKDFData = kdfDatas[i][j]; continue; };
                    }
                }
            }
        }

        return returnKDFData;
    }

}

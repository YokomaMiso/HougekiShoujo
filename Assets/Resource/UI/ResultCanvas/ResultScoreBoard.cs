using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultScoreBoard : MonoBehaviour
{
    [SerializeField] GameObject personalScorePrefab;

    GameObject[] scoreInstance = new GameObject[6];
    readonly Vector3 scoreBasePos = Vector3.down * 620;
    readonly Vector3 scorePosSub = Vector3.down * 80;

    readonly int[] teamPosX = new int[2] { -480, 480 };
    readonly int baseHeight = 140;
    readonly int heightSub = 140;

    int[] scoreNums = new int[6] { -1, -1, -1, -1, -1, -1 };

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

            if (oscRoomData.myTeamNum == MachingRoomData.bannerEmpty) { continue; }

            //�C���Q�[���f�[�^�̎擾
            IngameData.GameData gameData = OSCManager.OSCinstance.GetIngameData(i).mainPacketData.inGameData;

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

        for (int i = 0; i < 6; i++)
        {
            if (scoreNums[i] == -1) { continue; }
            KDFData nowData = kdfDatas[scoreNums[i] / 3][scoreNums[i] % 3];

            scoreInstance[scoreNums[i]] = Instantiate(personalScorePrefab, transform);
            scoreInstance[scoreNums[i]].transform.localPosition = scoreBasePos + scorePosSub * scoreNums[i];
            scoreInstance[scoreNums[i]].GetComponent<PersonalData>().SetData(nowData, scoreNums[i] % 2);
        }
    }

    void Update()
    {

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

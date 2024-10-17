using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AWARD_ID { DEATH = 0, FF, MAX_NUM };

public class DisplayOtherAward : MonoBehaviour
{
    ResultScoreBoard.KDFData[] kdfDatas = new ResultScoreBoard.KDFData[(int)AWARD_ID.MAX_NUM] { new ResultScoreBoard.KDFData(-1), new ResultScoreBoard.KDFData(-1) };
    PlayerData[] awardCharacter = new PlayerData[(int)AWARD_ID.MAX_NUM];

    [SerializeField] GameObject cautionTapePrefab;
    GameObject cautionTapeInstance;
    [SerializeField] GameObject keepoutTapePrefab;
    GameObject keepoutTapeInstance;

    [SerializeField] GameObject charaFramePrefab;
    GameObject[] charaFrameInstance = new GameObject[(int)AWARD_ID.MAX_NUM];
    int charaFrameIndex;

    [SerializeField] GameObject[] nameBorderPrefab;

    [SerializeField] Vector3[] charaFrameStartPos;
    [SerializeField] Vector3[] charaFrameEndPos;

    float timer;
    const float cautionSpawnTime = 0.5f;
    const float keepoutSpawnTime = 1.0f;
    readonly float[] charaFrameSpawnTime = new float[(int)AWARD_ID.MAX_NUM] { 1.25f, 1.75f };

    const float tapeFadeTime = 7.5f;
    const float lifeTime = 8.0f;

    public void SetKDFData(ResultScoreBoard.KDFData _kdf, AWARD_ID _id)
    {
        int num = (int)_id;

        //KDFデータを適用
        kdfDatas[num] = _kdf;
        //キャラデータの参照
        awardCharacter[num] = Managers.instance.gameManager.playerDatas[_kdf.characterID];
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        TapeSpawn();
        CharaFrameSpawn();

        if (timer > tapeFadeTime)
        {
            cautionTapeInstance.GetComponent<CautionTapeBehavior>().SetDestroy();
            keepoutTapeInstance.GetComponent<CautionTapeBehavior>().SetDestroy();
        }

        if (timer > lifeTime) { Destroy(gameObject); }
    }

    void TapeSpawn()
    {
        //最後のオブジェクト生成が終わったら
        if (keepoutTapeInstance != null) { return; }

        if (cautionTapeInstance == null)
        {
            if (timer > cautionSpawnTime) { cautionTapeInstance = Instantiate(cautionTapePrefab, transform); }
        }
        if (keepoutTapeInstance == null)
        {
            if (timer > keepoutSpawnTime) { keepoutTapeInstance = Instantiate(keepoutTapePrefab, transform); }
        }
    }

    void CharaFrameSpawn()
    {
        if (charaFrameIndex >= (int)AWARD_ID.MAX_NUM) { return; }

        if (timer > charaFrameSpawnTime[charaFrameIndex])
        {
            charaFrameInstance[charaFrameIndex] = Instantiate(charaFramePrefab, transform);
            Sprite charaIllust = awardCharacter[charaFrameIndex].GetCharacterAnimData().GetCharaIllust();
            string playerName = kdfDatas[charaFrameIndex].playerName;
            charaFrameInstance[charaFrameIndex].GetComponent<CharaFrame>().SetData(charaIllust, playerName);
            charaFrameInstance[charaFrameIndex].AddComponent<CharaFrameInAward>().SetPos(charaFrameStartPos[charaFrameIndex], charaFrameEndPos[charaFrameIndex]);
            charaFrameInstance[charaFrameIndex].GetComponent<CharaFrameInAward>().SpawnNameBorder(nameBorderPrefab[charaFrameIndex], kdfDatas[charaFrameIndex], awardCharacter[charaFrameIndex]);

            charaFrameIndex++;
        }
    }
}

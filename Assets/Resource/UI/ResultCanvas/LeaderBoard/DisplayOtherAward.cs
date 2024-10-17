using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayOtherAward : MonoBehaviour
{
    ResultScoreBoard.KDFData[] kdfDatas = new ResultScoreBoard.KDFData[3] { new ResultScoreBoard.KDFData(-1), new ResultScoreBoard.KDFData(-1), new ResultScoreBoard.KDFData(-1) };
    PlayerData[] awardCharacter = new PlayerData[3];

    [SerializeField] GameObject cautionTapePrefab;
    GameObject cautionTapeInstance;
    [SerializeField] GameObject keepoutTapePrefab;
    GameObject keepoutTapeInstance;

    float timer;
    const float cautionSpawnTime = 0.5f;
    const float keepoutSpawnTime = 1.0f;

    public void SetKDFData(ResultScoreBoard.KDFData _kdf, int _num)
    {
        //配列外を参照したらリターン
        if (kdfDatas.Length >= _num) { return; }

        //KDFデータを適用
        kdfDatas[_num] = _kdf;
        //キャラデータの参照
        awardCharacter[_num] = Managers.instance.gameManager.playerDatas[_kdf.characterID];
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        TapeSpawn();
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
}

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
        //�z��O���Q�Ƃ����烊�^�[��
        if (kdfDatas.Length >= _num) { return; }

        //KDF�f�[�^��K�p
        kdfDatas[_num] = _kdf;
        //�L�����f�[�^�̎Q��
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
        //�Ō�̃I�u�W�F�N�g�������I�������
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayMVP : MonoBehaviour
{
    ResultScoreBoard.KDFData mvpData;
    PlayerData mvpCharacter;

    [SerializeField] GameObject cautionTapePrefab;
    GameObject cautionTapeInstance;
    [SerializeField] GameObject keepoutTapePrefab;
    GameObject keepoutTapeInstance;
    [SerializeField] GameObject mvpIllustPrefab;
    MVPIllust mvpIllust;

    float timer;
    const float cautionSpawnTime = 0.5f;
    const float keepoutSpawnTime = 1.0f;

    public void SetMVPKDFData(ResultScoreBoard.KDFData _kdf)
    {
        //MVP�f�[�^��K�p
        mvpData = _kdf;
        //�L�����f�[�^�̎Q��
        mvpCharacter = Managers.instance.gameManager.playerDatas[_kdf.characterID];

        //mvp�C���X�g�̐���
        GameObject mvpIllustInstance = Instantiate(mvpIllustPrefab, transform);
        mvpIllust = mvpIllustInstance.GetComponent<MVPIllust>();
        mvpIllust.SetData(mvpCharacter);
    }
    void Update()
    {
        TapeSpawn();
        MVPIllustControll();
    }

    void MVPIllustControll()
    {
        if (!mvpIllust.GetArrive()) { return; }
       
        if (mvpIllust.GetTurnBackComplete()) { Destroy(gameObject); }
        else if (Input.GetButtonDown("Submit")) 
        {
            mvpIllust.SetTurnBack();
            cautionTapeInstance.GetComponent<CautionTapeBehavior>().SetDestroy();
            keepoutTapeInstance.GetComponent<CautionTapeBehavior>().SetDestroy();
        }
    }
    void TapeSpawn()
    {
        //�Ō�̃I�u�W�F�N�g�������I�������
        if (keepoutTapeInstance != null) { return; }

        timer += Time.deltaTime;
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

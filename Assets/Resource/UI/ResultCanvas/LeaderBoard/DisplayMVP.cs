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

    const float illustSpawnTime = 1.5f;

    public void SetMVPKDFData(ResultScoreBoard.KDFData _kdf)
    {
        //MVP�f�[�^��K�p
        mvpData = _kdf;
        //�L�����f�[�^�̎Q��
        mvpCharacter = Managers.instance.gameManager.playerDatas[_kdf.characterID];

        
    }
    void Update()
    {
        if (timer <= illustSpawnTime) { timer += Time.deltaTime; }

        TapeSpawn();
        IllustSpawn();
        MVPIllustControll();
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
    void IllustSpawn()
    {
        if (timer < illustSpawnTime) { return; }
        if (mvpIllust != null) { return; }

        //mvp�C���X�g�̐���
        GameObject mvpIllustInstance = Instantiate(mvpIllustPrefab, transform);
        mvpIllust = mvpIllustInstance.GetComponent<MVPIllust>();
        mvpIllust.SetData(mvpData,mvpCharacter);
    }
    void MVPIllustControll()
    {
        if (mvpIllust == null) { return; }

        if (!mvpIllust.GetArrive()) { return; }

        if (mvpIllust.GetTurnBackComplete()) { Destroy(gameObject); }
        else if (Input.GetButtonDown("Submit"))
        {
            mvpIllust.SetTurnBack();
            cautionTapeInstance.GetComponent<CautionTapeBehavior>().SetDestroy();
            keepoutTapeInstance.GetComponent<CautionTapeBehavior>().SetDestroy();
        }
    }
}

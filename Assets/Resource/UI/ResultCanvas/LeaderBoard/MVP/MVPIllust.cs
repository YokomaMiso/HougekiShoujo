using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MVPIllust : MonoBehaviour
{
    Vector3 startPos = Vector3.left * 1920;
    Vector3 endPos = Vector3.left * 160;

    Vector3 dropShadowPos = new Vector3(32, -16, 0);
    float dropShadowTimer;
    const float dropShadowTime = 0.25f;
    bool drowShadowArrive;

    Image[] charaIllusts = new Image[2];

    float timer;
    const float arriveTime = 0.75f;
    bool arrive;
    public bool GetArrive() { return arrive; }

    float turnBackTimer;
    const float turnBackTime = 0.5f;
    bool turnBack;
    bool turnBackArrive;
    public void SetTurnBack() { turnBack = true; }
    public bool GetTurnBackComplete() { return turnBackArrive; }

    ResultScoreBoard.KDFData kdf;
    PlayerData pd;
    [SerializeField] GameObject nameBorderPrefab;

    public void SetData(ResultScoreBoard.KDFData _kdf, PlayerData _pd)
    {
        pd = _pd;
        kdf = _kdf;

        //初期座標
        transform.localPosition = startPos;
        //イラストを適用するImageコンポーネントの参照
        for (int i = 0; i < 2; i++) { charaIllusts[i] = transform.GetChild(i).GetComponent<Image>(); }

        //キャライラストの適用
        Sprite illust = _pd.GetCharacterAnimData().GetCharaIllust();
        for (int i = 0; i < 2; i++) { charaIllusts[i].sprite = illust; }

    }
    void Update()
    {
        if (turnBack && !turnBackArrive)
        {
            turnBackTimer += Time.deltaTime;
            if (turnBackTimer >= turnBackTime)
            {
                turnBackTimer = turnBackTime;
                turnBackArrive = true;
            }
            float nowRate = Mathf.Pow(turnBackTimer / turnBackTime, 2);
            transform.localPosition = Vector3.Lerp(endPos, startPos, nowRate);
        }
        else if (!arrive)
        {
            timer += Time.deltaTime;
            if (timer >= arriveTime)
            {
                timer = arriveTime;
                arrive = true;
                GameObject obj = Instantiate(nameBorderPrefab, transform);
                obj.GetComponent<NameBorder>().SetData(kdf, pd);
            }
            float nowRate = Mathf.Sqrt(timer / arriveTime);
            transform.localPosition = Vector3.Lerp(startPos, endPos, nowRate);
        }

        if (arrive)
        {
            if (!drowShadowArrive)
            {
                dropShadowTimer += Time.deltaTime;
                if (dropShadowTimer >= dropShadowTime)
                {
                    dropShadowTimer = dropShadowTime;
                    drowShadowArrive = true;
                }
                float nowRate = Mathf.Sqrt(dropShadowTimer / dropShadowTime);
                charaIllusts[0].transform.localPosition = dropShadowPos * nowRate;
            }
        }
    }
}

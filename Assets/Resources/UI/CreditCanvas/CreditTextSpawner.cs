using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditTextSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] credits;
    bool spawned;
    int index;

    float timer;
    const float spawnStartTime = 5;
    float lifeTime;

    float spawnSpan;
    const float creditLifeTimeSub = 1.0f;

    void Start() 
    {
        lifeTime = spawnStartTime + 6.1f * credits.Length;
        spawnSpan = (lifeTime - spawnStartTime) / credits.Length; 
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer < spawnStartTime) { return; }
        if (timer > spawnStartTime + spawnSpan * index)
        {
            if (!spawned)
            {
                GameObject obj = Instantiate(credits[index], transform);
                if (index < credits.Length - 1)
                {
                    obj.AddComponent<CreditTextDestroyer>().SetLifeTime(spawnSpan - creditLifeTimeSub);
                    index++;
                }
                else
                {
                    spawned = true;
                }
            }
        }

        if (Managers.instance.UsingCanvas()) { return; }
        if (InputManager.GetKeyDown(BoolActions.SouthButton))
        {
            Managers.instance.ChangeScene(GAME_STATE.TITLE);
            Managers.instance.ChangeState(GAME_STATE.TITLE);
        }
    }
}

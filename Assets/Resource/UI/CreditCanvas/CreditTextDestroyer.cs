using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditTextDestroyer : MonoBehaviour
{
    float timer;
    float lifeTime;

    public void SetLifeTime(float _time) { lifeTime = _time; }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > lifeTime) { Destroy(gameObject); }
    }
}

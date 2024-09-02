using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBehavior : MonoBehaviour
{
    [SerializeField] Animator imageAnimator;

    float lifeTime;
    float timer;

    void Start()
    {
        imageAnimator.speed = 1;
        lifeTime = imageAnimator.GetCurrentAnimatorStateInfo(0).length - 0.75f;

        Vector3 distance = transform.position - Camera.main.transform.position;
        float weight = distance.magnitude / 5;
        if (weight < 1) { weight = 1; }

        float shakeValue = 1.0f / weight;
        Camera.main.GetComponent<CameraMove>().SetCameraShake(shakeValue);

    }

    void Update()
    {
        imageAnimator.speed = 2 * TimeManager.TimeRate();

        timer += TimeManager.deltaTime;
        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}

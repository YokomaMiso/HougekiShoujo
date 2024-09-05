using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBehavior : MonoBehaviour
{
    Player ownerPlayer;
    public void SetPlayer(Player _player) { ownerPlayer = _player; }

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
        imageAnimator.speed = 2 * Managers.instance.timeManager.TimeRate();

        timer += Managers.instance.timeManager.GetDeltaTime();
        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}

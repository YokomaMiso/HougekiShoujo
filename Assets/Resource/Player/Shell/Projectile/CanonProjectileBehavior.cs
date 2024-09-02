using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonProjectileBehavior : MonoBehaviour
{
    [SerializeField] GameObject explosion;
    [SerializeField] Animator imageAnimator;
    [SerializeField] RuntimeAnimatorController animatorController;
    int animNum = 0;

    [SerializeField] float speed = 10;
    [SerializeField] float lifeTime = 2;

    float angle = 0;
    float timer = 0;

    void Start()
    {
        Vector3 forwardVector = transform.forward;
        imageAnimator.runtimeAnimatorController = animatorController;
    }

    void Update()
    {
        imageAnimator.speed = 1 * TimeManager.TimeRate();

        timer += TimeManager.deltaTime;
        if (timer > lifeTime) { Destroy(gameObject); }

        transform.position += transform.forward * speed * TimeManager.deltaTime;
    }

    public void SetAngle(float _angle)
    {
        angle = _angle;
        imageAnimator.GetComponent<ObjectBillboard>().FixedAngles = Vector3.forward * angle;
    }

    void OnDestroy()
    {
        Vector3 spawnPos = transform.position;
        spawnPos.y = 2;
        Instantiate(explosion, spawnPos, Quaternion.identity);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentBehavior : ProjectileBehavior
{
    public GameObject explosionFlag;

    public void SetExplosionData(GameObject _object)
    {
        explosionFlag = _object;
    }

    public void SetExpData(Explosion _explosion)
    {
        explosion = _explosion;
    }

    public void SetOwnPlayer(Player _player)
    {
        ownerPlayer = _player;
    }

    protected override void Start()
    {
        base.Start();

        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.useGravity = true;

            float randomForce = Random.Range(1f, 3f);
            Vector3 randomDirection = Random.insideUnitSphere;
            randomDirection.y = 0; 

            rb.AddForce(randomDirection * randomForce, ForceMode.Impulse);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.tag == groundTag||other.tag== playerTag)
        {
            SpawnExplosion();
            Destroy(gameObject);
        }
    }

    protected override void Update()
    {

    }

    protected override void TimeSetting()
    {

    }

    void SpawnExplosion()
    {
        Vector3 explosionPos = transform.position;
        GameObject explosionInstance = explosionFlag;
        GameObject obj = Instantiate(explosionInstance, explosionPos, Quaternion.identity);
        obj.GetComponent<ExplosionBehavior>().SetPlayer(ownerPlayer);
        obj.GetComponent<ExplosionBehavior>().SetData(explosion);
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBehavior : MonoBehaviour
{
    protected Player ownerPlayer;
    public void SetPlayer(Player _player) { ownerPlayer = _player; }

    protected float timer = 0;
    [SerializeField] protected Explosion explosion;

    protected string playerTag = "Player";
    protected string groundTag = "Ground";

    public virtual void LaunchBomb(Vector3 _vector)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        Vector3 addVector = _vector + Vector3.up * 0.5f;
        rb.AddForce(addVector, ForceMode.Impulse);
    }
    protected virtual void Update()
    {
        float deltaTime = Managers.instance.timeManager.GetDeltaTime();
        timer += deltaTime;
        transform.GetChild(0).GetComponent<ObjectBillboard>().FixedAngles = Vector3.forward * (timer * 720);
    }

    protected virtual void SpawnExplosion()
    {
        Vector3 spawnPos = transform.position;
        spawnPos.y = 0;
        GameObject obj = Instantiate(explosion.GetBody(), spawnPos, Quaternion.identity);
        obj.GetComponent<ExplosionBehavior>().SetPlayer(ownerPlayer);
        obj.GetComponent<ExplosionBehavior>().SetData(explosion);
        obj.AddComponent<RoundCheckerSubWeapon>();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == playerTag || other.tag == groundTag)
        {
            SpawnExplosion();
            Destroy(gameObject);
        }
    }

}

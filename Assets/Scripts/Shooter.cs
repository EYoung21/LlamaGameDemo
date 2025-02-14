using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    Player player;
    public Transform projectileSpawn;

    public float cooldown;
    Action attackMotion;

    Coroutine attackRoutine;

    public void Passify()
    {
        StopCoroutine(attackRoutine);
    }

    void Start()
    {
        player = FindFirstObjectByType<Player>();
        attackRoutine = StartCoroutine(AttackBehaviour());
    }

    public void Init(ShooterAttack attack)
    {
        switch (attack)
        {
            case ShooterAttack.Solo:
                attackMotion = new Action(ShootOne);
                break;
            case ShooterAttack.Tripple:
                attackMotion = new Action(ShootTripple);
                break;
            case ShooterAttack.Circle:
                attackMotion = new Action(ShootCircle);
                break;
            default:
                attackMotion = new Action(ShootOne);
                break;
        }
    }

    void ShootOne()
    {
        Vector3 trajectory = player.transform.position - transform.position;
        trajectory.Normalize();
        SpawnProjectile(trajectory);
    }

    void ShootTripple()
    {
        Vector3 trajectory1 = player.transform.position - transform.position;
        trajectory1.Normalize();
        Vector3 trajectory2 = Quaternion.AngleAxis(10, Vector3.forward) * trajectory1;
        Vector3 trajectory3 = Quaternion.AngleAxis(-10, Vector3.forward) * trajectory1;
        Vector3[] trajectories = new Vector3[] { trajectory1, trajectory2, trajectory3 };

        foreach(Vector3 trajectory in trajectories)
        {
            SpawnProjectile(trajectory);
        }
    }

    void ShootCircle()
    {
        int count = 36;
        int c = 0;
        while (c < count)
        {
            Vector3 trajectory = Vector3.right;
            trajectory.x = Mathf.Sin(((float)c / count) * Mathf.PI * 2f);
            trajectory.y = Mathf.Cos(((float)c / count) * Mathf.PI * 2f);
            SpawnProjectile(trajectory);
            c++;
        }
    }

    void SpawnProjectile(Vector3 trajectory)
    {
        GameObject spitObject = BulletObjectPool.RequestBullet();
        Projectile projectile = spitObject.GetComponent<Projectile>();
        projectile.transform.position = projectileSpawn.transform.position;
        projectile.trajectory = trajectory;
        projectile.transform.right = trajectory;
    }

    IEnumerator AttackBehaviour()
    {
        while (true)
        {
            yield return new WaitForSeconds(cooldown);
            attackMotion();
            StartCoroutine(AnimateShooting(cooldown / 4f));
        }
    }

    IEnumerator AnimateShooting(float cooldown)
    {
        Vector3 scaling = transform.localScale;
        float timer = 0;
        float normalizedTime = 0;
        while (normalizedTime < 1)
        {
            timer += Time.deltaTime;
            normalizedTime = timer / cooldown;
            float factor = Mathf.Sin(normalizedTime * Mathf.PI) / 3f;
            transform.localScale = scaling + new Vector3(factor, factor, factor);
            yield return null;
        }
        transform.localScale = scaling;
    }
}

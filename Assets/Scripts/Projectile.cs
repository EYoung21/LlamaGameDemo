using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public Vector3 trajectory;
    public float speed = 5;
    public float lifeTime = 4;

    void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(DestroyAfterSeconds(lifeTime));
    }

    void Update()
    {
        transform.position += trajectory * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            player.Hurt(this);
            Die();
        }

        Llama llama = other.GetComponent<Llama>();
        if (llama != null && llama.friendly)
        {
            llama.Hurt(this);
            Die();
        }
    }

    IEnumerator DestroyAfterSeconds(float seconds){
        yield return new WaitForSeconds(lifeTime);
        Die();
    }

    void Die()
    {
        StopAllCoroutines();
        BulletObjectPool.ReturnBullet(gameObject);
    }
}

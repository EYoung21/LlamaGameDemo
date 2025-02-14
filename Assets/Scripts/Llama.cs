using UnityEngine;
using System.Collections;
using TMPro;

public class Llama : MonoBehaviour
{
    public GameObject heartAnimationPrefab;
    GameObject heartAnimation;
    SpriteRenderer sr;
    Player player;
    public ShooterSettings settings;

    public bool friendly = false;
    public bool dead = false;
    bool hopping = false;
    int hp;
    int maxhp;

    bool ishurting = false;
    public float hurtTimer = 0.2f;

    private Vector3 followOffset;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        player = FindFirstObjectByType<Player>();
        Setup(settings);
    }

    private void Update()
    {
        if (dead) return;

        if (settings.rotate)
        {
            RotateToPlayer();
        }
        if (friendly)
        {
            FollowPlayer();
        }
        if (settings.follow)
        {
            MoveToPlayer();
        }
    }

    public void FollowPlayer()
    {
        Vector3 trajectory = (player.transform.position + followOffset) - transform.position;
        if (trajectory.magnitude > Time.deltaTime * settings.speed && !hopping)
        {
            StartCoroutine(HoppingRoutine(0.5f));

        }

        if (trajectory.magnitude > 1 && !hopping) {
            trajectory.Normalize();
        }
        transform.position += trajectory * settings.speed * Time.deltaTime;
    }


    public void MoveToPlayer()
    {
        Vector3 trajectory = player.transform.position - transform.position;
        if (trajectory.magnitude > 1)
        {
            trajectory.Normalize();
            transform.position += trajectory * settings.speed * Time.deltaTime;
        }
    }

    public void RotateToPlayer()
    {
        Vector3 trajectory = player.transform.position - transform.position;

        if (friendly && trajectory.magnitude < settings.followRadius * 1.5f)
        {
            trajectory.y = 0;
            trajectory.x = -trajectory.x;
        }

        trajectory.Normalize();
        if (trajectory.x < 0)
        {
            trajectory = -trajectory;
            if (transform.localScale.x > 0)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            if (transform.localScale.x < 0)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        transform.right = trajectory;
    }

    public void Feed(Transform source)
    {
        GetComponent<Shooter>().Passify();
        friendly = true;
        followOffset = (transform.position - source.position).normalized * settings.followRadius;
        heartAnimation = Instantiate<GameObject>(heartAnimationPrefab, transform.position, Quaternion.identity);
        heartAnimation.GetComponent<HeartAnimation>().target = transform;
        sr.color = Color.magenta;

        ScoreManager.score += 100;
        UIManager.UpdateScore();
    }

    void Setup(ShooterSettings settings)
    {
        hp = settings.hp;
        maxhp = hp;
        GetComponent<Shooter>().Init(settings.attack);
    }

    IEnumerator HoppingRoutine(float animationTimer)
    {
        hopping = true;
        Transform graphics = sr.transform;
        Vector3 startpos = graphics.localPosition;
        Vector3 targetPos = graphics.localPosition + Vector3.up * 0.4f;
        float normalizedTime = 0;
        float timer = 0;
        while (normalizedTime < 1)
        {
            timer += Time.deltaTime;
            normalizedTime = timer / animationTimer;
            float scaleX = 1 - Mathf.Sin(normalizedTime * Mathf.PI * 2) * 0.2f;
            float scaleY = 1 + Mathf.Sin(normalizedTime * Mathf.PI * 2) * 0.2f;
            Vector3 scale = new Vector3(scaleX, scaleY, 1);
            graphics.localPosition = Vector3.Lerp(startpos, targetPos, Mathf.Sin(normalizedTime * Mathf.PI * 2));
            graphics.localScale = scale;
            yield return null;
        }
        graphics.localPosition = startpos;
        graphics.localScale = new Vector3(1, 1, 1);

        hopping = false;
    }

    public void Hurt(Projectile projectile)
    {
        if (ishurting)
        {
            return;
        }
        ishurting = true;
        hp--;
        sr.color = Color.Lerp(Color.grey, Color.magenta, (float)hp / maxhp);
        //UIHealthPanel.instance.SetLives(maxHP, hp);
        if (hp <= 0)
        {
            Die();
        }
        StartCoroutine(HurtRoutine());
    }

    IEnumerator HurtRoutine()
    {
        float startTime = Time.time;
        Color color = sr.color;
        while (startTime + hurtTimer > Time.time)
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(0.05f);
            sr.color = Color.white;
            yield return new WaitForSeconds(0.05f);
        }
        sr.color = color;
        ishurting = false;
    }

    void Die()
    {
        dead = true;
        Destroy(GetComponent<Collider2D>());
        Destroy(heartAnimation);
        StopAllCoroutines();
        Transform graphics = sr.transform;
        StartCoroutine(DieRoutine(1f));
    }

    IEnumerator DieRoutine(float animationTimer)
    {
        transform.rotation = Quaternion.identity;
        Transform graphics = sr.transform;
        Vector3 startpos = graphics.localPosition;
        Vector3 targetPos = graphics.localPosition + Vector3.up * 1f;
        float normalizedTime = 0;
        float timer = 0;
        while (normalizedTime < 1)
        {
            timer += Time.deltaTime;
            normalizedTime = timer / animationTimer;
            float scaleY = Mathf.Sin(normalizedTime * Mathf.PI);
            Vector3 scale = new Vector3(1, scaleY, 1);
            graphics.localPosition = Vector3.Lerp(startpos, targetPos, Mathf.Sin(normalizedTime * Mathf.PI * 2f));
            graphics.localScale = scale;
            yield return null;
        }
        graphics.localPosition = startpos;
        graphics.localScale = new Vector3(1,-1,1);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}

using UnityEngine;
using System.Collections;

public class HeartAnimation : MonoBehaviour
{
    SpriteRenderer sr;
    public Transform target;
    public Transform graphics;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        StartCoroutine(AnimationRoutine());
        transform.position = target.position;
    }

    private void Update()
    {
        if(target == null)
        {
            Destroy(gameObject);
        }else
            transform.position = target.position;
    }

    IEnumerator AnimationRoutine()
    {
        yield return StartCoroutine(AnimateRotation(0.5f));
        yield return StartCoroutine(AnimateBlinking(0.5f));
        yield return StartCoroutine(AnimateFade(1.0f));
    }

    IEnumerator AnimateRotation(float animationTimer)
    {
        Vector3 scaling = graphics.localScale;
        Vector3 startpos = graphics.localPosition;
        Vector3 targetPos = graphics.localPosition + Vector3.up * 1;
        float normalizedTime = 0;
        float timer = 0;
        while (normalizedTime < 1)
        {
            timer += Time.deltaTime;
            normalizedTime = timer / animationTimer;
            float factor = Mathf.Sin(normalizedTime * Mathf.PI * 2);
            Vector3 scale = new Vector3(factor, normalizedTime, normalizedTime);
            graphics.localPosition = Vector3.Lerp(startpos, targetPos, normalizedTime);
            graphics.localScale = scale;
            yield return null;
        }
        graphics.localPosition = targetPos;
        graphics.localScale = scaling;
    }

    IEnumerator AnimateBlinking(float animationTimer)
    {
        Vector3 scaling = graphics.localScale;
        float normalizedTime = 0;
        float timer = 0;
        Color srColor = sr.color;
        while (normalizedTime < 1)
        {
            timer += Time.deltaTime;
            normalizedTime = timer / animationTimer;
            float factor = 1 + Mathf.Sin(normalizedTime * Mathf.PI) * .3f;
            graphics.localScale = scaling * factor;
            sr.color = Color.Lerp(Color.white, Color.red, Mathf.Abs(Mathf.Sin(timer * 10f)));
            yield return null;
        }
        graphics.localScale = scaling;
        sr.color = srColor;
    }

    IEnumerator AnimateFade(float animationTimer)
    {
        float normalizedTime = 0;
        float timer = 0;
        Color color = sr.color;
        Color transparent = color;
        transparent.a = 0;
        while (normalizedTime < 1)
        {
            timer += Time.deltaTime;
            normalizedTime = timer / animationTimer;
            float factor = Mathf.Cos(normalizedTime * Mathf.PI * 2);
            Vector3 scale = new Vector3(factor, 1-normalizedTime, 1);
            sr.color = Color.Lerp(color, transparent, 1-normalizedTime);
            graphics.localScale = scale;
            yield return null;
        }
        Destroy(gameObject);
    }
}

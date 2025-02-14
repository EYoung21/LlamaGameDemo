using UnityEngine;

public class CarrotProjectile : MonoBehaviour
{
    Rigidbody2D rb2d;
    public GameObject carrotPrefab;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(rb2d.linearVelocity.magnitude < 0.1f)
        {
            Instantiate(carrotPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        Llama llama = other.GetComponent<Llama>();

        if (llama != null && !llama.friendly)
        {
            llama.Feed(transform);
            Destroy(gameObject);
        }
    }
}

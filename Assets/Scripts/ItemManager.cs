using System;
using System.Collections;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    Carrot item;

    public GameObject carrotProjectilePrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && item != null)
        {
            ThrowItem ();
        }
    }

    private void ThrowItem()
    {

        GameObject projectile = Instantiate(carrotProjectilePrefab, item.transform.position, Quaternion.identity);
        Rigidbody2D projectileRB = projectile.GetComponent<Rigidbody2D>();

        Vector2 trajectory = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        float magnitude = trajectory.magnitude;
        if (magnitude == 0) trajectory.x = -1;
        if (magnitude > 1) trajectory.Normalize();
        projectileRB.linearVelocity = trajectory * 20;

        Destroy(item.gameObject);
        item = null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Carrot carrot = other.GetComponent<Carrot>();
        if(carrot != null && item == null)
        {
            item = carrot;
            item.transform.SetParent(transform);

            item.transform.localPosition = new Vector3(0.25f, 0, 0);
            other.enabled = false;
        }

        Llama llama = other.GetComponent<Llama>();

        if(llama != null && item != null && !llama.friendly)
        {
            llama.Feed(transform);
            Destroy(item.gameObject);
            item = null;
        }
    }
}

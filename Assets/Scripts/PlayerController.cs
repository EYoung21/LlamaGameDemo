using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;

    Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float movementHorizontal = 0;
        float movementVertical = 0;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            movementVertical = 1;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            movementVertical = -1;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            movementHorizontal = 1;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            movementHorizontal = -1;
        }
        Vector2 movement = new Vector2(movementHorizontal, movementVertical);
        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }
        movement *= speed;
        rb2d.linearVelocity = movement;

    }
}

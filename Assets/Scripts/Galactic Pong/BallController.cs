using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody2D body;
    public float speed;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        body = GetComponent<Rigidbody2D>();
        transform.position = new Vector3(0, 0, 0);
        StartBall();
    }
    /// <summary>Initializes the ball in the scene with a starting force</summary>
    private void StartBall()
    {
        float value = Random.value;
        if (value > 0.5f)
        {
            body.AddForce(new Vector2(25, Random.Range(-15, 15)) * speed);
        }
        if (value < 0.5f)
        {
            body.AddForce(new Vector2(-25, Random.Range(-15, 15)) * speed);
        }
    }
    /// <summary>Upon collision change velocity of the ball by a random value</summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        audioSource.Play();
        Vector2 vel;
        if (body.velocity.x > 0f)
        {
            vel.x = body.velocity.x + Random.Range(0f, 10f);
        }
        else if (body.velocity.x < 0f)
        {
            vel.x = body.velocity.x + Random.Range(-10f, 0f);
        }
        else
        {
            vel.x = body.velocity.x + Random.Range(-0.1f, 0.1f);
        }
        if (body.velocity.y > 0f)
        {
            vel.y = body.velocity.y + Random.Range(0f, 10f) + (collision.collider.attachedRigidbody.velocity.y / 3);
        }
        else if (body.velocity.y < 0f)
        {
            vel.y = body.velocity.y + Random.Range(-10f, 0f) + (collision.collider.attachedRigidbody.velocity.y / 3);
        }
        else
        {
            vel.y = (body.velocity.y / 2) + (collision.collider.attachedRigidbody.velocity.y / 3);
        }
        body.AddForce(vel);
    }
}

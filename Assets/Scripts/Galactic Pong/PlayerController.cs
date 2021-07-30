using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float yBounds;
    public float movementSpeed = 15;
    private GameManager gameManager;
    private BallHandler ballHandler;
    private float ballPos;
    private Rigidbody2D ballBod;
    private void Start()
    {
        yBounds = GameObject.Find("BG").GetComponent<SpriteRenderer>().size.y/2 - GetComponent<SpriteRenderer>().size.y/2;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        ballHandler = GameObject.Find("BallHandler").GetComponent<BallHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        Constrain();
        if (gameManager.gameStart)
        {
            transform.position = new Vector2(transform.position.x, yBounds / 2);
        }
        if (gameManager.gameRunning)
        {
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(0, vertical, 0);
            if (direction.magnitude > 0.1f)
            {
                transform.Translate(direction.normalized * movementSpeed * Time.deltaTime);
            }
        }
        else
        {
            if (ballHandler.ball)
            {
                ballBod = ballHandler.ball.GetComponent<Rigidbody2D>();
                if (ballBod.velocity.x < 0)
                {
                    ballPos = ballHandler.ball.transform.position.y;
                    if (transform.position.y < ballPos)
                    {
                        transform.Translate(new Vector3(0, 1, 0).normalized * movementSpeed * Time.deltaTime);
                    }
                    if (transform.position.y > ballPos)
                    {
                        transform.Translate(new Vector3(0, -1, 0) * movementSpeed * Time.deltaTime);
                    }
                }
            }
        }
    }
    private void Constrain()
    {
        if (transform.position.y > yBounds)
        {
            transform.position = new Vector3(transform.position.x, yBounds, transform.position.z);
        }
        if (transform.position.y < -yBounds)
        {
            transform.position = new Vector3(transform.position.x, -yBounds, transform.position.z);
        }
    }
}
